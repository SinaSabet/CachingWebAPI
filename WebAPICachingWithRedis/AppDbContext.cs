
using Microsoft.EntityFrameworkCore;
using WebAPICachingWithRedis.Models;

namespace WebAPICachingWithRedis
{
    public class AppDbContext:DbContext
    {
        public DbSet<Driver> Drivers { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> dbContext):base(dbContext)
        {
            
        }
    }
}
