 using Microsoft.AspNetCore.Mvc;
using University_Information_System.Models;
using University_Information_System.Services.ServiceInterfaces;

namespace University_Information_System.Controllers
{
    public class StudentController : Controller
    {
        #region Fields
        private readonly IStudentService studentService;
        private readonly IDepartmentService departmentService;
        #endregion Fields


        #region ctor

        public StudentController(IStudentService studentService,
            IDepartmentService departmentService)
        {
            this.studentService = studentService;
            this.departmentService = departmentService; 
        }
        #endregion ctor


        #region Students

        public async Task<IActionResult> Students(string currentFilter,
                    string searchString, int? pageNumber, int? itemsPerPage)
        {
            var students = studentService.getAllStudent();

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

            searchString = !String.IsNullOrEmpty(searchString) ? searchString.ToLower() : "";
           
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(st => st.FirstName.ToLower().Contains(searchString)
                || st.LastName.ToLower().Contains(searchString));
            }

           
            return View(await PaginatedList<Student>.CreateAsync(students, pageNumber ?? 1, pageSize));
        }

        #endregion Students

        #region AddStudent
        public IActionResult AddStudent()
        {
            ViewBag.depertments = departmentService.getAllDepertment();
            return View();
        }


        [HttpPost]
        public IActionResult AddStudent(Student student)
        {
            if (ModelState.IsValid)
            {
                studentService.AddStudent(student);

                return RedirectToAction(actionName: "Students", controllerName: "Student");

            }

            ViewBag.depertments = departmentService.getAllDepertment();
            return View(student);
        }
        #endregion AddStudent

        #region DeleteStudent
        public IActionResult DeleteStudent(int id)
        {
            var student = studentService.GetStudentById(id);
            student.DeptName = departmentService.GetDepertmentById(student.DepertmentId).DeptName;

            return View(student);
        }


        [HttpPost]
        public IActionResult DeleteStudent(Student student)
        {
            studentService.DeleteStudent(student);

            return RedirectToAction(actionName: "Students", controllerName: "Student");
        }

        #endregion DeleteStudent

        #region UpdateStudent
        public IActionResult UpdateStudent(int id)
        {
            var student = studentService.GetStudentById(id);

            return View(student);
        }

        [HttpPost]
        public IActionResult UpdateStudent(Student student)
        {
            if (ModelState.IsValid)
            {
                studentService.UpdateStudent(student);

                return RedirectToAction(actionName: "Students", controllerName: "Student");

            }
            return View(student);
        }
        #endregion UpdateStudent


        #region DetailsStudent
        public IActionResult DetailsStudent(int id)
        {
            var student = studentService.GetStudentDetailsById(id);

            return View(student);
        }

        #endregion DetailsStudent

        #region EnroleSubject
        public IActionResult EnroleSubject(int id)
        {
            TempData["StudentId"] = id;
            var student = studentService.GetStudentById(id);
            var subjectOftheDept = new List<Subject>(departmentService.GetSubjectByDepertmentId(student.DepertmentId));
            var subjectOfTheStudent = studentService.GetSubjectByStudentId(id);
            var subjectOutOfTheStudent = subjectOftheDept.Except(subjectOfTheStudent).ToList();

            return View(subjectOutOfTheStudent);

        }

        [HttpPost]
        public IActionResult EnroleSubject(int id, int subjectId)
        {
            var enrollment = new SubjectStudentMapped();
            enrollment.subjectId = subjectId;
            enrollment.studentId= id;
            studentService.AddSubjectStudentMapped(enrollment);
            
            return RedirectToAction(actionName: "DetailsStudent", controllerName: "Student", new { @id = id });

        }
        #endregion EnroleSubject


        #region DeleteEnrolment
        public IActionResult DeleteEnrolment(int subjectId, int studentId)
        {
            studentService.DeleteEnrolmentFromSubjectStudentMapped(subjectId, studentId);
            
            return RedirectToAction(actionName: "DetailsStudent", controllerName: "Student", new { @id = studentId });
        }
        #endregion DeleteEnrolment
    }
}
