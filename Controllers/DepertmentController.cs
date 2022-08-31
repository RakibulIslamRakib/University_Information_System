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
            if(depertments == null)
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
             return RedirectToAction(actionName:"Depertments", controllerName:"Depertment");

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
            var depertments = mainServicesDept.GetDepertmentById(id);

            return View(depertments);
        }

        
    }
}
