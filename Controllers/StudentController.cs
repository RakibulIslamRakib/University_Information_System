using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NuGet.DependencyResolver;
using System.Data;
using University_Information_System.Models;
using University_Information_System.Services.ServiceClasses;
using University_Information_System.Services.ServiceInterfaces;

namespace University_Information_System.Controllers
{
    public class StudentController : Controller
    {
        #region Fields
        private readonly IStudentService studentService;
        private readonly IDepartmentService departmentService;
        private readonly IAccountService accountService;
        #endregion Fields


        #region ctor

        public StudentController(IStudentService studentService,
            IDepartmentService departmentService,
                IAccountService accountService)
        {
            this.studentService = studentService;
            this.departmentService = departmentService; 
            this.accountService = accountService;
        }
        #endregion ctor


        #region Students

        public async Task<IActionResult> Students(string currentFilter,
                    string searchString, int? pageNumber, int? itemsPerPage)
        {
            var students = await studentService.GetAllStudent();

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
                students = students.Where(st => st.FirstName.ToLower().Contains(searchString)
                || st.LastName.ToLower().Contains(searchString)).ToList();
            }

           
            return View( PaginatedList<ApplicationUser>.Create(students, pageNumber ?? 1, pageSize));
        }

        #endregion Students



        #region AddStudent
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddStudent(string userId, int deptId)
        {
            await studentService.AddStudent(userId, deptId);
            return RedirectToAction(actionName: "DetailsDepertment", 
                controllerName: "Depertment" , new {id = deptId, atributeType = "Students" });
        }
        #endregion AddStudent



        #region DeleteStudent
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteStudent(ApplicationUser student)
        {
            await studentService.DeleteStudent(student);

            return RedirectToAction(actionName: "Students", controllerName: "Student");
        }

        #endregion DeleteStudent


        #region UpdateStudent
        [Authorize(Roles = "Admin")]
        public async  Task<IActionResult> UpdateStudent(string id)
        {
            var student = await accountService.GetUserById(id);

            return View(student);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStudent(ApplicationUser user)
            {
                if (ModelState.IsValid)
                {

                    await accountService.UpdateUser(user);

                    return RedirectToAction(actionName: "Students", controllerName: "Student");
                }

                return View(user);
            }

        #endregion UpdateStudent


        #region DetailsStudent
        [Authorize(Roles = "Admin,Teacher,Student")]
        public async Task<IActionResult> DetailsStudent(string id, string currentFilter,
                    string searchString, int? pageSize, int? pageIndex)
    {
            var student = await studentService.GetStudentDetailsById(id);

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }


            var userRoles = await accountService.GetRoleOfCurrentUser();
            var currentUser = await accountService.GetCurrentUser();
            ViewBag.isStudent = userRoles.Contains("Student");                     
            ViewBag.CurrentUserId = currentUser.Id;

            student.PageSize = pageSize ?? 5;
            student.SearchString = searchString;
            student.PageIndex = pageIndex ?? 1;
            searchString = !String.IsNullOrEmpty(searchString) ? searchString.ToLower() : "";
            var subjects = student.Subjects;

            if (!String.IsNullOrEmpty(searchString))
            {
                subjects = subjects.Where(sb => sb.SubjectName.ToLower().Contains(searchString)
                || sb.Descryption.ToLower().Contains(searchString)).ToList();
            }

            student.TotalPages = (int)Math.Ceiling(subjects.Count / (double)student.PageSize);
            var items = subjects.Skip((student.PageIndex - 1) *
                student.PageSize).Take(student.PageSize).ToList();
            student.Subjects = items;
            student.Pages = student.GetPages(subjects.Count, student.PageSize);
            student.Students = new List<ApplicationUser>();
            student.Depertments = new List<Depertment>();


            return View(student);
    }

        #endregion DetailsStudent



        #region EnroleSubject
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> EnroleSubject(string id)
        {
            TempData["StudentId"] = id;
            //var student = await studentService.GetStudentById(id);
            var dept = await studentService.GetDeptByStudentId(id);
            var subjectOftheDept = await departmentService.GetSubjectByDepertmentId(dept.id);
            var subjectOfTheStudent = await studentService.GetSubjectByStudentId(id);
            var subjectOutOfTheStudent = subjectOftheDept.Except(subjectOfTheStudent).ToList();

            return View(subjectOutOfTheStudent);

        }


        [HttpPost]
        public async Task<IActionResult> EnroleSubject(string id, int subjectId)
        {
            var enrollment = new SubjectStudentMapped
            {
                subjectId = subjectId,
                studentId = id
            };
            await studentService.AddSubjectStudentMapped(enrollment);
            
            return RedirectToAction(actionName: "DetailsStudent", controllerName: "Student", new { id });
        }
        #endregion EnroleSubject


        #region DeleteEnrolment
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> DeleteEnrolment(int subjectId, string studentId)
        {
            await studentService.DeleteEnrolmentFromSubjectStudentMapped(subjectId, studentId);
            
            return RedirectToAction(actionName: "DetailsStudent", controllerName: "Student", new { @id = studentId });
        }
        #endregion DeleteEnrolment
    }
}
