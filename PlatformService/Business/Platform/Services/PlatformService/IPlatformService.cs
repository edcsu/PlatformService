using PlatformService.Business.Platform.ViewModels;

namespace PlatformService.Business.Platform.Services.PlatformService
{
    public interface IPlatformService
    {
        IEnumerable<PlatformDetails> GetAllPlatformsAsync();

        PlatformDetails? GetPlatformByIdAsync(Guid id);

        PlatformDetails CreatePlatformAsync(PlatformCreate platformCreate);
        
        PlatformDetails? UpdatePlatformAsync(Guid id, PlatformUpdate platformUpdate);
    }
}
