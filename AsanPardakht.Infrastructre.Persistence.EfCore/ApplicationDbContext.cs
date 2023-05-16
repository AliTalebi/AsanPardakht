using Microsoft.EntityFrameworkCore;

namespace AsanPardakht.Infrastructre.Persistence.EfCore
{
    public sealed class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions)
            : base(dbContextOptions) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasSequence<int>("Cities").HasMin(1).HasMax(int.MaxValue).StartsAt(1).IncrementsBy(1).IsCyclic(false);
            modelBuilder.HasSequence<int>("People").HasMin(1).HasMax(int.MaxValue).StartsAt(1).IncrementsBy(1).IsCyclic(false);
            modelBuilder.HasSequence<int>("Provinces").HasMin(1).HasMax(int.MaxValue).StartsAt(1).IncrementsBy(1).IsCyclic(false);

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}