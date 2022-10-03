using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using University_Information_System.Models;
using University_Information_System.Services.ServiceInterfaces;

namespace University_Information_System.Controllers
{
    public class TeacherController : Controller
    {


    #region Fields

        private readonly ITeacherService teacherService;
        private readonly ISubjectService subjectService;
        private readonly IAccountService accountService;

        #endregion Fields


        #region ctor
        public TeacherController(ITeacherService teacherService, 
            IAccountService accountService, ISubjectService subjectService)
        {
            this.teacherService = teacherService;
            this.subjectService = subjectService;
            this.accountService = accountService;
       
        }
        #endregion ctor




        #region Teachers


        public async Task<IActionResult> Teachers(string currentFilter,
                    string searchString, int? pageNumber, int? itemsPerPage)
        {
            var teachers = await accountService.GetUsersInRole("Teacher");

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
                teachers = teachers.Where(tr => tr.FirstName.ToLower().Contains(searchString)
                || tr.LastName.ToLower().Contains(searchString) || 
                tr.Email.ToLower().Contains(searchString)).ToList();
            }

 

            return View(PaginatedList<ApplicationUser>.Create(teachers, pageNumber ?? 1, pageSize));

        }
        #endregion Teachers


        [Authorize(Roles = "Admin")]
        #region AddTeacher
        public async Task<IActionResult> AddTeacher(string id)
        {
            var user = await accountService.GetUserById(id);
            await teacherService.AddTeacher(user);
            return RedirectToAction(actionName: "Teachers", controllerName: "Teacher");
        }
        #endregion AddTeacher


        [Authorize(Roles = "Admin")]
        #region DeleteTeacher

        public async Task<IActionResult> DeleteTeacher(ApplicationUser teacher)
        {
            await teacherService.DeleteTeacher(teacher);
            
            return RedirectToAction(actionName: "Teachers", controllerName: "Teacher");
        }
        #endregion DeleteTeacher


        [Authorize(Roles = "Admin,Teacher")]
        #region  UpdateTeacher
        public async Task<IActionResult> UpdateTeacher(string id)
        {

            var teacher = await teacherService.GetTeacherById(id);

            return View(teacher);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTeacher(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {

               await accountService.UpdateUser(user);

                return RedirectToAction(actionName: "Teachers", controllerName: "Teacher");
            }

            return View(user);
        }
        #endregion  UpdateTeacher


        [Authorize(Roles = "Admin,Teacher,Student")]
        #region  DetailsTeacher
        public async Task<IActionResult> DetailsTeacher(string id, string currentFilter,
                    string searchString, int? pageSize, int? pageIndex, string atributeType = "Subjects")

        {
            var teacher = await teacherService.GetTeacherDetailsById(id);

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var userRoles = await accountService.GetRoleOfCurrentUser();
            ViewBag.isTeacher = userRoles.Contains("Teacher");
            var currentUser = await accountService.GetCurrentUser();

            ViewBag.CurrentUserId = currentUser.Id;

            teacher.PageSize = pageSize ?? 5;
            teacher.SearchString = searchString;
            teacher.PageIndex = pageIndex ?? 1;
            ViewData["atributeType"] = atributeType ?? "Subjects";

            searchString = !String.IsNullOrEmpty(searchString) ? searchString.ToLower() : "";

            var students = teacher.Students;
            var subjects = teacher.Subjects;
            var departments = teacher.Depertments;

            if (atributeType == "Students")
            {

                if (!String.IsNullOrEmpty(searchString))
                {
                    students = students.Where(st => st.FirstName.ToLower().Contains(searchString)
                    || st.LastName.ToLower().Contains(searchString)).ToList();
                }

                teacher.TotalPages = (int)Math.Ceiling(students.Count / (double) teacher.PageSize);
                var items = students.Skip((teacher.PageIndex - 1) *
                    teacher.PageSize).Take(teacher.PageSize).ToList();
                teacher.Students = items;
                teacher.Pages = teacher.GetPages(students.Count, teacher.PageSize);
                teacher.Subjects = new List<Subject>();
                teacher.Depertments = new List<Depertment>();


            }


            else if (atributeType == "Subjects")
            {

                if (!String.IsNullOrEmpty(searchString))
                {
                    subjects = subjects.Where(sb => sb.SubjectName.ToLower().Contains(searchString)
                    || sb.Descryption.ToLower().Contains(searchString)).ToList();
                }

                teacher.TotalPages = (int)Math.Ceiling(subjects.Count / (double)teacher.PageSize);
                var items = subjects.Skip((teacher.PageIndex - 1) *
                    teacher.PageSize).Take(teacher.PageSize).ToList();
                teacher.Subjects = items;
                teacher.Pages = teacher.GetPages(subjects.Count, teacher.PageSize);
                teacher.Students = new List<ApplicationUser>();
                teacher.Depertments = new List<Depertment>();
            }


            else
            {

                if (!String.IsNullOrEmpty(searchString))
                {
                    departments = departments.Where(tr => tr.DeptName.ToLower().Contains(searchString)
                    || tr.Descryption.ToLower().Contains(searchString)).ToList();
                }
                teacher.TotalPages = (int)Math.Ceiling(departments.Count / (double)teacher.PageSize);
                var items = departments.Skip((teacher.PageIndex - 1) * 
                    teacher.PageSize).Take(teacher.PageSize).ToList();
                teacher.Pages = teacher.GetPages(departments.Count, teacher.PageSize);
                teacher.Depertments = items;
                teacher.Subjects = new List<Subject>();
                teacher.Students = new List<ApplicationUser>();
            }

            return View(teacher);
        }

        #endregion  DetailsTeacher


        [Authorize(Roles = "Admin,Teacher")]
        #region  AddSubjectToTheTeacher
        public async Task<IActionResult> AddSubjectToTheTeacher(string id)
        {

            TempData["teacherId"] = id;
            var subjects = await subjectService.getAllSubject();

            var subjectOftheTeacher = await teacherService.GetSubjectByTeacherId(id);

            var subjectOutOftheTeacher = subjects.Except(subjectOftheTeacher).ToList();


            return View(subjectOutOftheTeacher);
        }
       

        [HttpPost]
        public async Task<IActionResult> AddSubjectToTheTeacher(string id, int subjectId)
        {
            var subTeacher = new SubjectTeacherMapped
            {
                TeacherId = id,
                SubjectId = subjectId
            };
            await teacherService.AddSubjectTeacherMapped(subTeacher);

            return RedirectToAction(actionName: "DetailsTeacher", controllerName: "Teacher", new { @id = id });

        }
        #endregion  AddSubjectToTheTeacher

        [Authorize(Roles = "Admin,Teacher")]
        #region  DeleteSubjectFromTeacher
        public async Task<IActionResult> DeleteSubjectFromTeacher(int subjectId, string TeacherId)
        {
            await teacherService.DeleteSubTeacherMapped(subjectId, TeacherId);
            return RedirectToAction(actionName: "DetailsTeacher", controllerName:
                "Teacher", new {@id = TeacherId});
        }

        #endregion DeleteSubjectFromTeacher
    }
}
