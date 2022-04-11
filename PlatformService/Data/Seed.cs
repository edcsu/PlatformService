using PlatformService.Data.Context;
using Serilog;

namespace PlatformService.Data
{
    public static class Seed
    {
        public static void PopulateDb(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            SeedData(serviceScope.ServiceProvider.GetService<ApplicationDbContext>()!);
        }

        private static void SeedData(ApplicationDbContext context)
        {
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
                        Publisher = "Vloud Native Computing Foundation",
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
