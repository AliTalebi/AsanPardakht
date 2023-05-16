namespace AsanPardakht.Core.Query
{
    public abstract class QueryResponse
    {
    }

    public class QueryResponse<TResponse> : QueryResponse
    {
        protected QueryResponse(TResponse? data)
        {
            Data = data;
        }

        public new TResponse? Data { get; set; }

        public static QueryResponse<TResponse> Create(TResponse? data)
        {
            return new QueryResponse<TResponse>(data);
        }
    }
}