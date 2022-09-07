using System.Collections.Generic;
using University_Information_System.Data;
using University_Information_System.Models;
using University_Information_System.Services.ServiceInterfaces;

namespace University_Information_System.Services.ServiceClasses
{
    public class DepartmentService: IDepartmentService
    {

        public readonly ApplicationDbContext db;
        private readonly ISubjectService subjectService;
        private readonly IStudentService studentService;

        public DepartmentService(ApplicationDbContext db , ISubjectService subjectService, IStudentService studentService)
        {
            this.db = db;
            this.subjectService = subjectService;
            this.studentService = studentService;
        }

        public void AddDepertment(Depertment depertment)
        {
            db.Depertment.Add(depertment);
            db.SaveChanges();
        }

        public List<Depertment> getAllDepertment()
        {
            var depertments = db.Depertment.ToList();

            return depertments;
        }

        public Depertment GetDepertmentById(int id)
        {

            return db.Depertment.Find(id);
        }

        public void UpdateDepertment(Depertment depertment)
        {
            db.Depertment.Update(depertment);
            db.SaveChanges();
        }

        public void DeleteDepertment(Depertment depertment)
        {
            var studentOdTheDept = GetStudentByDepertmentId(depertment.id);
            var enrolMentsOfTheStudentsOfTheDept = new List<SubjectStudentMapped>();
            foreach (var student in studentOdTheDept)
            {
                enrolMentsOfTheStudentsOfTheDept.AddRange(db.SubjectStudentMapped.Where(enrol => enrol.studentId == student.id).ToList());
            }
            db.SubjectStudentMapped.RemoveRange(enrolMentsOfTheStudentsOfTheDept);
            db.SaveChanges();

            db.Student.RemoveRange(studentOdTheDept);
            db.SaveChanges();

            var subDeptOfTheDept = db.SubjectDepartmentMapped.Where(sd=>sd.departmentId==depertment.id).ToList();
            db.SubjectDepartmentMapped.RemoveRange(subDeptOfTheDept);
            db.SaveChanges();

            db.Depertment.Remove(depertment);
            db.SaveChanges();
        }

        public Depertment GetDepertmentDetailsById(int id)
        {
            var depertment = db.Depertment.Find(id);
            //depertment.Teachers = GetTeacherByDepertmentId(id);
            depertment.Teachers = new List<Teacher>();
            depertment.Students = GetStudentByDepertmentId(id);
            depertment.Subjects = GetSubjectByDepertmentId(id);

            return depertment;
        }


        public List<Subject> GetSubjectByDepertmentId(int id)
        {
            var subjectIdOfTheDepertment = db.SubjectDepartmentMapped.Where(sd => sd.departmentId == id ).Select(sd=>sd.subjectId).ToList();
            
            
            var allSubjectOfTheDepertment = db.Subject.Where(subject=> subjectIdOfTheDepertment.Contains(subject.id)).ToList();

            return allSubjectOfTheDepertment;
        }
        

        public List<Student> GetStudentByDepertmentId(int id)
        {

            return db.Student.Where(st => st.DepertmentId == id).ToList();
        }

        public void DeleteStudentFromDept(int studentId)
        {
            studentService.DeleteStudent(studentService.GetStudentById(studentId));
        }

        public void AddSubjectDapertmentMapped(SubjectDepartmentMapped subjectDapertmentMapped)
        {
            db.SubjectDepartmentMapped.Add(subjectDapertmentMapped);
            db.SaveChanges();
        }

        public void DeleteSubjectFromSubjectDepartmentMapped(int subjectId, int deptId)
        {
            var subDeptVar = db.SubjectDepartmentMapped.FirstOrDefault(sd => sd.departmentId == deptId && sd.subjectId==subjectId);
            
            db.SubjectDepartmentMapped.Remove(subDeptVar);
            db.SaveChanges();           
        }


        public List<Subject> SubjectOutOfDept(int depertmentId)
        {
            var subjects = subjectService.getAllSubject();
            var subjectOftheDept = GetSubjectByDepertmentId(depertmentId);
            var subjectOutOftheDept = subjects.Except(subjectOftheDept).ToList();
            
            return subjectOutOftheDept;
        }

        
    }
}
