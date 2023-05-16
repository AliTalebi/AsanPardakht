namespace AsanPardakht.Core.Query
{
    public sealed class PagingQueryResponse<TResponse> : QueryResponse<TResponse>
    {
        private PagingQueryResponse(int totalCount, TResponse data)
            : base(data)
        {
            TotalCount = totalCount;
        }

        public int TotalCount { get; set; }

        public static PagingQueryResponse<TResponse> Create(int totalCount, TResponse data)
        {
            return new PagingQueryResponse<TResponse>(totalCount, data);
        }
    }
}
