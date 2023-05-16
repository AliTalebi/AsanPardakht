namespace AsanPardakht.Core.Data
{
    public static class DataExtensions
    {
        public static IQueryable<TResult> UsePaging<TResult>(this IQueryable<TResult> query, int page = 1, int pageSize = 10)
        {
            if (page <= 0)
            {
                page = 1;
            }

            if (pageSize <= 0)
            {
                pageSize = 10;
            }

            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
}
