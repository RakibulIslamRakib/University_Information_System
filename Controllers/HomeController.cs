using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using University_Information_System.Models;
using University_Information_System.Services.ServiceClasses;
using University_Information_System.Services.ServiceInterfaces;

namespace University_Information_System.Controllers
{
    public class HomeController : Controller
    {

        #region Fields

        private readonly INoticeService noticeService;
        private readonly IAccountService accountService;
        private readonly IUserService userService;

        #endregion Fields



        #region ctor
        public HomeController(INoticeService noticeService, IAccountService accountService, IUserService userService)
        {
            this.noticeService = noticeService;
            this.accountService = accountService;
            this.userService = userService;
        }

        #endregion ctor


        #region Index
        public async Task<IActionResult> Index(string currentFilter,
                    string searchString, string orderBy,string sort, int? pageNumber, int? itemsPerPage)
        {
            var notice= await noticeService.GetAllNotice();

            switch (sort)
            {
                case "asc":
                    if(orderBy == "CreatedDate")
                    {
                       notice = notice.OrderBy(n => n.CreatedDate).ToList();
                    }
                    else
                    {
                        notice = notice.OrderBy(not => not.NoticeTitle).ToList();

                    }
                    break;

                case "dec":
                    if (orderBy == "CreatedDate")
                    {
                        notice = notice.OrderByDescending(not => not.CreatedDate).ToList();
                    }
                    else
                    {
                        notice = notice.OrderByDescending(not => not.NoticeTitle).ToList();

                    }
                    break;

            }

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.isAdmin = false;

            if (userService.IsAuthenticated())
            {
                var userRoles = await accountService.GetRoleOfCurrentUser();
                ViewBag.isAdmin = userRoles.Contains("Admin");
            }
            int pageSize = itemsPerPage ?? 5;
            ViewData["ItemsPerPage"] = pageSize;
            ViewData["CurrentFilter"] = searchString;
            ViewData["orderBy"] = orderBy;
            ViewData["sort"] = sort;

            searchString = !String.IsNullOrEmpty(searchString) ? searchString.ToLower() : "";

            if (!String.IsNullOrEmpty(searchString))
            {
                notice= notice.Where(st => st.NoticeTitle.ToLower().Contains(searchString)
                || st.Descryption.ToLower().Contains(searchString)).ToList();
            }
 
            return View(PaginatedList<Notice>.Create(notice, pageNumber ?? 1, pageSize));
        }
        #endregion Index

        [Authorize(Roles = "Admin")]
        #region AddNotice
        public IActionResult AddNotice()
        {
            ViewData["createdBy"] = userService.GetUserId();
            return View();

        }


        [HttpPost]
        public async Task<IActionResult> AddNotice(Notice notice)
        {
            notice.CreatedDate = DateTime.Now;

            //var err = ModelState.Values.SelectMany(er => er.Errors);
            if (ModelState.IsValid)
            {
                await noticeService.AddNotice(notice);
                return  RedirectToAction(actionName: "Index", controllerName: "Home");
            }

            return View(notice);
           
        }
        #endregion AddNotice

        [Authorize(Roles = "Admin")]
        #region DeleteNotice
        public async Task<IActionResult> DeleteNotice(int id)
        {
            var notice = await noticeService.GetNoticeById(id);
            if(notice == null) return View("Error");
            return View(notice);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteNotice(Notice notice)
        {
          await noticeService.DeleteNotice(notice);

            return RedirectToAction(actionName: "Index", controllerName: "Home");
        }

        #endregion DeleteNotice

        [Authorize(Roles = "Admin")]
        #region UpdateNotice
        public async Task<IActionResult> UpdateNotice(int id)
        {
            var notice = await noticeService.GetNoticeById(id);
            if (notice == null) return View("Error");
            ViewData["createdBy"] = userService.GetUserId();
            ViewData["createdDate"] = notice.CreatedDate;

            return View(notice);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateNotice(Notice notice)
        {
            
            if (ModelState.IsValid)
            {
                var updatedNotice = await noticeService.GetNoticeById(notice.id);
                updatedNotice.NoticeTitle = notice.NoticeTitle;
                updatedNotice.Descryption = notice.Descryption;
                await noticeService.UpdateNotice(updatedNotice);
                return RedirectToAction(actionName: "Index", controllerName: "Home");
            }

            return View(notice);
         
        }
        #endregion UpdateNotice

        #region DetailsNotice
        public async Task<IActionResult> DetailsNotice(int id)
        {
            var notice = await noticeService.GetNoticeDetailsById(id);
            if (notice == null) return View("Error");

            return View(notice);
        }

        #endregion DetailsNotice


    }
}