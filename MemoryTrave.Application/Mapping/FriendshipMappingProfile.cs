using AutoMapper;
using MemoryTrave.Application.Dto.Responses.Friend;
using MemoryTrave.Domain.Models;

namespace MemoryTrave.Application.Mapping;

public class FriendshipMappingProfile : Profile
{
    public FriendshipMappingProfile()
    {
        CreateMap<Friendship, GetFriendshipDto>()
            .ForMember(dto => dto.FriendName,
                opt => opt.MapFrom((src, dest, member, context) =>
                {
                    var currentUserId = (Guid)context.Items["UserId"];

                    return src.UserId == currentUserId
                        ? src.Friend.Username
                        : src.User.Username;
                }));
    }
}