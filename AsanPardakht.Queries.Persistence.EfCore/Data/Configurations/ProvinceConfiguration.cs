using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AsanPardakht.Queries.Infrastructure.DomainModel.Entities;

namespace AsanPardakht.Queries.Persistence.EfCore.Data.Configurations
{
    internal sealed class ProvinceConfiguration : IEntityTypeConfiguration<Province>
    {
        public void Configure(EntityTypeBuilder<Province> builder)
        {
            builder.ToTable("Provinces", "BaseInformation");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Id).ValueGeneratedNever().IsRequired();
        }
    }
}
