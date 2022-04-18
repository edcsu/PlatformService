using AutoMapper;
using PlatformService.Business.Platform.Protos;
using PlatformService.Business.Platform.ViewModels;

namespace PlatformService.Business.Platform.MapperProfiles
{
    public class PlatformProfiles : Profile
    {
        public PlatformProfiles()
        {
            // source -> destination
            CreateMap<Models.Platform, PlatformDetails>();
            
            CreateMap<PlatformCreate, Models.Platform>();

            CreateMap<PlatformUpdate, Models.Platform>();

            CreateMap<PlatformDetails, PlatformPublished>();

            CreateMap<Models.Platform, GrpcPlatformModel>()
                .ForMember(dest => dest.PlatformId, options => options.MapFrom(src => src.Id.ToString()));
        }
    }
}
