using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Models;
using PlatformService;

namespace CommandsService.Profiles
{
    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<CommandCreateDto, Command>();
            CreateMap<Command, CommandReadDto>();
            CreateMap<PlatformPublishedDto, Platform>().ForMember(f => f.External_Id, options => options.MapFrom(m => m.Id));
            CreateMap<GrpcPlatformModel, Platform>()
                .ForMember(f => f.External_Id, options => options.MapFrom(m => m.PlatformId))
                .ForMember(f => f.Name, options => options.MapFrom(m => m.Name))
                .ForMember(f => f.Commands, options => options.Ignore());
        }
    }
}