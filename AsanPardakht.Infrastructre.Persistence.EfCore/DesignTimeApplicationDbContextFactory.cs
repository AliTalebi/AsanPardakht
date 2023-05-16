using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AsanPardakht.Infrastructre.Persistence.EfCore
{
    internal class DesignTimeApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer("Server=.;Initial Catalog=AsanPardakhtDb;Integrated Security=true;");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
