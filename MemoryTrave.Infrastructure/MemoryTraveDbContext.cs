using MemoryTrave.Domain.Models;
using MemoryTrave.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace MemoryTrave.Infrastructure;

public class MemoryTraveDbContext(DbContextOptions<MemoryTraveDbContext> options) : DbContext(options)
{
    public DbSet<Article> Articles { get; set; }
    public DbSet<ArticleAccess>  ArticleAccesses { get; set; }
    public DbSet<FriendRequest> FriendRequests { get; set; }
    public DbSet<Friendship>  Friendships { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ArticleConfiguration());
        modelBuilder.ApplyConfiguration(new ArticleAccessConfiguration());
        modelBuilder.ApplyConfiguration(new FriendRequestConfiguration());
        modelBuilder.ApplyConfiguration(new FriendshipConfiguration());
        modelBuilder.ApplyConfiguration(new LocationConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
}