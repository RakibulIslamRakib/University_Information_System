using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
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


        [Authorize(Roles = "Admin")]
        #region AddStudent
        public async Task<IActionResult> AddStudent(string userId, int deptId)
        {
            await studentService.AddStudent(userId, deptId);
            return RedirectToAction(actionName: "DetailsDepertment", 
                controllerName: "Depertment" , new {id = deptId, atributeType = "Students" });
        }
        #endregion AddStudent


        [Authorize(Roles = "Admin")]
        #region DeleteStudent

        public async Task<IActionResult> DeleteStudent(ApplicationUser student)
        {
            await studentService.DeleteStudent(student);

            return RedirectToAction(actionName: "Students", controllerName: "Student");
        }

        #endregion DeleteStudent

        [Authorize(Roles = "Admin")]
        #region UpdateStudent
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

        [Authorize(Roles = "Admin,Teacher,Student")]
        #region DetailsStudent
         public async Task<IActionResult> DetailsStudent(string id)
    {
            var student = await studentService.GetStudentDetailsById(id);

            var userRoles = await accountService.GetRoleOfCurrentUser();
            var currentUser = await accountService.GetCurrentUser();
            ViewBag.isStudent = userRoles.Contains("Student");                     
            ViewBag.CurrentUserId = currentUser.Id;

            return View(student);
    }

        #endregion DetailsStudent


        [Authorize(Roles = "Student")]
        #region EnroleSubject
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

        [Authorize(Roles = "Student")]
        #region DeleteEnrolment
        public async Task<IActionResult> DeleteEnrolment(int subjectId, string studentId)
        {
            await studentService.DeleteEnrolmentFromSubjectStudentMapped(subjectId, studentId);
            
            return RedirectToAction(actionName: "DetailsStudent", controllerName: "Student", new { @id = studentId });
        }
        #endregion DeleteEnrolment
    }
}
