using AsanPardakht.Domain.Cities;
using AsanPardakht.Domain.Provinces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsanPardakht.Infrastructre.Persistence.EfCore.Configurations
{
    internal sealed class ProvinceConfiguration : IEntityTypeConfiguration<Province>
    {
        public void Configure(EntityTypeBuilder<Province> builder)
        {
            builder.ToTable("Provinces", "BaseInformation");

            builder.Ignore(x => x.DomainEvents);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Id).ValueGeneratedNever().IsRequired().HasConversion(to => to.Value, from => new ProvinceId(from));
        }
    }
}
