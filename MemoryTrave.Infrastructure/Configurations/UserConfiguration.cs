using MemoryTrave.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MemoryTrave.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).ValueGeneratedNever();
        
        builder.Property(u=>u.Email).IsRequired();
        builder.HasIndex(u => u.Email).IsUnique();
        
        builder.Property(u => u.Username).IsRequired();
        builder.Property(u=>u.PasswordHash).IsRequired();
        builder.Property(u=>u.PublicKey).IsRequired();
        builder.Property(u=>u.EncryptedPrivateKey).IsRequired();
        builder.Property(u => u.Role)
            .IsRequired()
            .HasConversion<string>();
        
        builder.HasMany(u => u.SentFriendRequests)
            .WithOne(fr => fr.FromUser)
            .HasForeignKey(fr => fr.FromUserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(u => u.ReceivedFriendRequests)
            .WithOne(fr => fr.ToUser)
            .HasForeignKey(fr => fr.ToUserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(u => u.ArticleAccesses)
            .WithOne(aa => aa.User)
            .HasForeignKey(aa => aa.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(u => u.Articles)
            .WithOne(a => a.Author)
            .HasForeignKey(a => a.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}