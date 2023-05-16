using Microsoft.EntityFrameworkCore;

namespace AsanPardakht.Queries.Persistence.EfCore.Data
{
    public sealed class QueryDbContext : DbContext
    {
        public QueryDbContext(DbContextOptions<QueryDbContext> dbContextOptions)
            : base(dbContextOptions) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
