using MemoryTrave.Application.Dto.Requests.User;
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
}