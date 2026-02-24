using MemoryTrave.Application.Dto.Responses.User;
using MemoryTrave.Application.Interfaces;
using MemoryTrave.Application.Interfaces.Jwt;
using MemoryTrave.Application.Interfaces.Profile;
using MemoryTrave.Application.Interfaces.User;
using MemoryTrave.Application.Mapping;
using MemoryTrave.Domain.Exceptions;
using MemoryTrave.Domain.Interfaces;

namespace MemoryTrave.Application.Services.Profile;

public class ProfileService(
    IUserRepository userRepository,
    IFriendshipRepository friendRepository,
    ICurrentUserProvider userProvider) : IProfileService
{
    public async Task<GetProfileDto> GetProfile()
    {
        var userId = userProvider.GetUserId();
        
        var user = await userRepository.GetByIdWithArticles(userId);
        if (user == null)
            throw new NotFoundException("User");
        
        var response = UserMapping.MapToGetProfileDto(user);
        
        var friends = await friendRepository.GetAllFriends(user.Id);
        response.FriendsCount = friends.Count;
        
        return response;
    }

    public async Task<List<GetUserDto>> GetBlockUsers()
    {
        var userId = userProvider.GetUserId();
        
        var user = await userRepository.GetUserById(userId);
        if (user == null)
            throw new NotFoundException("User");

        if (user.BlockedUsers.Count == 0)
            return [];
        
        var blockUsers = await userRepository.GetBlockUsers(user.BlockedUsers);

        return blockUsers.Select(UserMapping.MapToGetUserDto).ToList();
    }
}