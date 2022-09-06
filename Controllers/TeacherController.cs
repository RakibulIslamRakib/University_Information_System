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

        public IActionResult DetailsTeacher(int id)
        {
            var teacher = mainServiceTeacher.GetTeacherDetailsById(id);

            return View(teacher);
        }


        //AddSubjectToTheTeacher


        public IActionResult AddSubjectToTheTeacher(int id)
        {

            TempData["teacherId"] = id;
            var subjects = new List<Subject>(mainServiceTeacher.getAllSubject());

            var subjectOftheTeacher = new List<Subject>(mainServiceTeacher.GetSubjectByTeacherId(id));

            var subjectOutOftheTeacher = subjects.Except(subjectOftheTeacher).ToList();

            return View(subjectOutOftheTeacher);
        }

        [HttpPost]
        public IActionResult AddSubjectToTheTeacher(int id, int subjectId)
        {
            var subTeacher = new SubjectTeacherMapped();
            subTeacher.TeacherId = id;
            subTeacher.SubjectId = subjectId;
            //int departmentId = subjectDepartmentMapped.departmentId;
            //subjectDepartmentMapped.id = 0;
            //subjectDepartmentMapped.departmentId = departmentId;
            mainServiceTeacher.AddSubjectTeacherMapped(subTeacher);
            return RedirectToAction(actionName: "DetailsTeacher", controllerName: "Teacher", new { @id = id });

        }


        public IActionResult DeleteSubjectFromTeacher(int subjectId, int TeacherId)
        {
            mainServiceTeacher.DeleteSubjectFromSubjectTeacherMapped(subjectId, TeacherId);
            return RedirectToAction(actionName: "DetailsTeacher", controllerName: "Teacher", new {@id = TeacherId});
        }

    }
}
