using AutoMapper;
using MemoryTrave.Application.Dto.Responses.Friend;
using MemoryTrave.Domain.Models;

namespace MemoryTrave.Application.Mapping;

public class FriendRequestMappingProfile : Profile
{
    public FriendRequestMappingProfile()
    {
        CreateMap<FriendRequest, GetFriendDto>()
            .ForMember(dto => dto.FriendName,
                opt => opt.MapFrom(
                    src => src.FromUser != null ? src.FromUser.Username : src.ToUser.Username));
    }
}