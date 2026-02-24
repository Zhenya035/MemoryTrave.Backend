using MemoryTrave.Domain.Interfaces;
using MemoryTrave.Infrastructure.Repositories;

namespace MemoryTrave.Web.Extensions;

public static class InfrastructureLayerExtensions
{
    public static void AddInfrastructureLayer(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IFriendshipRepository, FriendshipRepository>();
    }
}