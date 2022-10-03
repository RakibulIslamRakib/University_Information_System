using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using University_Information_System.Models;
using University_Information_System.Services.ServiceInterfaces;

namespace University_Information_System.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        [Route("signup")]
        public IActionResult SignUp()
        {
            return View();
        }

        [Route("signup")]
        [HttpPost]
        public async Task<IActionResult> SignUpAsync(SignUpModel userModel)
        {
            if (ModelState.IsValid)
            {
               var resultSignUp = await _accountService.CreateUserAsync(userModel);
                if (!resultSignUp.Succeeded)
                {
                    foreach(var err in resultSignUp.Errors)
                    {
                        ModelState.AddModelError("", err.Description);
                    }

                    return View(userModel);
                }

                var signInModel = new SignInModel
                {
                    Email = userModel.Email,
                    password = userModel.Password,
                    RememberMe = false
                };


               
                ModelState.Clear();
                var result = await _accountService.PasswordSignInAsync(signInModel);

              

                if (result.Succeeded)
                {

                    return RedirectToAction("Index", "Home");

                }
                
                
            }

            return View(userModel);
        }


        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }


        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(SignInModel signINModel , string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.PasswordSignInAsync(signINModel);
                if (result.Succeeded)
                {
                    if(!string.IsNullOrEmpty(returnUrl))return LocalRedirect(returnUrl);
                    return RedirectToAction("Index", "Home");                   
                    
                }
                
                    ModelState.AddModelError("", "Invalid credentials");
               
            }
            return View(signINModel);
        }

        public async Task<IActionResult> Logout()
        {
          await _accountService.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Route("change-password")]
        public IActionResult  ChangePassword()
        {
            return View();
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.ChangePasswordAsync(model);

                if (result.Succeeded)
                {
                    ViewBag.success = true;
                    ModelState.Clear();
                    return View();
                }
            }
                return View(model);
        }
    }
}
