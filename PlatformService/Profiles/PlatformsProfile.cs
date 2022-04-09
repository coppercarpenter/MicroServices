using AutoMapper;
using PlatformService.DTOs;
using PlatformService.Models;

namespace PlatformService.Profiles
{
    public class PlatformsProfile : Profile
    {
        public PlatformsProfile()
        {
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<PlatformCreateDto, Platform>();
            CreateMap<PlatformReadDto, PlatformPublishedDto>();
            CreateMap<Platform, GrpcPlatformModel>()
                .ForMember(f => f.PlatformId, opt => opt.MapFrom(m => m.Id))
                .ForMember(f => f.Name, opt => opt.MapFrom(m => m.Name))
                .ForMember(f => f.Publisher, opt => opt.MapFrom(m => m.Publisher));
        }
    }
}
