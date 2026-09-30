using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DataAccessLayer.Concrate
{
    /// <summary>Wird nur von "dotnet ef" (Migrationen) zur Entwurfszeit verwendet.</summary>
    public class DesignTimeContextFactory : IDesignTimeDbContextFactory<Context>
    {
        public Context CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<Context>()
                .UseSqlite(Context.DefaultConnection)
                .Options;
            return new Context(options);
        }
    }
}
