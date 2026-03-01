using AutoMapper;
using MemoryTrave.Application.Dto.Responses.Friend;
using MemoryTrave.Domain.Models;

namespace MemoryTrave.Application.Mapping;

public class FriendRequestMappingProfile : Profile
{
    public FriendRequestMappingProfile()
    {
        CreateMap<FriendRequest, GetFriendRequestDto>()
            .ForMember(dto => dto.FromUserName,
                opt => opt.MapFrom(
                    src => src.FromUser.Username))
            .ForMember(dto => dto.ToUserName,
                opt => opt.MapFrom(
                    src => src.ToUser.Username));
    }
}