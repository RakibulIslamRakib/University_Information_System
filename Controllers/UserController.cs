using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Drawing;
using University_Information_System.Models;
using University_Information_System.Services.ServiceClasses;
using University_Information_System.Services.ServiceInterfaces;

namespace University_Information_System.Controllers
{
    
    public class UserController : Controller
    {


        #region Fields

        private readonly IAccountService accountService;
        private readonly IWebHostEnvironment _iweb;

        #endregion Fields


        #region ctor
        public UserController(IAccountService accountService, 
            IWebHostEnvironment iweb)
        {
            this.accountService = accountService;
            _iweb = iweb;
        }
        #endregion ctor




        #region Users

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Users(string currentFilter,
                    string searchString, int? pageNumber, int? itemsPerPage, int? deptId, bool? fromTeacher)
        {
            var users = await accountService.GetAllDefaultUsers();

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            int pageSize = itemsPerPage ?? 5;
            ViewData["ItemsPerPage"] = pageSize;
            ViewData["CurrentFilter"] = searchString;
            ViewBag.deptId = deptId ?? 0;
            ViewBag.fromTeacher = fromTeacher ?? false;

            searchString = !String.IsNullOrEmpty(searchString) ? searchString.ToLower() : "";
            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(tr => tr.FirstName.ToLower().Contains(searchString)
                || tr.LastName.ToLower().Contains(searchString) ).ToList();
            }

            return View(PaginatedList<ApplicationUser>.Create(users, pageNumber ?? 1, pageSize));

        }
        #endregion Teachers


        #region Profile
        public async Task<IActionResult> Profile(string userId)
        {
            var user = await accountService.GetUserById(userId);           
            return View(user);
        }
        #endregion Profile

        #region UpdateProfile
        public async Task<IActionResult> UpdateProfile(string userId)
        {
            var user = await accountService.GetUserById(userId);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(ApplicationUser userModel)
        {         
            var user = await accountService.GetCurrentUser();

            if (userModel.Picture != null)
            {
                string ext = Path.GetExtension(userModel.Picture.FileName);
                if (ext == ".jpg" || ext == ".gif")
                {

                    string folder = "images/user/";
                    var picturePath = Guid.NewGuid().ToString() + "_" + userModel.Picture.FileName;

                    //delete previous picture from folder
                    string path = Path.Combine(_iweb.WebRootPath, folder, user.PicturePath);
                    var file = new FileInfo(path);
                    if (file.Exists)
                    {
                        file.Delete();
                    }


                    //copy new file into folder

                    folder += picturePath;
                    var savePath = Path.Combine(_iweb.WebRootPath, folder);

                    var stream = new FileStream(savePath, FileMode.Create);
                    await userModel.Picture.CopyToAsync(stream);
                    stream.Close();

                    userModel.PicturePath = picturePath;
                }
            }
            else
            {
                userModel.PicturePath = user.PicturePath;
            }


            userModel.Id = user.Id;

            var result = await accountService.UpdateUser(userModel);

            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }

                return View(userModel);
            }

            if (result.Succeeded)
            {

                return RedirectToAction("Profile", "User",new {@userId = user.Id });

            }
          
            return View(userModel);
        }
        #endregion UpdateProfile


        #region DeleteUser
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await accountService.GetUserById(id);
            var result = await accountService.DeleteUser(user);
            if (result.Succeeded)
            {
                return RedirectToAction(actionName: "Users", controllerName: "User");
            }
            return View();
        }
        #endregion DeleteTeacher

    }


}
