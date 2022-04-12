namespace PlatformService.DataServices.CommandService
{
#nullable disable
    public class CommandServiceConfig
    {
        public const string ConfigurationName = "CommandServiceConfig";

        public string BaseUrl { get; set; }
        public string ClientName { get; set; }
        public string PlatformsEndpoint { get; set; }
    }
}
