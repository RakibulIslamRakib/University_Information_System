using Microsoft.AspNetCore.Identity;
using University_Information_System.Models;
using University_Information_System.Services.ServiceInterfaces;

namespace University_Information_System.Services.ServiceClasses
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AccountService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

      

        public async Task<IdentityResult> CreateUserAsync(SignUpModel userModel)
        {
            var user = new ApplicationUser()
            {
                Email = userModel.Email,
                UserName = userModel.Email
            };

           var result = await _userManager.CreateAsync(user,userModel.Password);
            return result;
        }
    }
}
