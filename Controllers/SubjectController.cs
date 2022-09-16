﻿using Microsoft.AspNetCore.Mvc;
using University_Information_System.Models;
using University_Information_System.Services.ServiceInterfaces;

namespace University_Information_System.Controllers
{
    public class SubjectController : Controller

    {
        #region Fields

        private readonly ISubjectService subjectService;

        #endregion Fields



        #region ctor

        public SubjectController(ISubjectService subjectService)
        {
            this.subjectService = subjectService;
        }

        #endregion ctor



        #region Subjects


        public async Task<IActionResult> Subjects(string currentFilter,
                    string searchString, int? pageNumber, int? itemsPerPage)
        {
            var subjects = subjectService.getAllSubject();

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
                subjects = subjects.Where(sub => sub.SubjectName.ToLower().Contains(searchString)
                || sub.Descryption.ToLower().Contains(searchString));
            }
 
            return View(await PaginatedList<Subject>.CreateAsync(subjects, pageNumber ?? 1, pageSize));
        }

        #endregion Subjects

        #region AddSubject
        public IActionResult AddSubject()
        {
            
            return View();
        }

        [HttpPost]
        public IActionResult AddSubject(Subject subject)
        {
            subject.CreatedDate = DateTime.Now;
            subject.CreatedBy = 12;
            if (ModelState.IsValid)
            {
                subjectService.AddSubject(subject);
                return RedirectToAction(actionName: "Subjects", controllerName: "Subject");
            }
            return View(subject);

        }
        #endregion AddSubject

        #region DeleteSubject
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
        #endregion DeleteSubject


        #region UpdateSubject
        public IActionResult UpdateSubject(int id)
        {
            var subject = subjectService.GetSubjectById(id);

            return View(subject);
        }

        [HttpPost]
        public IActionResult UpdateSubject(Subject subject)
        {
            subject.UpdatedDate=DateTime.Now;
            subject.UpdatedBy= 12;
            if (ModelState.IsValid)
            {
                subjectService.UpdateSubject(subject);

                return RedirectToAction(actionName: "Subjects", controllerName: "Subject");
            }
            return View(subject);
        }
        #endregion UpdateSubject


        #region DetailsSubject
        public IActionResult DetailsSubject(int id)
        {
            var subject = subjectService.GetSubjectDetailsById(id);

            return View(subject);
        }


        #endregion DetailsSubject

    }
}
