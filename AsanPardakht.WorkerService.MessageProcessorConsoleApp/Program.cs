using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using AsanPardakht.WorkerService.MessageProcessorConsoleApp;
using AsanPardakht.WorkerService.MessageProcessorConsoleApp.Data;
using AsanPardakht.WorkerService.MessageProcessorConsoleApp.Outbox;
using AsanPardakht.WorkerService.MessageProcessorConsoleApp.Options;
using AsanPardakht.WorkerService.MessageProcessorConsoleApp.Outbox.Http;

var hostBuilder = Host.CreateDefaultBuilder(args);

hostBuilder.UseServiceProviderFactory(new AutofacServiceProviderFactory());

hostBuilder.ConfigureAppConfiguration(options =>
{
    options.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
});

hostBuilder.ConfigureServices((context, services) =>
{
    services.AddHttpClient();

    services.AddOptions<ApiOptions>().Bind(context.Configuration.GetSection(ApiOptions.SECTION_NAME)).ValidateOnStart().ValidateDataAnnotations();

    services.AddSingleton(new DbContextOptionsBuilder<OutBoxEventDbContext>().UseSqlServer(context.Configuration.GetConnectionString("WriteDataConnection")).UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll).Options);
    services.AddSingleton(new DbContextOptionsBuilder<ProcessDataDbContext>().UseSqlServer(context.Configuration.GetConnectionString("ProcessedDataConnection")).UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll).Options);

    services.AddHostedService<SyncDataWorkerService>();

    var dataStore = context.Configuration.GetSection("DataDestination:Store").Get<string>();

    switch (dataStore?.ToLower())
    {
        case "fromoutbox":
            services.AddSingleton<IReadOutboxEventTask, ReadOutboxSqlServerEventTask>();
            break;

        case "":
        case null:
        case "fromweb":
        default:
            services.AddSingleton<IReadOutboxEventTask, ReadOutboxHttpRequestTask>();
            break;
    }
});

await hostBuilder.Build().RunAsync();