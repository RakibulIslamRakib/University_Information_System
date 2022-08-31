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
            if(students== null)
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
            
            return RedirectToAction(actionName:"Students", controllerName:"Student");

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
            var subject = mainServiceStudent.GetSubjectById(id);

            return View(subject);
        }

        
    }
}
