using Microsoft.Extensions.Options;
using PlatformService.Business.Platform.ViewModels;

namespace PlatformService.DataServices.CommandService
{
    public class CommandClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<CommandClient> _logger;
        private readonly IOptions<CommandServiceConfig> _commandOptions;

        public CommandClient(IHttpClientFactory httpClientFactory, 
            ILogger<CommandClient> logger, 
            IOptions<CommandServiceConfig> commandOptions)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _commandOptions = commandOptions;
        }

        public async Task SendPlatformAsync(PlatformDetails platformDetails)
        {
            var httpClient = _httpClientFactory.CreateClient(_commandOptions.Value.ClientName);
            var httpResponseMessage = await httpClient.PostAsJsonAsync(_commandOptions.Value.PlatformsEndpoint, platformDetails);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var content = await httpResponseMessage.Content.ReadAsStringAsync();
                _logger.LogInformation("Sync to CommandService was OK Content: {Content}", content);
            }
            else
            {
                var content = await httpResponseMessage.Content.ReadAsStringAsync();
                _logger.LogError("Sync to CommandService failed StatusCode: {StatusCode} Content: {Content}", httpResponseMessage.StatusCode, content);
            }
        }
    }
}
