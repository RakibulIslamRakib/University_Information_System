using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using University_Information_System.Models;
using University_Information_System.Services.ServiceClasses;
using University_Information_System.Services.ServiceInterfaces;

namespace University_Information_System.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {


        #region Fields

        private readonly IAccountService accountService;

        #endregion Fields


        #region ctor
        public UserController(IAccountService accountService)
        {
            this.accountService = accountService;
        }
        #endregion ctor




        #region Users


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



        public async Task<IActionResult> AddRole(string userId,string roleName)
        {
            var user = await accountService.GetUserById(userId);
            
            var result = await accountService.AddRole(user, roleName);

            if (result.Succeeded)
            {
                return  RedirectToAction(actionName: "Teachers", controllerName: "Teacher");
            }

            return View(result.Errors);
        }


    }
}
