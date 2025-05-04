using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;

namespace Business.Managers.Roles;

public class RoleManager : IRoleManager
{
    private readonly RoleManager<Role> _roleManager;

    public RoleManager(RoleManager<Role> roleManager)
    {
        _roleManager = roleManager;
    }
    public List<Role> GetRoles()
    {
        return _roleManager.Roles.ToList();
    }

    public async Task<Role?> FindByIdAsync(string id)
    {
        return await _roleManager.FindByIdAsync(id);
    }

    public async Task<IdentityResult> CreateAsync(Role role)
    {
        return await _roleManager.CreateAsync(role);
    }
}