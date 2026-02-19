using FluentValidation;
using MemoryTrave.Application.Interfaces.User;
using MemoryTrave.Application.Services;
using MemoryTrave.Application.Services.User;
using MemoryTrave.Application.Validators.Requests.User;

namespace MemoryTrave.Web.Extensions;

public static class ApplicationLayerExtensions
{
    public static void AddApplicationLayer(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<RegistrationRequestValidator>();

        services.AddScoped<IAuthorizationUseCase, AuthorizationUseCase>();
        services.AddScoped<IRegistrationUseCase, RegistrationUseCase>();
        services.AddScoped<IUserService, UserService>();
    }
}