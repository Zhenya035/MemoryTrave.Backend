using MemoryTrave.Application.Dto.Requests.User;
using MemoryTrave.Application.Dto.Responses.User;
using MemoryTrave.Domain.Models;

namespace MemoryTrave.Application.Mapping;

public static class UserMapping
{
    public static User MapFromRegistrationDto(RegistrationDto regUser) =>
        new()
        {
            Email = regUser.Email,
            Username =  regUser.Username
        };

    public static GetProfileDto MapToGetProfileDto(User user) =>
        new()
        {
            Username = user.Username,
            Email = user.Email,
            Role = user.Role.ToString(),
            ArticlesCount = user.Articles.Count,
            Articles = user.Articles.Select(ArticleMapping.MapToGetArticleForProfileDto).ToList(),
        };

    public static GetUserDto MapToGetUserDto(User user) =>
        new()
        {
            Id = user.Id,
            Username = user.Username
        };
}