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
            var students = await studentService.getAllStudent();

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
                students = (List<Student>)students.Where(st => st.FirstName.ToLower().Contains(searchString)
                || st.LastName.ToLower().Contains(searchString)).ToList();
            }

           
            return View( PaginatedList<Student>.Create(students, pageNumber ?? 1, pageSize));
        }

        #endregion Students


        #region AddStudent
        public async Task<IActionResult> AddStudent()
        {
            ViewBag.depertments = await departmentService.GetAllDepertment();
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AddStudent(Student student)
        {
            if (ModelState.IsValid)
            {
               await studentService.AddStudent(student);

               return RedirectToAction(actionName: "Students", controllerName: "Student");

            }

            ViewBag.depertments = await departmentService.GetAllDepertment();
            return View(student);
        }
        #endregion AddStudent

        #region DeleteStudent
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await studentService.GetStudentById(id);
            var dept = await departmentService.GetDepertmentById(student.DepertmentId);
            if(dept == null) return View("Error");
            student.DeptName = dept.DeptName;
            return View(student);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteStudent(Student student)
        {
            await studentService.DeleteStudent(student);

            return RedirectToAction(actionName: "Students", controllerName: "Student");
        }

        #endregion DeleteStudent

        #region UpdateStudent
        public async  Task<IActionResult> UpdateStudent(int id)
        {
            var student = await studentService.GetStudentById(id);

            return View(student);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStudent(Student student)
        {
            if (ModelState.IsValid)
            {
               await studentService.UpdateStudent(student);

                return RedirectToAction(actionName: "Students", controllerName: "Student");

            }
            return View(student);
        }

        #endregion UpdateStudent


        #region DetailsStudent
        public async Task<IActionResult> DetailsStudent(int id)
        {
            var student = await studentService.GetStudentDetailsById(id);

            return View(student);
        }

        #endregion DetailsStudent

        #region EnroleSubject
        public async Task<IActionResult> EnroleSubject(int id)
        {
            TempData["StudentId"] = id;
            var student = await studentService.GetStudentById(id);
            var subjectOftheDept = await departmentService.GetSubjectByDepertmentId(student.DepertmentId);
            var subjectOfTheStudent = await studentService.GetSubjectByStudentId(id);
            var subjectOutOfTheStudent =  subjectOftheDept.Except(subjectOfTheStudent);

            return View(subjectOutOfTheStudent);

        }


        [HttpPost]
        public async Task<IActionResult> EnroleSubject(int id, int subjectId)
        {
            var enrollment = new SubjectStudentMapped();
            enrollment.subjectId = subjectId;
            enrollment.studentId= id;
            await studentService.AddSubjectStudentMapped(enrollment);
            
            return RedirectToAction(actionName: "DetailsStudent", controllerName: "Student", new { id });
        }
        #endregion EnroleSubject


        #region DeleteEnrolment
        public async Task<IActionResult> DeleteEnrolment(int subjectId, int studentId)
        {
            await studentService.DeleteEnrolmentFromSubjectStudentMapped(subjectId, studentId);
            
            return RedirectToAction(actionName: "DetailsStudent", controllerName: "Student", new { @id = studentId });
        }
        #endregion DeleteEnrolment
    }
}
