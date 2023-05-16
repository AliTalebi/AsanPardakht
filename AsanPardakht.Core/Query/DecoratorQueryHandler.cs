using AsanPardakht.Core.Errors;
using AsanPardakht.Core.Resources;
using OneOf;

namespace AsanPardakht.Core.Query
{
    public sealed class DecoratorQueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult>
        where TQuery : IBaseQuery<TResult>
    {
        private readonly IResourceManager _resourceManager;
        private readonly IQueryHandler<TQuery, TResult> _next;

        public DecoratorQueryHandler(IResourceManager resourceManager, IQueryHandler<TQuery, TResult> next)
        {
            _next = next;
            _resourceManager = resourceManager;
        }

        public async Task<OneOf<TResult, Error>> ExecuteAsync(TQuery query, CancellationToken cancellationToken)
        {
            var queryResult = await _next.ExecuteAsync(query, cancellationToken);

            return queryResult.Match<OneOf<TResult, Error>>(result => result, error => new Error(_resourceManager[error.Message], error.Code));
        }
    }
}
