using Microsoft.EntityFrameworkCore;

namespace RESTcats.Models
// NuGet package Microsoft.EntityFrameworkCore.SqlServer
// same major version as your .NET version

{
    public class CatsDbContext : DbContext
    {
        public CatsDbContext(DbContextOptions<CatsDbContext> options) : base(options)
        {
        }
        public DbSet<Cat> Cats { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
