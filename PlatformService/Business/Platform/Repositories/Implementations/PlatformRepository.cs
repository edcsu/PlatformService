using PlatformService.Business.Platform.Repositories.Interfaces;
using PlatformService.Data.Context;

namespace PlatformService.Business.Platform.Repositories.Implementations
{
    public class PlatformRepository : IPlatformRepository
    {
        private readonly ApplicationDbContext _context;

        public PlatformRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void CreatePlatform(Models.Platform platform)
        {
            _context.Platforms.Add(platform);
        }

        public IEnumerable<Models.Platform> GetAllPlatforms()
        {
            return _context.Platforms.ToList();
        }

        public Models.Platform? GetPlatformById(Guid id)
        {
            return _context.Platforms.FirstOrDefault(p => p.Id == id);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void UpdatePlatform(Models.Platform platform)
        {
            _context.Platforms.Update(platform);
        }
    }
}
