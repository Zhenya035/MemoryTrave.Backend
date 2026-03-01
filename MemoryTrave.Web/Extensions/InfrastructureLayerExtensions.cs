using MemoryTrave.Domain.Interfaces;
using MemoryTrave.Infrastructure.Repositories;

namespace MemoryTrave.Web.Extensions;

public static class InfrastructureLayerExtensions
{
    public static void AddInfrastructureLayer(this IServiceCollection services)
    {
        services.AddScoped<IArticleAccessRepository, ArticleAccessRepository>();
        services.AddScoped<IArticleRepository, ArticleRepository>();
        services.AddScoped<IFriendRequestRepository, FriendRequestRepository>();
        services.AddScoped<IFriendshipRepository, FriendshipRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
    }
}