using University_Information_System.Data;
using University_Information_System.Models;
using University_Information_System.Services.ServiceInterfaces;

namespace University_Information_System.Services.ServiceClasses
{
    public class SubjectService: ISubjectService
    {
        public readonly ApplicationDbContext db;

        public SubjectService(ApplicationDbContext db) {
            this.db = db;
        }


        public void AddSubject(Subject subject)
        {
            db.Subject.Add(subject);
            db.SaveChanges();
        }


        public List<Subject> getAllSubject()
        {
            var subjects = db.Subject.ToList();

            return subjects;
        }

        public void DeleteSubject(Subject subject)
        {
            var enrolmentOfTheSubject = db.SubjectStudentMapped.Where(ss=>ss.subjectId==subject.id);
            db.SubjectStudentMapped.RemoveRange(enrolmentOfTheSubject);
            db.SaveChanges();

            var subTeacherOfTheSubject = db.SubjectTeacherMapped.Where(st => st.SubjectId == subject.id);
            db.SubjectTeacherMapped.RemoveRange(subTeacherOfTheSubject);
            db.SaveChanges();

            db.Subject.Remove(subject);
            db.SaveChanges();
        }


        public Subject GetSubjectById(int id)
        {
            return db.Subject.Find(id);
        }

        public void UpdateSubject(Subject subject)
        {
            db.Subject.Update(subject);
            db.SaveChanges();
        }

        public List<Student> GetStudentBySubjectId(int id)
        {
            var subStudentOfTheSubject = db.SubjectStudentMapped.Where(ss => ss.subjectId == id);
            var allStudentOfTheSubject = new List<Student>();

            int index = 0;
            foreach (var substudent in subStudentOfTheSubject)
            {
               allStudentOfTheSubject.Add(db.Student.FirstOrDefault(st=>st.id == substudent.studentId));
                  
            }

            return allStudentOfTheSubject;
        }

        public List<Teacher> GetTeacherBySubjectId(int id)
        {
            var subteacherOfTheSubject = db.SubjectTeacherMapped.Where(st => st.SubjectId == id);
            var allTeacherOfTheSubject = new List<Teacher>();

            foreach (var subTeacher in  subteacherOfTheSubject)
            {
                allTeacherOfTheSubject.Add(db.Teacher.FirstOrDefault(teacher => teacher.Id == subTeacher.TeacherId));
            }

            return allTeacherOfTheSubject;
        }

        public List<Depertment> GetDepertmentBySubjectId(int id)
        {
            var depertmentOfTheSubject = db.SubjectDepartmentMapped.Where(sd => sd.subjectId == id);           
            var allDepertmentOfTheSubject = new List<Depertment>();

            foreach (var subdept in depertmentOfTheSubject)
            {             
                allDepertmentOfTheSubject.Add(db.Depertment.FirstOrDefault(dept=>dept.id==subdept.departmentId));                
            }

            return allDepertmentOfTheSubject;
        }


        public Subject GetSubjectDetailsById(int id)
        {
            var subject = db.Subject.Find(id);
            subject.Depertments = GetDepertmentBySubjectId(id);
            subject.Teachers = GetTeacherBySubjectId(id);
            subject.Students = GetStudentBySubjectId(id);

            return subject;
        }

    }
}
