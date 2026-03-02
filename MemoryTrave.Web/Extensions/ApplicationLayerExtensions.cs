using FluentValidation;
using MemoryTrave.Application.Interfaces;
using MemoryTrave.Application.Mapping;
using MemoryTrave.Application.Services;
using MemoryTrave.Application.Validators.Requests.User;

namespace MemoryTrave.Web.Extensions;

public static class ApplicationLayerExtensions
{
    public static void AddApplicationLayer(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<RegistrationRequestValidator>();
        services.AddAutoMapper(
            cfg => { }, 
            typeof(UserMappingProfile).Assembly);

        services.AddScoped<IValidationService, ValidationService>();
        services.AddScoped<IArticleService, ArticleService>();
        services.AddScoped<IFriendRequestService, FriendRequestService>();
        services.AddScoped<IFriendshipService, FriendshipService>();
        services.AddScoped<IUserService, UserService>();
    }
}