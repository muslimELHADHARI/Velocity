using Microsoft.EntityFrameworkCore;
using Velocity.Data;
using Velocity.Models;

namespace Velocity.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
    {
        return await _context.Users.OrderBy(u => u.Email).ToListAsync();
    }

    public Task<ApplicationUser?> GetByIdAsync(string id)
    {
        return _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public Task UpdateAsync(ApplicationUser user)
    {
        _context.Users.Update(user);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync() => _context.SaveChangesAsync();
}

