using PlatformService.Business.Platform.ViewModels;

namespace PlatformService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishNewPlatform(PlatformPublished platformPublished);
    }
}
