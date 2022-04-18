using AutoMapper;
using Grpc.Core;
using PlatformService.Business.Platform.Protos;
using PlatformService.Business.Platform.Repositories.Interfaces;
using PlatformService.Business.Platform.Services.PlatformService;

namespace PlatformService.DataServices.Grpc
{
    public class GrpcPlatformSerivce : GrpcPlatform.GrpcPlatformBase
    {
        private readonly IPlatformRepository _platformRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GrpcPlatformSerivce> _logger;

        public GrpcPlatformSerivce(IPlatformRepository platformRepository, 
            IMapper mapper, 
            ILogger<GrpcPlatformSerivce> logger)
        {
            _platformRepository = platformRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public override Task<PlatformResponse> GetAllPlatforms(GetAllRequest request, ServerCallContext context)
        {
            var response = new PlatformResponse();
            var platforms = _platformRepository.GetAllPlatforms();
            foreach (var platform in platforms)
            {
                response.Platform.Add(_mapper.Map<GrpcPlatformModel>(platform));
            }

            return Task.FromResult(response);
        }
    }
}
