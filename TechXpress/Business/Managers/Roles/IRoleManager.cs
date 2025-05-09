using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;

namespace Business.Managers.Roles;

public interface IRoleManager
{
    List<Role> GetRoles();
    Task<Role?> FindByIdAsync(string id);
    Task<IdentityResult> CreateAsync(Role role);
}