using AsanPardakht.Infrastructre.Persistence.EfCore.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsanPardakht.Infrastructre.Persistence.EfCore.Configurations
{
    internal class GeneratedNewIdViewConfiguration : IEntityTypeConfiguration<GeneratedNewIdView>
    {
        public void Configure(EntityTypeBuilder<GeneratedNewIdView> builder)
        {
            builder.ToTable("GeneratedNewIdView", b => b.ExcludeFromMigrations());

            builder.HasNoKey();

            builder.Property(x => x.Id).IsRequired();
        }
    }
}
