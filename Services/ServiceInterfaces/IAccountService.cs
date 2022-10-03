using Microsoft.AspNetCore.Identity;
using University_Information_System.Models;

namespace University_Information_System.Services.ServiceInterfaces
{
    public interface IAccountService
    {
        Task<IdentityResult> CreateUserAsync(SignUpModel userModel);
        Task<SignInResult> PasswordSignInAsync(SignInModel signInUser);
        Task<IdentityResult> ChangePasswordAsync(ChangePasswordModel model);
        Task SignOutAsync();
        Task<IdentityResult> AddRole(ApplicationUser user, string role);
        Task<List<ApplicationUser>> GetAllUsers();
        Task<ApplicationUser> GetUserById(string userId);
        Task<List<ApplicationUser>> GetUsersInRole(string roleName);
        Task<IdentityResult> UpdateUser(ApplicationUser user);
        Task<List<string>> GetRoleOfCurrentUser();
        Task<ApplicationUser> GetCurrentUser();
        Task<List<ApplicationUser>> GetAllDefaultUsers();
        Task<IdentityResult> RemoveRole(ApplicationUser user, string roleName);
    }
}
