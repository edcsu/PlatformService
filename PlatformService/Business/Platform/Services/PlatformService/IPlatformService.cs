using PlatformService.Business.Platform.ViewModels;

namespace PlatformService.Business.Platform.Services.PlatformService
{
    public interface IPlatformService
    {
        IEnumerable<PlatformDetails> GetAllPlatformsAsync();

        PlatformDetails? GetPlatformByIdAsync(Guid id);

        Task<PlatformDetails> CreatePlatformAsync(PlatformCreate platformCreate);

        Task<PlatformDetails?> UpdatePlatformAsync(Guid id, PlatformUpdate platformUpdate);
    }
}
