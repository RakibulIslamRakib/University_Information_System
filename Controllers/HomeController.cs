using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using University_Information_System.Models;
using University_Information_System.Services.ServiceClasses;
using University_Information_System.Services.ServiceInterfaces;

namespace University_Information_System.Controllers
{
    public class HomeController : Controller
    {

        #region Fields

        private readonly INoticeService noticeService;

        #endregion Fields



        #region ctor
        public HomeController(INoticeService noticeService)
        {
            this.noticeService = noticeService;
        }

        #endregion ctor


        #region Index
        public async Task<IActionResult> Index(string currentFilter,
                    string searchString, int? pageNumber)
        {
            var notice= noticeService.getAllNotice();

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;

            searchString = !String.IsNullOrEmpty(searchString) ? searchString.ToLower() : "";

            if (!String.IsNullOrEmpty(searchString))
            {
                notice= notice.Where(st => st.NoticeTitle.ToLower().Contains(searchString)
                || st.Descryption.ToLower().Contains(searchString));
            }

            int pageSize = 3;


            return View(await PaginatedList<Notice>.CreateAsync(notice, pageNumber ?? 1, pageSize));
        }
        #endregion Index


        #region AddNotice
        public IActionResult AddNotice()
        {
            return View();

        }


        [HttpPost]
        public async Task<IActionResult> AddNotice(Notice notice)
        {
            notice.CreatedDate = DateTime.Now;
            notice.CreatedBy= 1;

            var err = ModelState.Values.SelectMany(er => er.Errors);
            if (ModelState.IsValid)
            {
                noticeService.AddNotice(notice);
                return  RedirectToAction(actionName: "Index", controllerName: "Home");
            }

            return View(notice);
           
        }
        #endregion AddNotice

        #region DeleteNotice
        public IActionResult DeleteNotice(int id)
        {
            var notice = noticeService.GetNoticeById(id);

            return View(notice);
        }


        [HttpPost]
        public IActionResult DeleteNotice(Notice notice)
        {
            noticeService.DeleteNotice(notice);

            return RedirectToAction(actionName: "Index", controllerName: "Home");
        }

        #endregion DeleteNotice


        #region UpdateNotice
        public IActionResult UpdateNotice(int id)
        {
            var notice = noticeService.GetNoticeById(id);

            return View(notice);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateNotice(Notice notice)
        {
            

            if (ModelState.IsValid)
            {
                var updatedNotice = noticeService.GetNoticeById(notice.id);
                updatedNotice.NoticeTitle = notice.NoticeTitle;
                updatedNotice.Descryption = notice.Descryption;
                noticeService.UpdateNotice(updatedNotice);
                return RedirectToAction(actionName: "Index", controllerName: "Home");
            }

            return View(notice);
         
        }
        #endregion UpdateNotice

        #region DetailsNotice
        public IActionResult DetailsNotice(int id)
        {
            var notice = noticeService.GetNoticeDetailsById(id);

            return View(notice);
        }

        #endregion DetailsNotice


    }
}