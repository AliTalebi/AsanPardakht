using AsanPardakht.WorkerService.MessageProcessorConsoleApp.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AsanPardakht.WorkerService.MessageProcessorConsoleApp.Data
{
    public sealed class ProcessDataDbContext : DbContext
    {
        public ProcessDataDbContext(DbContextOptions<ProcessDataDbContext> dbContextOptions)
            : base(dbContextOptions) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProcessedTehranCityAddress>(b =>
            {
                b.ToTable("ProcessedTehranCityAddresses");

                b.HasKey(x => x.Id);

                b.Property(x => x.Id).UseIdentityColumn().IsRequired();
                b.Property(x => x.AddressId).IsRequired(false);
                b.Property(x => x.Address).HasMaxLength(1000).IsRequired();
                b.Property(x => x.CityName).HasMaxLength(100).IsRequired();
                b.Property(x => x.ProvinceName).HasMaxLength(100).IsRequired();
            });
        }
    }
}
