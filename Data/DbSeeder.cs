using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Velocity.Models;
using Velocity.Models.Enums;

namespace Velocity.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var scoped = scope.ServiceProvider;

        var context = scoped.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();

        var roleManager = scoped.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scoped.GetRequiredService<UserManager<ApplicationUser>>();

        var roles = new[] { "Admin", "Customer" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Seed admin user
        var adminEmail = "admin@velocity.tn";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser is null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                IsActive = true
            };
            await userManager.CreateAsync(adminUser, "Admin#12345");
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }

        // Seed sample vehicles with Tunisian culture
        if (!context.Vehicles.Any())
        {
            var vehicles = new List<Vehicle>
            {
                // Bicycles
                new() { 
                    Name = "Vélo de Ville Tunis", 
                    Type = VehicleType.Bicycle, 
                    Brand = "Tunisia Bike", 
                    Description = "Vélo urbain léger parfait pour les rues de Tunis. Idéal pour se déplacer dans la médina et les quartiers modernes.", 
                    PricePerHour = 8, 
                    ImageUrl = "/images/bike1.jpg", 
                    AvailabilityStatus = AvailabilityStatus.Available 
                },
                new() { 
                    Name = "Vélo Tout Terrain Sidi Bou", 
                    Type = VehicleType.Bicycle, 
                    Brand = "Atlas Cycles", 
                    Description = "Vélo de montagne robuste avec suspension, parfait pour explorer les collines de Sidi Bou Saïd et les sentiers côtiers.", 
                    PricePerHour = 12, 
                    ImageUrl = "/images/bike2.jpg", 
                    AvailabilityStatus = AvailabilityStatus.Available 
                },
                new() { 
                    Name = "Vélo Électrique Carthage", 
                    Type = VehicleType.Bicycle, 
                    Brand = "Medina Cycles", 
                    Description = "Vélo électrique moderne pour une balade confortable le long de la corniche de Carthage et de La Marsa.", 
                    PricePerHour = 15, 
                    ImageUrl = "/images/bike3.jpg", 
                    AvailabilityStatus = AvailabilityStatus.Available 
                },
                // Motorcycles
                new() { 
                    Name = "Scooter 125cc Tunis", 
                    Type = VehicleType.Motorcycle, 
                    Brand = "Piaggio", 
                    Description = "Scooter agile et économique, parfait pour naviguer dans le trafic tunisien. Idéal pour les déplacements urbains.", 
                    EngineSize = 125, 
                    PricePerHour = 25, 
                    ImageUrl = "/images/motor1.jpg", 
                    AvailabilityStatus = AvailabilityStatus.Available 
                },
                new() { 
                    Name = "Moto 250cc Hammamet", 
                    Type = VehicleType.Motorcycle, 
                    Brand = "Yamaha", 
                    Description = "Moto sportive pour explorer la côte tunisienne. Parfaite pour les routes entre Tunis et Hammamet.", 
                    EngineSize = 250, 
                    PricePerHour = 35, 
                    ImageUrl = "/images/motor2.jpg", 
                    AvailabilityStatus = AvailabilityStatus.Available 
                },
                new() { 
                    Name = "Moto 500cc Djerba", 
                    Type = VehicleType.Motorcycle, 
                    Brand = "Honda", 
                    Description = "Moto de tourisme confortable pour les longues distances. Idéale pour découvrir l'île de Djerba et le sud tunisien.", 
                    EngineSize = 500, 
                    PricePerHour = 50, 
                    ImageUrl = "/images/motor3.jpg", 
                    AvailabilityStatus = AvailabilityStatus.Available 
                },
                new() { 
                    Name = "Scooter 150cc Sousse", 
                    Type = VehicleType.Motorcycle, 
                    Brand = "Vespa", 
                    Description = "Scooter élégant et pratique pour les promenades dans la médina de Sousse et le long de la plage.", 
                    EngineSize = 150, 
                    PricePerHour = 28, 
                    ImageUrl = "/images/motor4.jpg", 
                    AvailabilityStatus = AvailabilityStatus.Available 
                }
            };

            context.Vehicles.AddRange(vehicles);
            await context.SaveChangesAsync();
        }
    }
}

