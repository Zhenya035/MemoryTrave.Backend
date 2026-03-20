using AutoMapper;
using MemoryTrave.Application.Dto.Requests.Location;
using MemoryTrave.Application.Dto.Responses.Location;
using MemoryTrave.Domain.Models;

namespace MemoryTrave.Application.Mapping;

public class LocationMappingProfile : Profile
{
    public LocationMappingProfile()
    {
        CreateMap<Location, GetLocationDto>()
            .ForMember(dto => dto.Articles,
                opt => opt.MapFrom(
                    src => src.Articles));

        CreateMap<Location, GetAllLocationDto>();

        CreateMap<AddAndUpdateLocationDto, Location>()
            .ForMember(mod => mod.Id,
                opt => opt.Ignore())
            .ForMember(mod => mod.Geohash,
                opt => opt.Ignore());
    }
}