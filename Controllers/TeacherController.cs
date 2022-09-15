using Microsoft.AspNetCore.Mvc;
using University_Information_System.Models;
using University_Information_System.Services.ServiceInterfaces;

namespace University_Information_System.Controllers
{
    public class TeacherController : Controller
    {


    #region Fields

        private readonly ITeacherService teacherService;
        private readonly ISubjectService subjectService;

        #endregion Fields


        #region ctor
        public TeacherController(ITeacherService teacherService, ISubjectService subjectService)
        {
            this.teacherService = teacherService;
            this.subjectService = subjectService;
        }
        #endregion ctor




        #region Teachers


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
        #endregion Teachers

        #region AddTeacher
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

        #endregion AddTeacher

        #region DeleteTeacher
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
        #endregion DeleteTeacher

        #region  UpdateTeacher
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
        #endregion  UpdateTeacher

        #region  DetailsTeacher
        public IActionResult DetailsTeacher(int id)
        {
            var teacher = teacherService.GetTeacherDetailsById(id);

            return View(teacher);
        }

        #endregion  DetailsTeacher


        #region  AddSubjectToTheTeacher
        public IActionResult AddSubjectToTheTeacher(int id)
        {

            TempData["teacherId"] = id;
            var subjects = subjectService.getAllSubject().ToList();

            var subjectOftheTeacher =  teacherService.GetSubjectByTeacherId(id);

            var subjectOutOftheTeacher = subjects.Except(subjectOftheTeacher.ToList()).ToList();

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
        #endregion  AddSubjectToTheTeacher

        #region  DeleteSubjectFromTeacher
        public IActionResult DeleteSubjectFromTeacher(int subjectId, int TeacherId)
        {
            teacherService.DeleteSubTeacherMapped(subjectId, TeacherId);
            return RedirectToAction(actionName: "DetailsTeacher", controllerName: "Teacher", new {@id = TeacherId});
        }

        #endregion DeleteSubjectFromTeacher
    }
}
