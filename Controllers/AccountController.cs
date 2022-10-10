using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using University_Information_System.Models;
using University_Information_System.Services.ServiceInterfaces;

namespace University_Information_System.Controllers
{
    public class AccountController : Controller
    {
        #region Field
        private readonly IAccountService _accountService;
        private readonly IWebHostEnvironment _iweb;
        #endregion Field

        #region ctor
        public AccountController(IAccountService accountService,
            IWebHostEnvironment iweb)
        {
            _iweb = iweb;
            _accountService = accountService;
        }

        #endregion ctor

        #region signUp
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
                if (userModel.Picture != null)
                {
                    
                    string ext = Path.GetExtension(userModel.Picture.FileName);
                    if (ext == ".jpg" || ext == ".gif")
                    {
                        string folder = "images/user/";
                        var picturePath = Guid.NewGuid().ToString() + "_" + userModel.Picture.FileName;
                        folder += picturePath;
                        
                        var savePath = Path.Combine(_iweb.WebRootPath,folder);

                        var stream = new FileStream(savePath, FileMode.Create);
                        await userModel.Picture.CopyToAsync(stream);
                        stream.Close();

                        userModel.PicturePath = picturePath;


                        var resultSignUp = await _accountService.CreateUserAsync(userModel);

                        if (!resultSignUp.Succeeded)
                        {
                            foreach (var err in resultSignUp.Errors)
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
                }

                
                
            }

            return View(userModel);
        }
        #endregion signUp

        #region Login
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
                    if(!string.IsNullOrEmpty(returnUrl) && returnUrl!= "/signup") return LocalRedirect(returnUrl);
                    return RedirectToAction("Index", "Home");                   
                    
                }
                
                    ModelState.AddModelError("", "Invalid credentials");
               
            }
            return View(signINModel);
        }
        #endregion Login


        #region Logout
        public async Task<IActionResult> Logout()
        {
          await _accountService.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        #endregion Login


        #region ChangePassword
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
        #endregion ChangePassword
    }
}
