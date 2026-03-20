using MemoryTrave.Domain.Interfaces;
using MemoryTrave.Infrastructure.Cloudflare;
using MemoryTrave.Infrastructure.Repositories;

namespace MemoryTrave.Web.Extensions;

public static class InfrastructureLayerExtensions
{
    public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IArticleAccessRepository, ArticleAccessRepository>();
        services.AddScoped<IArticleRepository, ArticleRepository>();
        services.AddScoped<IFriendRequestRepository, FriendRequestRepository>();
        services.AddScoped<IFriendshipRepository, FriendshipRepository>();
        services.AddScoped<ILocationRepository, LocationRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        
        services.AddScoped<IR2Service, R2Service>();
        services.AddScoped<ICloudStorageService, CloudStorageService>();
        
        services.Configure<R2Settings>(
            configuration.GetSection("Cloudflare"));
    }
}