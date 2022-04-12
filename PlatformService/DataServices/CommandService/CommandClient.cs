using Microsoft.Extensions.Options;
using PlatformService.Business.Platform.ViewModels;

namespace PlatformService.DataServices.CommandService
{
    public class CommandClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CommandClient> _logger;
        private readonly IOptions<CommandServiceConfig> _commandOptions;

        public CommandClient(HttpClient httpClient, 
            ILogger<CommandClient> logger, 
            IOptions<CommandServiceConfig> commandOptions)
        {
            _logger = logger;
            _commandOptions = commandOptions;

            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(_commandOptions.Value.BaseUrl);
        }

        public async Task SendPlatformAsync(PlatformDetails platformDetails)
        {
            var httpResponseMessage = await _httpClient.PostAsJsonAsync(_commandOptions.Value.PlatformsEndpoint, platformDetails);

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
