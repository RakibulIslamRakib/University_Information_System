﻿using Microsoft.AspNetCore.Mvc;
using University_Information_System.Models;
using University_Information_System.Services.ServiceInterfaces;

namespace University_Information_System.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService studentService;

        public StudentController(IStudentService studentService)
        {
            this.studentService = studentService;
        }


        public IActionResult Students()
        {
            var students = studentService.getAllStudent();
            if (students == null)
            {
                students = new List<Student>();
            }


            return View(students);
        }

        public IActionResult AddStudent()
        {
            var depertments = studentService.getAllDepertment();
            return View(depertments);
        }

        [HttpPost]
        public IActionResult AddStudent(Student student)
        {
            studentService.AddStudent(student);
         
            return RedirectToAction(actionName: "Students", controllerName: "Student");

        }


        public IActionResult DeleteStudent(int id)
        {
            var student = studentService.GetStudentById(id);
            student.DeptName = studentService.GetDepertmentById(student.DepertmentId).DeptName;

            return View(student);
        }


        [HttpPost]
        public IActionResult DeleteStudent(Student student)
        {
            studentService.DeleteStudent(student);

            return RedirectToAction(actionName: "Students", controllerName: "Student");
        }

        public IActionResult UpdateStudent(int id)
        {
            var student = studentService.GetStudentById(id);

            return View(student);
        }

        [HttpPost]
        public IActionResult UpdateStudent(Student student)
        {
            var updatedStudent = studentService.GetStudentById(student.id);
            updatedStudent.FirstName = student.FirstName;
            updatedStudent.LastName = student.LastName;
            updatedStudent.Reg = student.Reg;
            studentService.UpdateStudent(updatedStudent);

            return RedirectToAction(actionName: "Students", controllerName: "Student");
        }


        //Incomplete
        public IActionResult DetailsStudent(int id)
        {
            var student = studentService.GetStudentDetailsById(id);

            return View(student);
        }


        public IActionResult EnroleSubject(int id)
        {
            TempData["StudentId"] = id;
            var student = studentService.GetStudentById(id);
            var subjectOftheDept = new List<Subject>(studentService.GetSubjectByDepertmentId(student.DepertmentId));
            var subjectOfTheStudent = studentService.GetSubjectByStudentId(id);
            var subjectOutOfTheStudent = subjectOftheDept.Except(subjectOfTheStudent).ToList();

            return View(subjectOutOfTheStudent);

        }

        [HttpPost]
        public IActionResult EnroleSubject(int id, int subjectId)
        {
            var enrollment = new SubjectStudentMapped();
            enrollment.subjectId = subjectId;
            enrollment.studentId= id;
            //int departmentId = subjectDepartmentMapped.departmentId;
            //subjectDepartmentMapped.id = 0;
            //subjectDepartmentMapped.departmentId = departmentId;
            studentService.AddSubjectStudentMapped(enrollment);
            return RedirectToAction(actionName: "DetailsStudent", controllerName: "Student", new { @id = id });

            return View();

        }

        
            public IActionResult DeleteEnrolment(int subjectId, int studentId)
        {
            studentService.DeleteEnrolmentFromSubjectStudentMapped(subjectId, studentId);
            return RedirectToAction(actionName: "DetailsStudent", controllerName: "Student", new { @id = studentId });
        }
    }
}