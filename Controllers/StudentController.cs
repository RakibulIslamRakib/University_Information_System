using Microsoft.AspNetCore.Mvc;
using University_Information_System.Models;
using University_Information_System.Services;

namespace University_Information_System.Controllers
{
    public class StudentController : Controller
    {
        private readonly IMainService mainServiceStudent;

        public StudentController(IMainService mainServices)
        {
            this.mainServiceStudent = mainServices;
        }


        public IActionResult Students()
        {
            var students = mainServiceStudent.getAllStudent();
            if (students == null)
            {
                students = new List<Student>();
            }


            return View(students);
        }

        public IActionResult AddStudent()
        {
            var depertments = mainServiceStudent.getAllDepertment();
            return View(depertments);
        }

        [HttpPost]
        public IActionResult AddStudent(Student student)
        {

            mainServiceStudent.AddStudent(student);
            //TempData["success"] = "Successfully Added!";

            return RedirectToAction(actionName: "Students", controllerName: "Student");

        }


        public IActionResult DeleteStudent(int id)
        {
            var student = mainServiceStudent.GetStudentById(id);
            student.DeptName = mainServiceStudent.GetDepertmentById(student.DepertmentId).DeptName;

            return View(student);
        }


        [HttpPost]
        public IActionResult DeleteStudent(Student student)
        {
            //mainService.AddProject(project);
            mainServiceStudent.DeleteStudent(student);

            return RedirectToAction(actionName: "Students", controllerName: "Student");
        }

        public IActionResult UpdateStudent(int id)
        {
            var student = mainServiceStudent.GetStudentById(id);

            return View(student);
        }

        [HttpPost]
        public IActionResult UpdateStudent(Student student)
        {
            var updatedStudent = mainServiceStudent.GetStudentById(student.id);
            updatedStudent.FirstName = student.FirstName;
            updatedStudent.LastName = student.LastName;
            updatedStudent.Reg = student.Reg;
            //mainService.AddProject(project);
            mainServiceStudent.UpdateStudent(updatedStudent);

            return RedirectToAction(actionName: "Students", controllerName: "Student");
        }


        //Incomplete
        public IActionResult DetailsStudent(int id)
        {
            var student = mainServiceStudent.GetStudentDetailsById(id);

            return View(student);
        }


        public IActionResult EnroleSubject(int id)
        {
            TempData["StudentId"] = id;
            var student = mainServiceStudent.GetStudentById(id);
            var subjectOftheDept = new List<Subject>(mainServiceStudent.GetSubjectByDepertmentId(student.DepertmentId));
            var subjectOfTheStudent = mainServiceStudent.GetSubjectByStudentId(id);
            var subjectOutOfTheStudent = subjectOftheDept.Except(subjectOfTheStudent).ToList();

            return View(subjectOutOfTheStudent);

        }

        [HttpPost]
        public IActionResult EnroleSubject(int id, int subjectId)
        {
            var enrollment = new SubjectStudentMapped();
            enrollment.subjectId = subjectId;
            enrollment.studentId= id;
            //int departmentId = subjectDepartmentMapped.departmentId;
            //subjectDepartmentMapped.id = 0;
            //subjectDepartmentMapped.departmentId = departmentId;
            mainServiceStudent.AddSubjectStudentMapped(enrollment);
            return RedirectToAction(actionName: "DetailsStudent", controllerName: "Student", new { @id = id });

            return View();

        }

        
            public IActionResult DeleteEnrolment(int subjectId, int studentId)
        {
            mainServiceStudent.DeleteEnrolmentFromSubjectStudentMapped(subjectId, studentId);
            return RedirectToAction(actionName: "DetailsStudent", controllerName: "Student", new { @id = studentId });
        }
    }
}
