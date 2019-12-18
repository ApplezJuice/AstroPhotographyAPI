using Microsoft.EntityFrameworkCore;

namespace AstroPhotographyAPI.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        public DbSet<DBPhoto> Photos { get; set; }
    }
}
