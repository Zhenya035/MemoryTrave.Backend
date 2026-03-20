using MemoryTrave.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MemoryTrave.Infrastructure.Configurations;

public class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedNever();

        builder.Property(a => a.Visibility)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(a => a.CreatedAt)
            .IsRequired();
        builder.Property(a => a.LastChange)
            .IsRequired();
        
        builder.HasOne(a => a.Author)
            .WithMany(u => u.Articles)
            .HasForeignKey(a => a.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(a => a.Location)
            .WithMany(u => u.Articles)
            .HasForeignKey(a => a.LocationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}