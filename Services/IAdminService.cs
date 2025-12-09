using Velocity.ViewModels;

namespace Velocity.Services;

public interface IAdminService
{
    Task<AdminDashboardViewModel> GetDashboardAsync();
    Task<IEnumerable<ManageUserViewModel>> GetUsersAsync();
    Task<bool> UpdateUserRoleAsync(string userId, string role, bool isActive);
}

