using PlatformService.DataServices.CommandService;

namespace PlatformService.Business.Platform.Config
{
    public static class ConfigurationExtensions
    {
        public static CommandServiceConfig GetCommandServiceConfig(this IConfiguration configuration)
        {
            return configuration.GetSection("CommandServiceConfig").Get<CommandServiceConfig>();
        }

        public static SeqConfig GetSeqSettings(this IConfiguration configuration)
        {
            return configuration.GetSection("Seq").Get<SeqConfig>();
        }
    }
}
