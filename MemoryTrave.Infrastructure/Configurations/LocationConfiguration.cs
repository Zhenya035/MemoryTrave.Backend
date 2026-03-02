using MemoryTrave.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MemoryTrave.Infrastructure.Configurations;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Name).ValueGeneratedNever();
        
        builder.Property(l => l.Name).IsRequired();
        builder.Property(l=>l.Latitude).IsRequired();
        builder.Property(l=>l.Longitude).IsRequired();
        builder.Property(l=>l.Geohash)
            .IsRequired()
            .HasMaxLength(8);
        
        builder.HasIndex(l => new {l.Type, l.Geohash})
            .HasDatabaseName("IX_Location_Type_Geohash");
        
        builder.HasMany(l => l.Articles)
            .WithOne(a => a.Location)
            .HasForeignKey(a => a.LocationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}