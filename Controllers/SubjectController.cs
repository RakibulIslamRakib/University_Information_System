using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using University_Information_System.Models;
using University_Information_System.Services.ServiceClasses;
using University_Information_System.Services.ServiceInterfaces;

namespace University_Information_System.Controllers
{
    public class SubjectController : Controller

    {
        #region Fields

        private readonly ISubjectService subjectService;
        private readonly IAccountService accountService;
        private readonly IUserService _userService;
        #endregion Fields



        #region ctor

        public SubjectController(ISubjectService subjectService, IUserService userService
            , IAccountService accountService)
        {
            this.subjectService = subjectService;
            this.accountService = accountService;
            this._userService = userService;
        }

        #endregion ctor



        #region Subjects


        public async Task<IActionResult> Subjects(string currentFilter,
                    string searchString, int? pageNumber, int? itemsPerPage)
        {
            var subjects = await subjectService.getAllSubject();

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }


            var userRoles = await accountService.GetRoleOfCurrentUser();
            ViewBag.isAdmin = userRoles.Contains("Admin");
            ViewBag.hasAuth = userRoles.Contains("Admin") || userRoles.Contains("Student")
                || userRoles.Contains("Teacher");
            int pageSize = itemsPerPage ?? 5;
            ViewData["ItemsPerPage"] = pageSize;
            ViewData["CurrentFilter"] = searchString;

            searchString = !String.IsNullOrEmpty(searchString) ? searchString.ToLower() : "";
            if (!String.IsNullOrEmpty(searchString))
            {
                subjects = subjects.Where(sub => sub.SubjectName.ToLower().Contains(searchString)
                || sub.Descryption.ToLower().Contains(searchString)).ToList();
            }
 
            return View( PaginatedList<Subject>.Create(subjects, pageNumber ?? 1, pageSize));
        }

        #endregion Subjects

        #region AddSubject

        [Authorize(Roles = "Admin")]
        public IActionResult AddSubject()
        {
            ViewData["createdBy"] = _userService.GetUserId();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddSubject(Subject subject)
        {
            subject.CreatedDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                await subjectService.AddSubject(subject);
                return RedirectToAction(actionName: "Subjects", controllerName: "Subject");
            }
            return View(subject);

        }
        #endregion AddSubject


        #region DeleteSubject
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            var subject = await subjectService.GetSubjectById(id);
            if (subject == null) return View("Error");  

            return View(subject);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSubject(Subject subject)
        {
            await subjectService.DeleteSubject(subject);
            
            return RedirectToAction(actionName: "Subjects", controllerName: "Subject");
        }
        #endregion DeleteSubject



        #region UpdateSubject
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSubject(int id)
        {
            var subject = await subjectService.GetSubjectById(id);
            ViewData["createdBy"] = _userService.GetUserId();
            ViewData["createdDate"] = subject.CreatedDate;
            return View(subject);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSubject(Subject subject)
        {
            subject.UpdatedDate=DateTime.Now;
            subject.UpdatedBy= _userService.GetUserId();
            if (ModelState.IsValid)
            {
                await subjectService.UpdateSubject(subject);

                return RedirectToAction(actionName: "Subjects", controllerName: "Subject");
            }
            return View(subject);
        }
        #endregion UpdateSubject



        #region DetailsSubject
        [Authorize(Roles = "Admin,Teacher,Student")]
        public async Task<IActionResult> DetailsSubject(int id, string currentFilter,
                    string searchString, int? pageSize, int? pageIndex, string atributeType = "Depertments")
        {
            var subject = await subjectService.GetSubjectDetailsById(id);  


            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            subject.PageSize = pageSize ?? 5;
            subject.SearchString = searchString;
            subject.PageIndex = pageIndex ?? 1;
            ViewData["atributeType"] = atributeType ?? "Depertments";

            searchString = !String.IsNullOrEmpty(searchString) ? searchString.ToLower() : "";

            var students = subject.Students;
            var depertments = subject.Depertments;
            var teachers = subject.Teachers;

            if (atributeType == "Students")
            {

                if (!String.IsNullOrEmpty(searchString))
                {
                    students = students.Where(st => st.FirstName.ToLower().Contains(searchString)
                    || st.LastName.ToLower().Contains(searchString)).ToList();
                }

                subject.TotalPages = (int)Math.Ceiling(students.Count / (double)subject.PageSize);
                var items = students.Skip((subject.PageIndex - 1) * subject.PageSize).Take(subject.PageSize).ToList();
                subject.Students = items;
                subject.Pages = subject.GetPages(students.Count, subject.PageSize);
                subject.Depertments = new List<Depertment>();
                subject.Teachers = new List<ApplicationUser>();


            }


            else if (atributeType == "Depertments")
            {

                if (!String.IsNullOrEmpty(searchString))
                {
                    depertments = depertments.Where(dp => dp.DeptName.ToLower().Contains(searchString)
                    || dp.Descryption.ToLower().Contains(searchString)).ToList();
                }

                subject.TotalPages = (int)Math.Ceiling(depertments.Count / (double)subject.PageSize);
                var items = depertments.Skip((subject.PageIndex - 1) *subject.PageSize).Take(subject.PageSize).ToList();
                subject.Depertments = items;
                subject.Pages = subject.GetPages(depertments.Count, subject.PageSize);
                subject.Students = new List<ApplicationUser>();
                subject.Teachers = new List<ApplicationUser>();
            }


            else
            {

                if (!String.IsNullOrEmpty(searchString))
                {
                    teachers = teachers.Where(tr => tr.FirstName.ToLower().Contains(searchString)
                    || tr.LastName.ToLower().Contains(searchString)).ToList();
                }
                subject.TotalPages = (int)Math.Ceiling(teachers.Count / (double)subject.PageSize);
                var items = teachers.Skip((subject.PageIndex - 1) * subject.PageSize).Take(subject.PageSize).ToList();
                subject.Pages = subject.GetPages(teachers.Count, subject.PageSize);
                subject.Teachers = items;
                subject.Depertments = new List<Depertment>();
                subject.Students = new List<ApplicationUser>();
            }

            return View(subject);
        }

        #endregion DetailsSubject

    }
}
