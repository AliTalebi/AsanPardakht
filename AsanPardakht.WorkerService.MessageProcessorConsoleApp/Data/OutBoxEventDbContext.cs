using AsanPardakht.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace AsanPardakht.WorkerService.MessageProcessorConsoleApp.Data
{
    public sealed class OutBoxEventDbContext : DbContext
    {
        public OutBoxEventDbContext(DbContextOptions<OutBoxEventDbContext> dbContextOptions)
            : base(dbContextOptions) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OutBoxEventData>(builder =>
            {
                builder.ToTable("OutBoxEvents", "Outbox");

                builder.HasKey(x=>x.Id);

                builder.Property(x => x.Data).IsRequired();
                builder.Property(x => x.Read).IsRequired();
                builder.Property(x => x.IssuedAt).IsRequired();
                builder.Property(x => x.IssuedBy).IsRequired();
                builder.Property(x => x.EventType).IsRequired();
                builder.Property(x => x.AggregateId).IsRequired();
                builder.Property(x => x.ReadAt).IsRequired(false);
                builder.Property(x => x.AggregateType).IsRequired();
                builder.Property(x => x.Id).UseIdentityColumn().IsRequired();
            });
        }
    }
}
