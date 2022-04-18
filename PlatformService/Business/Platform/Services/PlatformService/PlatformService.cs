using AutoMapper;
using PlatformService.AsyncDataServices;
using PlatformService.Business.Platform.Repositories.Interfaces;
using PlatformService.Business.Platform.ViewModels;
using PlatformService.DataServices.CommandService;

namespace PlatformService.Business.Platform.Services.PlatformService
{
    public class PlatformService : IPlatformService
    {
        private readonly IPlatformRepository _platformRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PlatformService> _logger;
        private readonly CommandClient _commandClient;
        private readonly IMessageBusClient _messageBusClient;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PlatformService(IPlatformRepository platformRepository,
            IMapper mapper,
            ILogger<PlatformService> logger,
            CommandClient commandClient,
            IMessageBusClient messageBusClient, IWebHostEnvironment webHostEnvironment)
        {
            _platformRepository = platformRepository;
            _mapper = mapper;
            _logger = logger;
            _commandClient = commandClient;
            _messageBusClient = messageBusClient;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<PlatformDetails> CreatePlatformAsync(PlatformCreate platformCreate)
        {
            var platform = _mapper.Map<Models.Platform>(platformCreate);
            platform.Created = DateTime.UtcNow;
            
            _platformRepository.CreatePlatform(platform);
            await _platformRepository.SaveChangesAsync();
            _logger.LogInformation("Created a new platform with id:{PlatformId}", platform.Id);

            var platformDetails = _mapper.Map<PlatformDetails>(platform);

            // Send sync message
            if (_webHostEnvironment.IsDevelopment())
            {
                try
                {
                    await _commandClient.SendPlatformAsync(platformDetails);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex, "Failed to send new platform details with id:{PlatformId} synchronously", platform.Id);
                    //throw;
                }
            }

            // send async
            try
            {
                var publishedDto = _mapper.Map<PlatformPublished>(platformDetails);
                publishedDto.Event = PlatformEvents.PlatFormPublished;

                _logger.LogInformation("Sending new platform details with id:{PlatformId} to commands service", platform.Id);

                _messageBusClient.PublishNewPlatform(publishedDto);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Failed to send new platform details with id:{PlatformId} async", platform.Id);

                //throw;
            }

            return platformDetails;
        }

        public IEnumerable<PlatformDetails> GetAllPlatformsAsync()
        {
            var platforms = _platformRepository.GetAllPlatforms();
            return _mapper.Map<IEnumerable<PlatformDetails>>(platforms);
        }

        public PlatformDetails? GetPlatformByIdAsync(Guid id)
        {
            var platform = _platformRepository.GetPlatformById(id);
            return _mapper.Map<PlatformDetails>(platform);
        }

        public async Task<PlatformDetails?> UpdatePlatformAsync(Guid id, PlatformUpdate platformUpdate)
        {
            var platform = _platformRepository.GetPlatformById(id);
            if (platform is null)
            {
                _logger.LogInformation("Platform with ID: {PlatformId} does not exist", id);
                return null;
            }

            var updatedPlatform = _mapper.Map<Models.Platform>(platformUpdate);
            updatedPlatform.Id = id;
            updatedPlatform.Updated = DateTime.UtcNow;

            _platformRepository.UpdatePlatform(updatedPlatform);
            await _platformRepository.SaveChangesAsync();
            _logger.LogInformation("Updated a new platform with id:{PlatformId}", updatedPlatform.Id);

            return _mapper.Map<PlatformDetails>(updatedPlatform);
        }
    }
}
