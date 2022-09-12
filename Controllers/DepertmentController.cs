
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using University_Information_System.Models;
using University_Information_System.Services.ServiceClasses;
using University_Information_System.Services.ServiceInterfaces;

namespace University_Information_System.Controllers
{
    public class DepertmentController : Controller
    {
        private readonly IDepartmentService departmentService;

        public DepertmentController(IDepartmentService departmentService)
        {
            this.departmentService = departmentService;
        }


        public async Task<IActionResult> Depertments(string currentFilter,
                    string searchString, int? pageNumber)
        {      
            var depertments = departmentService.getAllDepertment();

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            searchString = !String.IsNullOrEmpty(searchString)? searchString.ToLower():"";

            if (!String.IsNullOrEmpty(searchString))
            {
                depertments =depertments.Where(dept => dept.DeptName.ToLower().Contains(searchString) 
                || dept.Descryption.ToLower().Contains(searchString));
            }
            int pageSize = 3;

            return View(await PaginatedList<Depertment>.CreateAsync(depertments, pageNumber ?? 1, pageSize));
        }

        public IActionResult AddDepertment()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddDepertment(Depertment depertment)
        {
            depertment.CreatedDate = DateTime.Now;
            depertment.CreatedBy = 1;

            //var err = ModelState.Values.SelectMany(er => er.Errors);

            if (ModelState.IsValid)
            {
                departmentService.AddDepertment(depertment);

                return RedirectToAction(actionName: "Depertments", controllerName: "Depertment");
            }
            
            return View(depertment);
        }


        public IActionResult DeleteDepertment(int id)
        {
            var department = departmentService.GetDepertmentById(id);

            return View(department);
        }

        [HttpPost]
        public IActionResult DeleteDepertment(Depertment depertment)
        {
            departmentService.DeleteDepertment(depertment);

            return RedirectToAction(actionName: "Depertments", controllerName: "Depertment");
        }

        public IActionResult UpdateDepertment(int id)
        {
            var updatedDept = departmentService.GetDepertmentById(id);

            return View(updatedDept);
        }

        [HttpPost]
        public IActionResult UpdateDepertment(Depertment depertment)
        {
            //var updatedDeptVar = departmentService.GetDepertmentById(depertment.id);   
            depertment.UpdatedDate= DateTime.Now;
            depertment.UpdatedBy = 1;
            //var err = ModelState.Values.SelectMany(er => er.Errors);

            if (ModelState.IsValid)
            {
                departmentService.UpdateDepertment(depertment);

                return RedirectToAction(actionName: "Depertments", controllerName: "Depertment");
            }

            return View(depertment);
        }

        public IActionResult DetailsDepertment(int id)
        {
            var depertment = departmentService.GetDepertmentDetailsById(id);

            return View(depertment);
        }

        public IActionResult AddSubjectToTheDepertment(int id)
        {
            TempData["departmentId"] = id;
            var subjectOutOftheDept = departmentService.SubjectOutOfDept(id);
            
            return View(subjectOutOftheDept);
        }

        [HttpPost]
        public IActionResult AddSubjectToTheDepertment(int id, int subjectId)
        {
            var subDept = new SubjectDepartmentMapped();
            subDept.departmentId = id;
            subDept.subjectId = subjectId;
            departmentService.AddSubjectDapertmentMapped(subDept);
            
            return RedirectToAction(actionName: "DetailsDepertment" , controllerName:"Depertment" , new { @id = id });
    
        }

        public IActionResult DeleteSubjectFromDept(int subjectId,int deptId)
        {
            departmentService.DeleteSubjectFromSubjectDepartmentMapped(subjectId, deptId);
            
            return RedirectToAction(actionName: "Depertments", controllerName: "Depertment");
        }


        public IActionResult DeleteStudentFromDept(int studentId, int deptId)
        {
            departmentService.DeleteStudentFromDept(studentId);
            
            return RedirectToAction(actionName: "DetailsDepertment", controllerName: "Depertment", new {@id = deptId});
        }

    }
}
