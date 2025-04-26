using System.Data;

using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Aggregates.Identity.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HeadphoneStore.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(ApplicationDbContext context, RoleManager<AppRole> roleManager, ILogger<UserRepository> logger)
    {
        _context = context;
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task<bool> UpdateLastLogin(string email)
    {
        var query = await _context
            .AppUsers
            .Where(x => x.Email == email)
            .FirstOrDefaultAsync();

        query.LastLoginDate = DateTimeOffset.UtcNow;

        var rowAffected = await _context.SaveChangesAsync();

        return rowAffected > 0;
    }
}
