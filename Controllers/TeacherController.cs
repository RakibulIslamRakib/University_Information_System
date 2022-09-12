using Microsoft.AspNetCore.Mvc;
using University_Information_System.Models;
using University_Information_System.Services.ServiceInterfaces;

namespace University_Information_System.Controllers
{
    public class TeacherController : Controller
    {
        private readonly ITeacherService teacherService;
        private readonly ISubjectService subjectService;

        public TeacherController(ITeacherService teacherService, ISubjectService subjectService)
        {
            this.teacherService = teacherService;
            this.subjectService = subjectService;
        }


        public async Task<IActionResult> Teachers(string currentFilter,
                    string searchString, int? pageNumber)
        {
            var teachers = teacherService.getAllTeacher();

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
                teachers = teachers.Where(tr => tr.FirstName.ToLower().Contains(searchString)
                || tr.LastName.ToLower().Contains(searchString) || tr.Descryption.ToLower().Contains(searchString));
            }



            int pageSize = 3;


            return View(await PaginatedList<Teacher>.CreateAsync(teachers, pageNumber ?? 1, pageSize));

        }

        public IActionResult AddTeacher()
        {
            
            return View();
        }

        [HttpPost]
        public IActionResult AddTeacher(Teacher teacher)
        {
            if (ModelState.IsValid)
            {

                teacherService.AddTeacher(teacher);

                return RedirectToAction(actionName: "Teachers", controllerName: "Teacher");
            }
            return View(teacher);
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
            if (ModelState.IsValid)
            {

                teacherService.UpdateTeacher(teacher);

                return RedirectToAction(actionName: "Teachers", controllerName: "Teacher");
            }

            return View(teacher);
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
            var subjects = subjectService.getAllSubject();

            var subjectOftheTeacher =  teacherService.GetSubjectByTeacherId(id);

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
            teacherService.DeleteSubTeacherMapped(subjectId, TeacherId);
            return RedirectToAction(actionName: "DetailsTeacher", controllerName: "Teacher", new {@id = TeacherId});
        }

    }
}
