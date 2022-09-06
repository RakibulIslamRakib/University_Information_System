using Microsoft.AspNetCore.Mvc;
using University_Information_System.Models;
using University_Information_System.Services;

namespace University_Information_System.Controllers
{
    public class DepertmentController : Controller
    {
        private readonly IMainService mainServicesDept;

        public DepertmentController(IMainService mainServices)
        {
            this.mainServicesDept = mainServices;
        }


        public IActionResult Depertments()
        {
            var depertments = mainServicesDept.getAllDepertment();
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
            mainServicesDept.AddDepertment(depertment);
            //TempData["success"] = "Successfully Added!";
            return RedirectToAction(actionName: "Depertments", controllerName: "Depertment");

        }


        public IActionResult DeleteDepertment(int id)
        {
            var department = mainServicesDept.GetDepertmentById(id);


            return View(department);
        }

        [HttpPost]
        public IActionResult DeleteDepertment(Depertment depertment)
        {
            //mainService.AddProject(project);

            mainServicesDept.DeleteDepertment(depertment);

            return RedirectToAction(actionName: "Depertments", controllerName: "Depertment");
        }

        public IActionResult UpdateDepertment(int id)
        {
            var project = mainServicesDept.GetDepertmentById(id);

            return View(project);
        }

        [HttpPost]
        public IActionResult UpdateDepertment(Depertment depertment)
        {
            //mainService.AddProject(project);
            mainServicesDept.UpdateDepertment(depertment);
            return RedirectToAction(actionName: "Depertments", controllerName: "Depertment");
        }

        public IActionResult DetailsDepertment(int id)
        {
            var depertment = mainServicesDept.GetDepertmentDetailsById(id);

            return View(depertment);
        }

        public IActionResult AddSubjectToTheDepertment(int id)
        {

            TempData["departmentId"] = id;
            var subjects = new List<Subject>(mainServicesDept.getAllSubject());
            var subjectOftheDept = new List<Subject>(mainServicesDept.GetSubjectByDepertmentId(id));
            var subjectOutOftheDept = subjects.Except(subjectOftheDept).ToList();

            return View(subjectOutOftheDept);
        }

        [HttpPost]
        public IActionResult AddSubjectToTheDepertment(int id, int subjectId)
        {
            var subDept = new SubjectDepartmentMapped();
            subDept.departmentId = id;
            subDept.subjectId = subjectId;
            //int departmentId = subjectDepartmentMapped.departmentId;
            //subjectDepartmentMapped.id = 0;
            //subjectDepartmentMapped.departmentId = departmentId;
            mainServicesDept.AddSubjectDapertmentMapped(subDept);
            return RedirectToAction(actionName: "DetailsDepertment" , controllerName:"Depertment" , new { @id = id });
    
        }

        public IActionResult DeleteSubjectFromDept(int subjectId,int deptId)
        {
           mainServicesDept.DeleteSubjectFromSubjectDepartmentMapped(subjectId, deptId);
            return RedirectToAction(actionName: "Depertments", controllerName: "Depertment");
        }


        public IActionResult DeleteStudentFromDept(int studentId, int deptId)
        {
            var student = mainServicesDept.GetStudentById(studentId);
            mainServicesDept.DeleteStudent(student);
            return RedirectToAction(actionName: "DetailsDepertment", controllerName: "Depertment", new {@id = deptId});
        }


    }
}
