using AsanPardakht.Queries.Infrastructure.DomainModel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsanPardakht.Queries.Persistence.EfCore.Data.Configurations
{
    internal sealed class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.ToTable("People", "People");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Id).ValueGeneratedNever().IsRequired();
            builder.Property(x => x.NationalCode).HasColumnType("varchar").HasMaxLength(10).IsRequired();

            builder.HasIndex(x => x.NationalCode).HasDatabaseName("IX_PEOPLE_NATIONALCODE").IsUnique();

            builder.OwnsMany(x => x.Addresses, b =>
            {
                b.ToTable("PeopleAddresses", "People");

                b.HasKey(x => x.Id);
                b.Property(x => x.Id).UseIdentityColumn().IsRequired();

                b.Property(x => x.CityId).IsRequired();
                b.Property(x => x.PersonId).IsRequired();
                b.Property(x => x.ProvinceId).IsRequired();
                b.Property(x => x.Detail).HasMaxLength(1000).IsRequired();

                b.HasOne(x => x.City).WithMany().HasForeignKey(x => x.CityId).OnDelete(DeleteBehavior.NoAction);
                b.HasOne(x => x.Province).WithMany().HasForeignKey(x => x.ProvinceId).OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}
