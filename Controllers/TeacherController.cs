using Microsoft.AspNetCore.Mvc;
using University_Information_System.Models;
using University_Information_System.Services.ServiceClasses;
using University_Information_System.Services.ServiceInterfaces;

namespace University_Information_System.Controllers
{
    public class TeacherController : Controller
    {
        private readonly ITeacherService teacherService;

        public TeacherController(ITeacherService teacherService)
        {
            this.teacherService = teacherService;
        }


        public IActionResult Teachers()
        {
            var teachers = teacherService.getAllTeacher();
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
            teacherService.AddTeacher(teacher);
            
            return RedirectToAction(actionName: "Teachers", controllerName: "Teacher");

        }


        public IActionResult DeleteTeacher(int id)
        {
            var teacher = teacherService.GetTeacherById(id);

            return View(teacher);
        }

        [HttpPost]
        public IActionResult DeleteTeacher(Teacher teacher)
        {
            teacherService.DeleteTeacher(teacher);
            
            return RedirectToAction(actionName: "Teachers", controllerName: "Teacher");
        }

        public IActionResult UpdateTeacher(int id)
        {
            var teacher = teacherService.GetTeacherById(id);

            return View(teacher);
        }

        [HttpPost]
        public IActionResult UpdateTeacher(Teacher teacher)
        {
            teacherService.UpdateTeacher(teacher);
            
            return RedirectToAction(actionName: "Teachers", controllerName: "Teacher");
        }

        public IActionResult DetailsTeacher(int id)
        {
            var teacher = teacherService.GetTeacherDetailsById(id);

            return View(teacher);
        }


        //AddSubjectToTheTeacher


        public IActionResult AddSubjectToTheTeacher(int id)
        {

            TempData["teacherId"] = id;
            var subjects = new List<Subject>(teacherService.getAllSubject());

            var subjectOftheTeacher = new List<Subject>(teacherService.GetSubjectByTeacherId(id));

            var subjectOutOftheTeacher = subjects.Except(subjectOftheTeacher).ToList();

            return View(subjectOutOftheTeacher);
        }

        [HttpPost]
        public IActionResult AddSubjectToTheTeacher(int id, int subjectId)
        {
            var subTeacher = new SubjectTeacherMapped();
            subTeacher.TeacherId = id;
            subTeacher.SubjectId = subjectId;
            teacherService.AddSubjectTeacherMapped(subTeacher);

            return RedirectToAction(actionName: "DetailsTeacher", controllerName: "Teacher", new { @id = id });

        }


        public IActionResult DeleteSubjectFromTeacher(int subjectId, int TeacherId)
        {
            teacherService.DeleteSubjectFromSubjectTeacherMapped(subjectId, TeacherId);
            return RedirectToAction(actionName: "DetailsTeacher", controllerName: "Teacher", new {@id = TeacherId});
        }

    }
}
