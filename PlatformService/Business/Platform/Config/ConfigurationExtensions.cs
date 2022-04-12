using PlatformService.DataServices.CommandService;

namespace PlatformService.Business.Platform.Config
{
    public static class ConfigurationExtensions
    {
        public static CommandServiceConfig GetCommandServiceConfig(this IConfiguration configuration)
        {
            return configuration.GetSection("CommandServiceConfig").Get<CommandServiceConfig>();
        }
    }
}
