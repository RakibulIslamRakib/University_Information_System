using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using University_Information_System.Models;
using University_Information_System.Services.ServiceInterfaces;

namespace University_Information_System.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminsController : Controller
    {


    #region Fields

        private readonly IAccountService accountService;
      

        #endregion Fields


        #region ctor
        public AdminsController(ITeacherService teacherService, 
            IAccountService accountService, ISubjectService subjectService)
        {
            this.accountService = accountService;
       
        }
        #endregion ctor




        #region admins

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(string currentFilter,
                    string searchString, int? pageNumber, int? itemsPerPage)
        {
            var admins = await accountService.GetUsersInRole("Admin");

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
            var currentUser = await accountService.GetCurrentUser();
            ViewData["currentUserEmail"] = currentUser.Email;
            searchString = !String.IsNullOrEmpty(searchString) ? searchString.ToLower() : "";
            if (!String.IsNullOrEmpty(searchString))
            {
                admins = admins.Where(tr => tr.FirstName.ToLower().Contains(searchString)
                || tr.LastName.ToLower().Contains(searchString)).ToList();
            }

            return View(PaginatedList<ApplicationUser>.Create(admins, pageNumber ?? 1, pageSize));

        }
        #endregion admins



        #region AddAdmin
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddAdmin(string id)
        {
            var user = await accountService.GetUserById(id);
            await accountService.AddRole(user, "Admin");
             
            return RedirectToAction(actionName: "Index", controllerName: "Admins");
             
        }
        #endregion AddAdmin



        #region DeleteAdmin
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAdmin(ApplicationUser adminUser)
        {
            await accountService.RemoveRole(adminUser, "Admin");
            
            return RedirectToAction(actionName: "Index", controllerName: "Admins");
        }
        #endregion DeleteAdmin


    }
}
