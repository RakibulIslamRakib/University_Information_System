using Microsoft.AspNetCore.Mvc;
using University_Information_System.Models;
using University_Information_System.Services;

namespace University_Information_System.Controllers
{
    public class TeacherController : Controller
    {
        private readonly IMainService mainServiceTeacher;

        public TeacherController(IMainService mainServices)
        {
            this.mainServiceTeacher = mainServices;
        }


        public IActionResult Teachers()
        {
            var teachers = mainServiceTeacher.getAllTeacher();
            if(teachers == null)
            {
                teachers = new List<Teacher>();
            }

            
            return View(teachers);
        }

        public IActionResult AddTeacher()
        {
            
            return View();
        }

        [HttpPost]
        public IActionResult AddTeacher(Teacher teacher)
        {
            mainServiceTeacher.AddTeacher(teacher);
                //TempData["success"] = "Successfully Added!";
            
            return RedirectToAction(actionName: "Teachers", controllerName: "Teacher");

        }


        public IActionResult DeleteTeacher(int id)
        {
            var teacher = mainServiceTeacher.GetTeacherById(id);

            return View(teacher);
        }

        [HttpPost]
        public IActionResult DeleteTeacher(Teacher teacher)
        {
            //mainService.AddProject(project);
            mainServiceTeacher.DeleteTeacher(teacher);
            
            return RedirectToAction(actionName: "Teachers", controllerName: "Teacher");
        }

        public IActionResult UpdateTeacher(int id)
        {
            var teacher = mainServiceTeacher.GetTeacherById(id);

            return View(teacher);
        }

        [HttpPost]
        public IActionResult UpdateTeacher(Teacher teacher)
        {
            //mainService.AddProject(project);
            mainServiceTeacher.UpdateTeacher(teacher);
            
            return RedirectToAction(actionName: "Teachers", controllerName: "Teacher");
        }

        public IActionResult DetailsSubject(int id)
        {
            var subject = mainServiceTeacher.GetSubjectById(id);

            return View(subject);
        }

        
    }
}
