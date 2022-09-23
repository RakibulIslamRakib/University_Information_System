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
    }
}
