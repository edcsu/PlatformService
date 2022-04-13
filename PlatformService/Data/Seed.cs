using Microsoft.EntityFrameworkCore;
using PlatformService.Data.Context;
using Serilog;

namespace PlatformService.Data
{
    public static class Seed
    {
        public static void PopulateDb(IApplicationBuilder app, bool isProd)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            SeedData(serviceScope.ServiceProvider.GetService<ApplicationDbContext>()!, isProd);
        }

        private static void SeedData(ApplicationDbContext context, bool isProd)
        {
            if (isProd)
            {
                Log.Information("Trying to apply migraations to Database");
                context.Database.Migrate();
            }
            if (!context.Platforms.Any())
            {
                Log.Information("Seeding data to Database");
                var platforms = new List<Business.Platform.Models.Platform>() {
                    new Business.Platform.Models.Platform()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Dot Net",
                        Publisher = "Microsoft",
                        Cost = "Free",
                        Created = DateTime.UtcNow.AddDays(-20),
                        Updated = DateTime.UtcNow.AddDays(-10),
                    },
                    new Business.Platform.Models.Platform()
                    {
                        Id = Guid.NewGuid(),
                        Name = "SQL Server Express",
                        Publisher = "Microsoft",
                        Cost = "Free",
                        Created = DateTime.UtcNow.AddDays(-60),
                        Updated = DateTime.UtcNow.AddDays(-30),
                    },
                    new Business.Platform.Models.Platform()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Kubernetes",
                        Publisher = "Cloud Native Computing Foundation",
                        Cost = "Free",
                        Created = DateTime.UtcNow.AddDays(-90),
                        Updated = DateTime.UtcNow.AddDays(-5),
                    },
                };

                context.Platforms.AddRange(platforms);
                context.SaveChanges();
                Log.Information("Seeded data to the Database");
            }
            else
            {
                Log.Information("The Database has data");
            }
        }
    }
}
