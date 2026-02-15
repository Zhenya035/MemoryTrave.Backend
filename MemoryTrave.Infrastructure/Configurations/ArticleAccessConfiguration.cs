using MemoryTrave.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MemoryTrave.Infrastructure.Configurations;

public class ArticleAccessConfiguration : IEntityTypeConfiguration<ArticleAccess>
{
    public void Configure(EntityTypeBuilder<ArticleAccess> builder)
    {
        builder.HasKey(aa => aa.Id);
        builder.Property(aa => aa.Id).ValueGeneratedNever();

        builder.Property(aa => aa.EncryptedKey)
            .IsRequired();
        
        builder.HasOne(aa => aa.User)
            .WithMany(u => u.ArticleAccesses)
            .HasForeignKey(aa => aa.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(aa => aa.Article)
            .WithMany(a => a.EncryptedKeys)
            .HasForeignKey(aa => aa.ArticleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}