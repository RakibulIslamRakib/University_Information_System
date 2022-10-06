using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using University_Information_System.Models;
using University_Information_System.Services.ServiceInterfaces;

namespace University_Information_System.Services.ServiceClasses
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserService _userService;

        public AccountService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUserService userService
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
        }



        public async Task<IdentityResult> CreateUserAsync(SignUpModel userModel)
        {
            var user = new ApplicationUser()
            {
                Email = userModel.Email,
                UserName = userModel.Email,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                PicturePath = userModel.PicturePath,
                //DateOfBirth = userModel.DateOfBirth,
                //About = userModel.About
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);


            return result;
        }

        public async Task<IdentityResult> RemoveRole(ApplicationUser user, string roleName)
        {
            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            return result;
        }

        public async Task<SignInResult> PasswordSignInAsync(SignInModel signInModel)
        {
            var result = await _signInManager.PasswordSignInAsync(signInModel.Email,
                signInModel.password, signInModel.RememberMe, false);
            return result;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }


        public async Task<IdentityResult> ChangePasswordAsync(ChangePasswordModel model)
        {
            var userId = _userService.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ChangePasswordAsync(user,
                model.CurrentPassword, model.NewPassword);
            return result;
        }



        public async Task<IdentityResult> UpdateUser(ApplicationUser user)
        {
           
            var userVar = await _userManager.FindByIdAsync(user.Id);
            userVar.FirstName = user.FirstName;
            userVar.LastName = user.LastName;
            userVar.PicturePath = user.PicturePath;
            var result = await _userManager.UpdateAsync(userVar);
            return result;
        }


        public async Task<ApplicationUser> GetUserById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user;
        }


        public async Task<List<ApplicationUser>> GetAllUsers()
        {
            var users =await _userManager.Users.ToListAsync();
            return users;
        }

        public async Task<List<ApplicationUser>> GetAllDefaultUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var teachers = await _userManager.GetUsersInRoleAsync("Teacher");
            var students = await _userManager.GetUsersInRoleAsync("Student");
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            users.RemoveAll(x => teachers.Contains(x));
            users.RemoveAll(x => students.Contains(x));
            users.RemoveAll(x => admins.Contains(x));
            return users;
        }

        public async Task<IdentityResult> AddRole(ApplicationUser user, string role)
        {
            var result = await _userManager.AddToRoleAsync(user, role);
            return result;
        }

        public async Task<List<ApplicationUser>> GetUsersInRole(string roleName)
        {
            var usersOfRole =await _userManager.GetUsersInRoleAsync(roleName);
            var result = usersOfRole.ToList();
            return result;
        }

        public async Task<List<string>> GetRoleOfCurrentUser()
        {
            var userId = _userService.GetUserId();
            if(userId == null)
            {
                return new List<string>();
            }
            var currentUser = await  _userManager.FindByIdAsync( userId);
            var userRoles = await _userManager.GetRolesAsync(currentUser);
            return (List<string>)userRoles;

        }

        public async Task<ApplicationUser> GetCurrentUser()
        {
            var userId = _userService.GetUserId();
            var currentUser = await _userManager.FindByIdAsync(userId);
            return currentUser;
        }


    }
}
