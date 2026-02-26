using FluentValidation;
using MemoryTrave.Application.Interfaces.User;
using MemoryTrave.Application.Mapping;
using MemoryTrave.Application.Services.User;
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

        services.AddScoped<IUserService, UserService>();
    }
}