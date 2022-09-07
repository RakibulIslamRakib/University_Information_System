using Microsoft.AspNetCore.Mvc;
using University_Information_System.Models;
using University_Information_System.Services;
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


        public IActionResult Depertments()
        {
            var depertments = departmentService.getAllDepertment();
            if (depertments == null)
            {
                depertments = new List<Depertment>();
            }

            return View(depertments);
        }

        public IActionResult AddDepertment()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddDepertment(Depertment depertment)
        {
            depertment.CreatedDate = DateTime.Now;
            departmentService.AddDepertment(depertment);

            return RedirectToAction(actionName: "Depertments", controllerName: "Depertment");

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
            departmentService.UpdateDepertment(depertment);

            return RedirectToAction(actionName: "Depertments", controllerName: "Depertment");
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
