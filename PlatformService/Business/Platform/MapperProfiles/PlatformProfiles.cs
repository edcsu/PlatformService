using AutoMapper;
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
        }
    }
}
