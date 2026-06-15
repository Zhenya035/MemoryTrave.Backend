using AutoMapper;
using MemoryTrave.Application.Dto.Responses.Friend;
using MemoryTrave.Domain.Models;

namespace MemoryTrave.Application.Mapping;

public class FriendRequestMappingProfile : Profile
{
    public FriendRequestMappingProfile()
    {
        CreateMap<FriendRequest, GetOtherDto>()
            .ForMember(dto => dto.Name,
                opt => opt.MapFrom(
                    src => src.FromUser != null ? src.FromUser.Username : src.ToUser.Username))
            .ForMember(dto => dto.Email,
                opt => opt.MapFrom(
                    src => src.FromUser != null ? src.FromUser.Email : src.ToUser.Email));
    }
}