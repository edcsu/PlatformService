using AutoMapper;
using PlatformService.Business.Platform.Repositories.Interfaces;
using PlatformService.Business.Platform.ViewModels;

namespace PlatformService.Business.Platform.Services.PlatformService
{
    public class PlatformService : IPlatformService
    {
        private readonly IPlatformRepository _platformRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PlatformService> _logger;

        public PlatformService(IPlatformRepository platformRepository, IMapper mapper, ILogger<PlatformService> logger)
        {
            _platformRepository = platformRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public PlatformDetails CreatePlatformAsync(PlatformCreate platformCreate)
        {
            var platform = _mapper.Map<Models.Platform>(platformCreate);
            platform.Created = DateTime.UtcNow;
            
            _platformRepository.CreatePlatform(platform);
            _platformRepository.SaveChanges();
            _logger.LogInformation("Created a new platform with id:{PlatformId}", platform.Id);
            
            return _mapper.Map<PlatformDetails>(platform);
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

        public PlatformDetails? UpdatePlatformAsync(Guid id, PlatformUpdate platformUpdate)
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
            _platformRepository.SaveChanges();
            _logger.LogInformation("Updated a new platform with id:{PlatformId}", updatedPlatform.Id);

            return _mapper.Map<PlatformDetails>(updatedPlatform);
        }
    }
}
