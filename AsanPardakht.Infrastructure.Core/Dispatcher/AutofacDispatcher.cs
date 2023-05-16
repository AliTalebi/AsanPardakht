using OneOf;
using Autofac;
using AsanPardakht.Core.Query;
using AsanPardakht.Core.Errors;
using AsanPardakht.Core.Command;
using Microsoft.AspNetCore.Http;
using AsanPardakht.Core.Dipatcher;
using AsanPardakht.Core.Domain.Events;
using Autofac.Core.Lifetime;

namespace AsanPardakht.Infrastructure.Core.Dispatcher
{
    public sealed class AutofacDispatcher : IDispatcher
    {
        private readonly ILifetimeScope _lifetimeScope;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AutofacDispatcher(ILifetimeScope lifeTimeScope, IHttpContextAccessor httpContextAccessor)
        {
            _lifetimeScope = lifeTimeScope;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<OneOf<bool, Error>> DispachAsync<TCommand>(TCommand command) where TCommand : IBaseCommand
        {
            return await _lifetimeScope.Resolve<ICommandHandler<TCommand>>().ExecuteAsync(command, _httpContextAccessor.HttpContext.RequestAborted);
        }

        public async Task<OneOf<TResult, Error>> ExecuteQueryAsync<TResult>(IBaseQuery<TResult> query)
        {
            var queryType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));

            object resolvedDependency = _lifetimeScope.Resolve(queryType);

            var method = resolvedDependency.GetType().GetMethod("ExecuteAsync");

            var task = method?.Invoke(resolvedDependency, new object[] { query, _httpContextAccessor.HttpContext.RequestAborted }) as Task<OneOf<TResult, Error>>;

            return await task!;
        }

        public async Task NotifyEventAsync<TEvent>(TEvent @event) where TEvent : class, IDomainEvent
        {
            var registeredType = typeof(IDomainEventHandler<>).MakeGenericType(@event.GetType());

            if (_lifetimeScope.IsRegistered(registeredType))
            {
                var eventHandler = _lifetimeScope.Resolve(registeredType);

                if (eventHandler != null)
                {
                    var method = eventHandler.GetType().GetMethod("ExecuteAsync");

                    var task = method?.Invoke(eventHandler, new object[] { @event, _httpContextAccessor.HttpContext.RequestAborted }) as Task;

                    await task!;
                }
            }
        }
    }
}

