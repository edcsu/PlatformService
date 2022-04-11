namespace PlatformService.Business.Platform.Repositories.Interfaces
{
    public interface IPlatformRepository
    {
        bool SaveChanges();

        IEnumerable<Models.Platform> GetAllPlatforms();

        Models.Platform? GetPlatformById(Guid id);

        void CreatePlatform(Models.Platform platform);

        void UpdatePlatform(Models.Platform platform);
    }
}
