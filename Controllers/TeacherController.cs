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
                    string searchString, int? pageNumber, int? itemsPerPage)
        {
            var teachers = await teacherService.getAllTeacher();

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
                teachers = teachers.Where(tr => tr.FirstName.ToLower().Contains(searchString)
                || tr.LastName.ToLower().Contains(searchString) || tr.Descryption.ToLower().Contains(searchString)).ToList();
            }

 

            return View(PaginatedList<Teacher>.Create(teachers, pageNumber ?? 1, pageSize));

        }
        #endregion Teachers

        #region AddTeacher
        public IActionResult AddTeacher()
        {
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddTeacher(Teacher teacher)
        {
            if (ModelState.IsValid)
            {

               await teacherService.AddTeacher(teacher);

                return RedirectToAction(actionName: "Teachers", controllerName: "Teacher");
            }
            return View(teacher);
        }

        #endregion AddTeacher

        #region DeleteTeacher
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            var teacher = await teacherService.GetTeacherById(id);
            if(teacher == null)return View("Error");    
            return View(teacher);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTeacher(Teacher teacher)
        {
            await teacherService.DeleteTeacher(teacher);
            
            return RedirectToAction(actionName: "Teachers", controllerName: "Teacher");
        }
        #endregion DeleteTeacher

        #region  UpdateTeacher
        public async Task<IActionResult> UpdateTeacher(int id)
        {

            var teacher = await teacherService.GetTeacherById(id);

            return View(teacher);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTeacher(Teacher teacher)
        {
            if (ModelState.IsValid)
            {

               await teacherService.UpdateTeacher(teacher);

                return RedirectToAction(actionName: "Teachers", controllerName: "Teacher");
            }

            return View(teacher);
        }
        #endregion  UpdateTeacher

        #region  DetailsTeacher
        public async Task<IActionResult> DetailsTeacher(int id, string currentFilter,
                    string searchString, int? pageSize, int? pageIndex, string atributeType = "Subjects")

        {
            var teacher = await teacherService.GetTeacherDetailsById(id);

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            teacher.PageSize = pageSize ?? 5;
            teacher.SearchString = searchString;
            teacher.PageIndex = pageIndex ?? 1;
            ViewData["atributeType"] = atributeType ?? "Subjects";

            searchString = !String.IsNullOrEmpty(searchString) ? searchString.ToLower() : "";

            var students = teacher.Students;
            var subjects = teacher.Subjects;
            var departments = teacher.Depertments;

            if (atributeType == "Students")
            {

                if (!String.IsNullOrEmpty(searchString))
                {
                    students = students.Where(st => st.FirstName.ToLower().Contains(searchString)
                    || st.LastName.ToLower().Contains(searchString)).ToList();
                }

                teacher.TotalPages = (int)Math.Ceiling(students.Count / (double) teacher.PageSize);
                var items = students.Skip((teacher.PageIndex - 1) * teacher.PageSize).Take(teacher.PageSize).ToList();
                teacher.Students = items;
                teacher.Pages = teacher.GetPages(students.Count, teacher.PageSize);
                teacher.Subjects = new List<Subject>();
                teacher.Depertments = new List<Depertment>();


            }


            else if (atributeType == "Subjects")
            {

                if (!String.IsNullOrEmpty(searchString))
                {
                    subjects = subjects.Where(sb => sb.SubjectName.ToLower().Contains(searchString)
                    || sb.Descryption.ToLower().Contains(searchString)).ToList();
                }

                teacher.TotalPages = (int)Math.Ceiling(subjects.Count / (double)teacher.PageSize);
                var items = subjects.Skip((teacher.PageIndex - 1) * teacher.PageSize).Take(teacher.PageSize).ToList();
                teacher.Subjects = items;
                teacher.Pages = teacher.GetPages(subjects.Count, teacher.PageSize);
                teacher.Students = new List<Student>();
                teacher.Depertments = new List<Depertment>();
            }


            else
            {

                if (!String.IsNullOrEmpty(searchString))
                {
                    teachers = teachers.Where(tr => tr.FirstName.ToLower().Contains(searchString)
                    || tr.LastName.ToLower().Contains(searchString)).ToList();
                }
                teacher.TotalPages = (int)Math.Ceiling(teachers.Count / (double)teacher.PageSize);
                var items = teachers.Skip((depertment.PageIndex - 1) * teacher.PageSize).Take(teacher.PageSize).ToList();
                teacher.Pages = teacher.GetPages(teachers.Count, teacher.PageSize);
                teacher.Teachers = items;
                teacher.Subjects = new List<Subject>();
                teacher.Students = new List<Student>();
            }

            return View(depertment);

            return View(teacher);
        }

        #endregion  DetailsTeacher


        #region  AddSubjectToTheTeacher
        public async Task<IActionResult> AddSubjectToTheTeacher(int id)
        {

            TempData["teacherId"] = id;
            var subjects = await subjectService.getAllSubject();

            var subjectOftheTeacher = await teacherService.GetSubjectByTeacherId(id);

            var subjectOutOftheTeacher =  subjects.Except(subjectOftheTeacher);

            return View(subjectOutOftheTeacher);
        }
       

        [HttpPost]
        public async Task<IActionResult> AddSubjectToTheTeacher(int id, int subjectId)
        {
            var subTeacher = new SubjectTeacherMapped
            {
                TeacherId = id,
                SubjectId = subjectId
            };
            await teacherService.AddSubjectTeacherMapped(subTeacher);

            return RedirectToAction(actionName: "DetailsTeacher", controllerName: "Teacher", new { @id = id });

        }
        #endregion  AddSubjectToTheTeacher

        #region  DeleteSubjectFromTeacher
        public async Task<IActionResult> DeleteSubjectFromTeacher(int subjectId, int TeacherId)
        {
            await teacherService.DeleteSubTeacherMapped(subjectId, TeacherId);
            return RedirectToAction(actionName: "DetailsTeacher", controllerName: "Teacher", new {@id = TeacherId});
        }

        #endregion DeleteSubjectFromTeacher
    }
}
