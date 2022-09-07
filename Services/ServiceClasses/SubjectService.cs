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
            var studentIdOfTheSubject = db.SubjectStudentMapped.Where(ss => ss.subjectId == id).Select(ss=>ss.studentId);
            var allStudentOfTheSubject = db.Student.Where(student => studentIdOfTheSubject.Contains(student.id)).ToList();

            return allStudentOfTheSubject;
        }

        public List<Teacher> GetTeacherBySubjectId(int id)
        {
            var teacherIdOfTheSubject = db.SubjectTeacherMapped.Where(st => st.SubjectId == id).Select(st => st.TeacherId);
            var allTeacherOfTheSubject = db.Teacher.Where(teacher => teacherIdOfTheSubject.Contains(teacher.Id)).ToList();

            return allTeacherOfTheSubject;
        }


        public List<Depertment> GetDepertmentBySubjectId(int id)
        {
            var depertmentIdOfTheSubject = db.SubjectDepartmentMapped.Where(sd => sd.subjectId == id).Select(sd => sd.departmentId);
            var allDepertmentOfTheSubject = db.Depertment.Where(dept => depertmentIdOfTheSubject.Contains(dept.id)).ToList();

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
