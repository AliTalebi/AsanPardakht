using AsanPardakht.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsanPardakht.Infrastructre.Persistence.EfCore.Configurations
{
    internal sealed class OutBoxEventDataConfiguration : IEntityTypeConfiguration<OutBoxEventData>
    {
        public void Configure(EntityTypeBuilder<OutBoxEventData> builder)
        {
            builder.ToTable("OutBoxEvents", "Outbox");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn().IsRequired();
            builder.Property(x => x.ReadAt).IsRequired(false);
            builder.Property(x => x.Read).IsRequired().HasDefaultValue(false);
            builder.Property(x => x.Data).IsRequired();
            builder.Property(x => x.IssuedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            builder.Property(x => x.EventType).HasColumnType("varchar").HasMaxLength(1000).IsRequired();
            builder.Property(x => x.AggregateId).HasColumnType("varchar").HasMaxLength(100).IsRequired();
            builder.Property(x => x.AggregateType).HasColumnType("varchar").HasMaxLength(1000).IsRequired();
            builder.Property(x => x.IssuedBy).HasColumnType("varchar").HasMaxLength(50).IsRequired().HasDefaultValue("ali.talebi");
        }
    }
}
