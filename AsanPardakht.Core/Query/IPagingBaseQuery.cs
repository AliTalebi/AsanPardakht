namespace AsanPardakht.Core.Query
{
    public interface IPagingBaseQuery<TResult> : IBaseQuery<TResult>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
