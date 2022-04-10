using Microsoft.EntityFrameworkCore;
using PlatformService.Business.Platform.Models;

namespace PlatformService.Data.Context
{
#nullable disable
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Platform> Platforms { get; set; }
    }
}
