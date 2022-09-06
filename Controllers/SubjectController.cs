using Microsoft.AspNetCore.Mvc;
using University_Information_System.Models;
using University_Information_System.Services;

namespace University_Information_System.Controllers
{
    public class SubjectController : Controller
    {
        private readonly IMainService mainServiceSubject;

        public SubjectController(IMainService mainServices)
        {
            this.mainServiceSubject = mainServices;
        }


        public IActionResult Subjects()
        {
            var subjects = mainServiceSubject.getAllSubject();
            if(subjects == null)
            {
                subjects = new List<Subject>();
            }

            
            return View(subjects);
        }

        public IActionResult AddSubject()
        {
            
            return View();
        }

        [HttpPost]
        public IActionResult AddSubject(Subject subject)
        {
             subject.CreatedDate = DateTime.Now;
            mainServiceSubject.AddSubject(subject);
                //TempData["success"] = "Successfully Added!";
            
            return RedirectToAction(actionName:"Subjects", controllerName:"Subject");

        }


        public IActionResult DeleteSubject(int id)
        {
            var subject = mainServiceSubject.GetSubjectById(id);

            return View(subject);
        }

        [HttpPost]
        public IActionResult DeleteSubject(Subject subject)
        {
            //mainService.AddProject(project);
            mainServiceSubject.DeleteSubject(subject);
            
            return RedirectToAction(actionName: "Subjects", controllerName: "Subject");
        }

        public IActionResult UpdateSubject(int id)
        {
            var subject = mainServiceSubject.GetSubjectById(id);

            return View(subject);
        }

        [HttpPost]
        public IActionResult UpdateSubject(Subject subject)
        {
            //mainService.AddProject(project);
            mainServiceSubject.UpdateSubject(subject);
            
            return RedirectToAction(actionName: "Subjects", controllerName: "Subject");
        }

        public IActionResult DetailsSubject(int id)
        {
            var subject = mainServiceSubject.GetSubjectDetailsById(id);

            return View(subject);
        }

        
    }
}
