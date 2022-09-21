
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using University_Information_System.Models;
using University_Information_System.Services.ServiceInterfaces;

namespace University_Information_System.Controllers
{
    public class DepertmentController : Controller
    {
        #region Fields

        private readonly IDepartmentService departmentService;

        #endregion Fields



        #region ctor

        public DepertmentController(IDepartmentService departmentService)
        {
            this.departmentService = departmentService;
        }

        #endregion ctor


        #region Depertments
        public async Task<IActionResult> Depertments(string currentFilter, 
                    string searchString, int? pageNumber, int? itemsPerPage)
        {      
            var depertments = await departmentService.GetAllDepertment();

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

            searchString = !String.IsNullOrEmpty(searchString)? searchString.ToLower():"";

            if (!String.IsNullOrEmpty(searchString))
            {
                depertments = depertments.Where(dept => dept.DeptName.ToLower().Contains(searchString)
                || dept.Descryption.ToLower().Contains(searchString)).ToList();
            }
       
            return View(PaginatedList<Depertment>.Create(depertments, pageNumber ?? 1, pageSize));
        }



        #endregion Depertments


        #region AddDepertment
        public IActionResult AddDepertment()
        {
            return  View();
        }

        [HttpPost]
        public async Task<IActionResult> AddDepertment(Depertment depertment)
        {
            depertment.CreatedDate = DateTime.Now;
            depertment.CreatedBy = 1;

            //var err = ModelState.Values.SelectMany(er => er.Errors);

            if (ModelState.IsValid)
            {
               await departmentService.AddDepertment(depertment);

                return RedirectToAction(actionName: "Depertments", controllerName: "Depertment");
            }
            
            return View(depertment);
        }

        #endregion AddDepertment


        #region DeleteDepertment
        public async Task<IActionResult> DeleteDepertment(int id)
        {
            var department = await departmentService.GetDepertmentById(id);

            return View(department);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDepertment(Depertment depertment)
        {
            await departmentService.DeleteDepertment(depertment);

            return RedirectToAction(actionName: "Depertments", controllerName: "Depertment");
        }
        #endregion DeleteDepertment


        #region UpdateDepertment
        public async Task<IActionResult> UpdateDepertment(int id)
        {
            var updatedDept = await departmentService.GetDepertmentById(id);

            return View(updatedDept);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDepertment(Depertment depertment)
        { 
            depertment.UpdatedDate= DateTime.Now;
            depertment.UpdatedBy = 1;
            //var err = ModelState.Values.SelectMany(er => er.Errors);

            if (ModelState.IsValid)
            {
               await departmentService.UpdateDepertment(depertment);

                return RedirectToAction(actionName: "Depertments", controllerName: "Depertment");
            }

            return View(depertment);
        }

        #endregion UpdateDepertment


        #region DetailsDepertment
        public async Task<IActionResult> DetailsDepertment(int id, string currentFilter,
                    string searchString,  int? pageSize , int? pageIndex, string atributeType = "Subjects")
        {
            var depertment = await departmentService.GetDepertmentDetailsById(id);

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            depertment.PageSize = pageSize ?? 5;
            depertment.SearchString = searchString;
            depertment.PageIndex = pageIndex??1;
            ViewData["atributeType"] = atributeType ?? "Subjects";

            searchString = !String.IsNullOrEmpty(searchString) ? searchString.ToLower() : "";

            var students = depertment.Students;
            var subjects = depertment.Subjects;
            var teachers = depertment.Teachers;

            if (atributeType == "Students")
            {

                if (!String.IsNullOrEmpty(searchString))
                {
                    students = students.Where(st => st.FirstName.ToLower().Contains(searchString)
                    || st.LastName.ToLower().Contains(searchString)).ToList();
                }

                    depertment.TotalPages = (int)Math.Ceiling(students.Count / (double)depertment.PageSize);
                    var items = students.Skip((depertment.PageIndex - 1) * depertment.PageSize).Take(depertment.PageSize).ToList();
                    depertment.Students = items;
                    depertment.Pages = depertment.GetPages(students.Count, depertment.PageSize);
                    depertment.Subjects = new List<Subject>();
                    depertment.Teachers = new List<Teacher>();
                 

            }


            else if (atributeType == "Subjects")
            {

                if (!String.IsNullOrEmpty(searchString))
                {
                    subjects = subjects.Where(sb => sb.SubjectName.ToLower().Contains(searchString)
                    || sb.Descryption.ToLower().Contains(searchString)).ToList();
                }

                depertment.TotalPages = (int)Math.Ceiling(subjects.Count / (double)depertment.PageSize);
                var items = subjects.Skip((depertment.PageIndex - 1) * depertment.PageSize).Take(depertment.PageSize).ToList();
                depertment.Subjects = items;
                depertment.Pages = depertment.GetPages(subjects.Count, depertment.PageSize);
                depertment.Students = new List<Student>();
                depertment.Teachers = new List<Teacher>();
            }

         
            else
            {

                if (!String.IsNullOrEmpty(searchString))
                {
                    teachers = teachers.Where(tr => tr.FirstName.ToLower().Contains(searchString)
                    || tr.LastName.ToLower().Contains(searchString)).ToList();
                }
                depertment.TotalPages = (int)Math.Ceiling(teachers.Count / (double)depertment.PageSize);
                var items = teachers.Skip((depertment.PageIndex - 1) * depertment.PageSize).Take(depertment.PageSize).ToList();
                depertment.Pages = depertment.GetPages(teachers.Count, depertment.PageSize);
                depertment.Teachers = items;
                depertment.Subjects = new List<Subject>();
                depertment.Students = new List<Student>();
            }

            return View(depertment);
        }

        #endregion DetailsDepertment


        #region AddSubjectToTheDepertment
        public async Task<IActionResult> AddSubjectToTheDepertment(int id)
        {
            TempData["departmentId"] = id;
            var subjectOutOftheDept = await departmentService.SubjectOutOfDept(id);
            
            return View(subjectOutOftheDept);
        }

        [HttpPost]
        public async Task<IActionResult> AddSubjectToTheDepertment(int id, int subjectId)
        {
            var subDept = new SubjectDepartmentMapped
            {
                departmentId = id,
                subjectId = subjectId
            };

            await departmentService.AddSubjectDapertmentMapped(subDept);
            
            return RedirectToAction(actionName: "DetailsDepertment" , controllerName:"Depertment" , new { @id = id });
    
        }

        #endregion AddSubjectToTheDepertment


        #region DeleteSubjectFromDept
        public async Task<IActionResult> DeleteSubjectFromDept(int subjectId,int deptId)
        {
            await departmentService.DeleteSubjectFromSubjectDepartmentMapped(subjectId, deptId);
            
            return RedirectToAction(actionName: "DetailsDepertment", controllerName: "Depertment", new { @id = deptId });
        }


        public async Task<IActionResult> DeleteStudentFromDept(int studentId, int deptId)
        {
            await departmentService.DeleteStudentFromDept(studentId);
            
            return RedirectToAction(actionName: "DetailsDepertment", controllerName: "Depertment", new {@id = deptId});
        }

        #endregion DeleteSubjectFromDept

    }
}
