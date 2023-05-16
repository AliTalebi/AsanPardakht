using AsanPardakht.Domain.Cities;
using AsanPardakht.Domain.Provinces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsanPardakht.Infrastructre.Persistence.EfCore.Configurations
{
    internal sealed class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.ToTable("Cities", "BaseInformation");

            builder.Ignore(x => x.DomainEvents);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
            builder.Property(x => x.ProvinceId).IsRequired().HasConversion(to => to.Value, from => new ProvinceId(from));

            builder.Property(x => x.Id).ValueGeneratedNever().IsRequired().HasConversion(to => to.Value, from => new CityId(from));
            builder.HasOne<Province>().WithMany().HasForeignKey(x => x.ProvinceId).IsRequired().OnDelete(DeleteBehavior.NoAction);

        }
    }
}
