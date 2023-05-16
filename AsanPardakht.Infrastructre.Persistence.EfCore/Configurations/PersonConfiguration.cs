using AsanPardakht.Domain.Cities;
using AsanPardakht.Domain.People;
using AsanPardakht.Domain.Provinces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsanPardakht.Infrastructre.Persistence.EfCore.Configurations
{
    internal sealed class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.ToTable("People", "People");

            builder.Ignore(x => x.DomainEvents);
            builder.Ignore(x => x.Addresses);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
            builder.Property(x => x.NationalCode).HasColumnType("varchar").HasMaxLength(10).IsRequired();
            builder.Property(x => x.Id).ValueGeneratedNever().IsRequired().HasConversion(to => to.Value, from => new PersonId(from));

            builder.HasIndex(x => x.NationalCode).HasDatabaseName("IX_PEOPLE_NATIONALCODE").IsUnique();

            builder.OwnsMany<PersonAddress>("_addresses", b =>
            {
                b.ToTable("PeopleAddresses", "People");

                b.HasKey("Id");
                b.Property<int>("Id").UseIdentityColumn().IsRequired();

                b.Property(x => x.Detail).HasMaxLength(1000).IsRequired();
                b.Property(x => x.ProvinceId).IsRequired().HasConversion(to => to.Value, from => new ProvinceId(from));
                b.Property(x => x.CityId).IsRequired().HasConversion(to => to.Value, from => new CityId(from));

                b.HasOne<City>().WithMany().HasForeignKey(x => x.CityId).IsRequired().OnDelete(DeleteBehavior.NoAction);
                b.HasOne<Province>().WithMany().HasForeignKey(x => x.ProvinceId).IsRequired().OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}
