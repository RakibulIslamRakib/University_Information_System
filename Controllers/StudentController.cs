 using Microsoft.AspNetCore.Mvc;
using University_Information_System.Models;
using University_Information_System.Services.ServiceInterfaces;

namespace University_Information_System.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService studentService;
        private readonly IDepartmentService departmentService;

        public StudentController(IStudentService studentService,
            IDepartmentService departmentService)
        {
            this.studentService = studentService;
            this.departmentService = departmentService; 
        }


        public async Task<IActionResult> Students(string currentFilter,
                    string searchString, int? pageNumber)
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
            ViewData["CurrentFilter"] = searchString;

            searchString = !String.IsNullOrEmpty(searchString) ? searchString.ToLower() : "";
           
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(st => st.FirstName.ToLower().Contains(searchString)
                || st.LastName.ToLower().Contains(searchString));
            }

            int pageSize = 3;


            return View(await PaginatedList<Student>.CreateAsync(students, pageNumber ?? 1, pageSize));
        }


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


        //Incomplete
        public IActionResult DetailsStudent(int id)
        {
            var student = studentService.GetStudentDetailsById(id);

            return View(student);
        }


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

        
        public IActionResult DeleteEnrolment(int subjectId, int studentId)
        {
            studentService.DeleteEnrolmentFromSubjectStudentMapped(subjectId, studentId);
            
            return RedirectToAction(actionName: "DetailsStudent", controllerName: "Student", new { @id = studentId });
        }
    }
}
