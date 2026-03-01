using FluentValidation;
using MemoryTrave.Application.Interfaces;
using MemoryTrave.Application.Interfaces.Article;
using MemoryTrave.Application.Interfaces.Friend;
using MemoryTrave.Application.Interfaces.User;
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
        services.AddScoped<IUserService, UserService>();
    }
}