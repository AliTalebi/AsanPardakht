using OneOf;
using AsanPardakht.Core.Errors;
using AsanPardakht.Core.Command;
using AsanPardakht.Core.Query;
using AsanPardakht.Core.Domain.Events;

namespace AsanPardakht.Core.Dipatcher
{
    public interface IDispatcher
    {
        Task<OneOf<bool, Error>> DispachAsync<TCommand>(TCommand command) where TCommand : IBaseCommand;
        Task<OneOf<TResult, Error>> ExecuteQueryAsync<TResult>(IBaseQuery<TResult> query);
        Task NotifyEventAsync<TEvent>(TEvent @event) where TEvent : class, IDomainEvent;
    }
}
