using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Velocity.Models;
using Velocity.Models.Enums;

namespace Velocity.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<Reservation> Reservations => Set<Reservation>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Vehicle>(entity =>
        {
            entity.Property(v => v.Name).IsRequired().HasMaxLength(150);
            entity.Property(v => v.Brand).IsRequired().HasMaxLength(120);
            entity.Property(v => v.Description).HasMaxLength(1024);
            entity.Property(v => v.ImageUrl).HasMaxLength(500);
            entity.Property(v => v.PricePerHour).HasPrecision(10, 2);
            entity.Property(v => v.EngineSize).HasPrecision(10, 2);
        });

        builder.Entity<Reservation>(entity =>
        {
            entity.Property(r => r.TotalAmount).HasPrecision(10, 2);
            entity.Property(r => r.PaymentReference).HasMaxLength(150);
            entity.HasOne(r => r.Vehicle)
                .WithMany(v => v.Reservations)
                .HasForeignKey(r => r.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
