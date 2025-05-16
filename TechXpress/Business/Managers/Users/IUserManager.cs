using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;

namespace Business.Managers.Users;

public interface IUserManager
{
    public Task<IdentityResult> CreateUserAsync(User user, string password);
    public Task<User?> FindByEmailAsync(string email);
    public Task<bool> CheckPasswordAsync(User user, string password);
    public Task SignInAsync(User user, bool rememberMe);
    public Task SignOutAsync();
    public List<User> GetUsers();
    public Task<List<User>> GetUsersAsync();
    public Task<User?> FindByIdAsync(string userId);
    public Task<IList<string>> GetRolesAsync(User user);
    public Task<IdentityResult> RemoveFromRolesAsync(User user, List<string> roles);
    public Task<IdentityResult> UpdateAsync(User user);
    public Task<IdentityResult> AddToRoleAsync(User user, string role);
    public Task<IdentityResult> AddToRolesAsync(User user, List<string> roles);
    public Task<User?> FindByNameAsync(string name);


}