using Microsoft.EntityFrameworkCore;
using AsanPardakht.Queries.Infrastructure.DomainModel.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsanPardakht.Queries.Persistence.EfCore.Data.Configurations
{
    internal sealed class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.ToTable("Cities", "BaseInformation");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ProvinceId).IsRequired();
            builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Id).ValueGeneratedNever().IsRequired();

            builder.HasOne(x => x.Province).WithMany().HasForeignKey(x => x.ProvinceId).OnDelete(DeleteBehavior.NoAction);

        }
    }
}
