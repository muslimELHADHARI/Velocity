using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Velocity.Models;
using Velocity.Models.Enums;
using Velocity.Repositories;
using Velocity.ViewModels;

namespace Velocity.Services;

public class AdminService : IAdminService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IReservationRepository _reservationRepository;

    public AdminService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IVehicleRepository vehicleRepository,
        IReservationRepository reservationRepository)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _vehicleRepository = vehicleRepository;
        _reservationRepository = reservationRepository;
    }

    public async Task<AdminDashboardViewModel> GetDashboardAsync()
    {
        var users = await _userManager.Users.CountAsync();
        var vehiclesList = await _vehicleRepository.GetAllAsync(null, null, null);
        var vehicles = vehiclesList.Count();
        var reservations = await _reservationRepository.GetAllAsync();

        var bicycleReservations = reservations.Count(r => r.Vehicle?.Type == VehicleType.Bicycle);
        var motorcycleReservations = reservations.Count(r => r.Vehicle?.Type == VehicleType.Motorcycle);
        var onlineRevenue = reservations
            .Where(r => r.PaymentMethod == PaymentMethod.Online && r.PaymentStatus == PaymentStatus.Paid)
            .Sum(r => r.TotalAmount);

        return new AdminDashboardViewModel
        {
            TotalUsers = users,
            TotalVehicles = vehicles,
            BicycleReservations = bicycleReservations,
            MotorcycleReservations = motorcycleReservations,
            OnlineRevenue = onlineRevenue,
            RecentReservations = reservations.Take(10),
            RecentVehicles = vehiclesList.Take(5)
        };
    }

    public async Task<IEnumerable<ManageUserViewModel>> GetUsersAsync()
    {
        var list = new List<ManageUserViewModel>();
        var users = await _userManager.Users.ToListAsync();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            list.Add(new ManageUserViewModel
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                PhoneNumber = user.PhoneNumber,
                Role = roles.FirstOrDefault() ?? string.Empty,
                IsActive = user.IsActive
            });
        }

        return list;
    }

    public async Task<bool> UpdateUserRoleAsync(string userId, string role, bool isActive)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) return false;

        if (!await _roleManager.RoleExistsAsync(role))
        {
            await _roleManager.CreateAsync(new IdentityRole(role));
        }

        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        await _userManager.AddToRoleAsync(user, role);

        user.IsActive = isActive;
        await _userManager.UpdateAsync(user);

        return true;
    }
}

