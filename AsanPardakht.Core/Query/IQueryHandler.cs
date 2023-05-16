using AsanPardakht.Core.Errors;
using OneOf;

namespace AsanPardakht.Core.Query
{
    public interface IQueryHandler { }

    public interface IQueryHandler<TQuery, TResult> : IQueryHandler
        where TQuery : IBaseQuery<TResult>
    {
        Task<OneOf<TResult, Error>> ExecuteAsync(TQuery query, CancellationToken cancellationToken = default!);
    }
}
