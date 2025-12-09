using Velocity.Models;

namespace Velocity.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<ApplicationUser>> GetAllAsync();
    Task<ApplicationUser?> GetByIdAsync(string id);
    Task UpdateAsync(ApplicationUser user);
    Task SaveChangesAsync();
}

