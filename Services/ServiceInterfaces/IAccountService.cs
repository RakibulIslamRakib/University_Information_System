using Microsoft.AspNetCore.Identity;
using University_Information_System.Models;

namespace University_Information_System.Services.ServiceInterfaces
{
    public interface IAccountService
    {
        public Task<IdentityResult> CreateUserAsync(SignUpModel userModel);
    }
}
