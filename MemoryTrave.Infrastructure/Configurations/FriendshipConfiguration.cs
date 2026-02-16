using MemoryTrave.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MemoryTrave.Infrastructure.Configurations;

public class FriendshipConfiguration : IEntityTypeConfiguration<Friendship>
{
    public void Configure(EntityTypeBuilder<Friendship> builder)
    {
        builder.HasKey(f => new {f.UserId, f.FriendId});
        
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(f => f.FriendId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}