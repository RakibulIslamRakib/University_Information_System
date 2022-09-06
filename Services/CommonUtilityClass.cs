using University_Information_System.Data;
using University_Information_System.Migrations;
using University_Information_System.Models;

namespace University_Information_System.Services
{
    public class CommonUtilityClass
    {
        public readonly ApplicationDbContext db;

        public CommonUtilityClass(ApplicationDbContext db)
        {
            this.db = db;

        }


        public List<Subject> getAllSubject()
        {
            var subjects = db.Subject.ToList();

            return subjects;
        }

        public List<Depertment> getAllDepertment()
        {
            var depertments = db.Depertment.ToList();

            return depertments;
        }


        public List<Depertment> GetDepertmentBySubjectId(int id)
        {
            var subjectDepertment = db.SubjectDepartmentMapped.ToList();
            var depertmentIdOfTheSubject = from subDept in subjectDepertment
                                           where subDept.subjectId == id
                                           orderby subDept.departmentId
                                           select subDept.departmentId;
            List<Depertment> allDepartmentList = db.Depertment.ToList();

            var allDepertmentOfTheSubject = new List<Depertment>();
            int index = 0;
            foreach (var depertment in allDepartmentList)
            {
                if (depertmentIdOfTheSubject.Count() <= index) break;
                if (depertment.id == depertmentIdOfTheSubject.ElementAt(index))
                {
                    allDepertmentOfTheSubject.Add(depertment);
                    index++;
                }
            }


            return allDepertmentOfTheSubject;
        }


        public Depertment GetDepertmentById(int id)
        {

            return db.Depertment.Find(id);
        }

        public Student GetStudentById(int id)
        {

            return db.Student.Find(id);
        }

        public void DeleteStudent(Student student)
        {
            db.Student.Remove(student);
            db.SaveChanges();
        }

        public List<Subject> GetSubjectByDepertmentId(int id)
        {
            var subjectDepertment = db.SubjectDepartmentMapped.ToList();
            var subjectIdOfTheDepertment = from subDept in subjectDepertment
                                           where subDept.departmentId == id
                                           orderby subDept.subjectId
                                           select subDept.subjectId;
            List<Subject> allSubjectList = db.Subject.ToList();

            var allSubjectOfTheDepertment = new List<Subject>();
            int index = 0;
            foreach (var subject in allSubjectList)
            {
                if (subjectIdOfTheDepertment.Count() <= index) break;

                if (subject.id == subjectIdOfTheDepertment.ElementAt(index))
                {
                    allSubjectOfTheDepertment.Add(subject);
                    index++;
                }
            }


            return allSubjectOfTheDepertment;
        }


        public void AddSubjectTeacherMapped(SubjectTeacherMapped subjectTeacherMapped)
        {
            db.SubjectTeacherMapped.Add(subjectTeacherMapped);
            db.SaveChanges();
        }
    }
}
