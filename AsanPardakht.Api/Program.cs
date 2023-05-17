using Autofac;
using System.Globalization;
using AsanPardakht.Core.Data;
using AsanPardakht.Core.Query;
using AsanPardakht.Core.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using AsanPardakht.Api.Security;
using AsanPardakht.Core.Command;
using AsanPardakht.Core.Security;
using AsanPardakht.Core.Dipatcher;
using AsanPardakht.Core.Resources;
using AsanPardakht.Api.Middlewares;
using Microsoft.EntityFrameworkCore;
using AsanPardakht.Core.Domain.Events;
using AsanPardakht.Api.RouteConventions;
using AsanPardakht.Domain.People.Events;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Authorization;
using AsanPardakht.Queries.Queries.People;
using Microsoft.AspNetCore.Authentication;
using AsanPardakht.Application.EventHandlers;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Authorization;
using AsanPardakht.Application.Commands.Cities;
using AsanPardakht.Infrastructure.Core.Security;
using AsanPardakht.Infrastructure.Core.Resources;
using AsanPardakht.Infrastructure.Core.Dispatcher;
using AsanPardakht.Queries.Persistence.EfCore.Data;
using AsanPardakht.Infrastructre.Persistence.EfCore;
using AsanPardakht.Infrastructre.Persistence.EfCore.UnitOfWork;
using AsanPardakht.Infrastructre.Persistence.EfCore.Repositories;
using AsanPardakht.Infrastructre.Persistence.EfCore.DomainServices.Cities;

[assembly: ApiController]

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(container =>
{
    container.RegisterAssemblyTypes(typeof(PersonRepository).Assembly)
    .Where(x => typeof(IRepository).IsAssignableFrom(x))
    .AsImplementedInterfaces().InstancePerLifetimeScope();

    container.RegisterAssemblyTypes(typeof(CityExistByIdDomainService).Assembly)
    .Where(x => typeof(IDomainService).IsAssignableFrom(x))
    .AsImplementedInterfaces().InstancePerDependency();

    container.RegisterAssemblyTypes(typeof(CreateCityCommand).Assembly)
    .Where(x => typeof(ICommandHandler).IsAssignableFrom(x))
    .AsImplementedInterfaces().InstancePerLifetimeScope();

    container.RegisterAssemblyTypes(typeof(GetPersonByIdQuery).Assembly)
    .Where(x => x.IsClass && typeof(IQueryHandler).IsAssignableFrom(x))
    .AsImplementedInterfaces()
    .InstancePerLifetimeScope();

    container.RegisterAssemblyTypes(typeof(PersonAddressAddedEventHandler).Assembly)
    .Where(x => x.IsClass && typeof(IDomainEventHandler).IsAssignableFrom(x))
    .AsImplementedInterfaces()
    .InstancePerLifetimeScope();

    container.RegisterGenericDecorator(typeof(DecoratorCommandHandler<>), typeof(ICommandHandler<>));
    container.RegisterGenericDecorator(typeof(DecoratorQueryHandler<,>), typeof(IQueryHandler<,>));

    container.RegisterType<AspNetCoreResourceManager>().As<IResourceManager>().InstancePerLifetimeScope();
});

var services = builder.Services;

services.AddControllers(options =>
{
    options.Filters.Add(new AuthorizeFilter());
    options.Conventions.Add(new GlobalRouteConvention(new RouteAttribute("api")));
})
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

services.AddAuthentication("Basic")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", null);

services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
});

services.AddLocalization(options =>
{
    options.ResourcesPath = "";
});

services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")).UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
});

services.AddDbContext<QueryDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

services.AddSwaggerGen(config =>
{
    foreach (var item in Directory.EnumerateFiles(builder.Environment.ContentRootPath, builder.Environment.IsDevelopment() ? @"bin\AsanPardakht.*.xml" : "AsanPardakht.*.xml", SearchOption.AllDirectories))
        config.IncludeXmlComments(item, true);

    config.SwaggerDoc("v1", new OpenApiInfo { Title = "BasicAuth", Version = "v1" });
    config.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        In = ParameterLocation.Header,
        Description = "Basic Authorization header using the Bearer scheme."
    });
    config.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "basic"
                }
            },
            new string[] {}
        }
    });
});

services.AddCors(config =>
{
    config.AddPolicy("appCors", builder =>
    {
        builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
    });
});

services.AddHttpContextAccessor();

services.AddScoped<IUnitOfWork, EfCoreUnitOfWork>();
services.AddTransient<IDispatcher, AutofacDispatcher>();
services.AddScoped<IResourceManager, AspNetCoreResourceManager>();
services.AddScoped<IUserIdentityAccessor, AspNetCoreUserIdentityAccessor>();
services.AddTransient<ExceptionMiddleware>();
services.AddSingleton<IStringLocalizer, StringLocalizer<Messages>>();

services.AddAutoMapper(typeof(GetPersonByIdQuery).Assembly);

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseRouting();
app.UseCors("appCors");
app.UseAuthentication();
app.UseAuthorization();

app.UseRequestLocalization(config =>
{
    var supportedCultures = new List<CultureInfo>
    {
        new CultureInfo("en-US"),
        new CultureInfo("fa")
    };

    config.SupportedCultures = supportedCultures;
    config.SupportedUICultures = supportedCultures;

    config.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture(new CultureInfo("fa"));
});

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint($"/swagger/v1/swagger.json", "V1");

    options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);

    options.RoutePrefix = string.Empty;
});

app.Run();
