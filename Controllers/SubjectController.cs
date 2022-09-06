using Microsoft.AspNetCore.Mvc;
using University_Information_System.Models;
using University_Information_System.Services.ServiceInterfaces;

namespace University_Information_System.Controllers
{
    public class SubjectController : Controller
    {
        private readonly ISubjectService subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            this.subjectService = subjectService;
        }


        public IActionResult Subjects()
        {
            var subjects = subjectService.getAllSubject();
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
            subjectService.AddSubject(subject);
            
            return RedirectToAction(actionName:"Subjects", controllerName:"Subject");

        }


        public IActionResult DeleteSubject(int id)
        {
            var subject = subjectService.GetSubjectById(id);

            return View(subject);
        }

        [HttpPost]
        public IActionResult DeleteSubject(Subject subject)
        {
            subjectService.DeleteSubject(subject);
            
            return RedirectToAction(actionName: "Subjects", controllerName: "Subject");
        }

        public IActionResult UpdateSubject(int id)
        {
            var subject = subjectService.GetSubjectById(id);

            return View(subject);
        }

        [HttpPost]
        public IActionResult UpdateSubject(Subject subject)
        {
            subjectService.UpdateSubject(subject);
            
            return RedirectToAction(actionName: "Subjects", controllerName: "Subject");
        }

        public IActionResult DetailsSubject(int id)
        {
            var subject = subjectService.GetSubjectDetailsById(id);

            return View(subject);
        }

        
    }
}
