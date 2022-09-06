using University_Information_System.Data;
using University_Information_System.Models;
using University_Information_System.Services.ServiceInterfaces;

namespace University_Information_System.Services.ServiceClasses
{
    public class SubjectService: CommonUtilityClass,ISubjectService
    {

        public SubjectService(ApplicationDbContext db):base(db) {}


        public void AddSubject(Subject subject)
        {
            db.Subject.Add(subject);
            db.SaveChanges();
        }

        public void DeleteSubject(Subject subject)
        {
            RemoveAllEnrolmentOfTheSubject(subject.id);
            RemoveAllSubTeacherOfTheSubject(subject.id);
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
            var subjectStudent = db.SubjectStudentMapped.ToList();
            var studentIdOfTheSubject = from subStudent in subjectStudent
                                        where subStudent.subjectId == id
                                        orderby subStudent.studentId
                                        select subStudent.studentId;
            List<Student> allStudentList = db.Student.ToList();

            var allStudentOfTheSubject = new List<Student>();
            int index = 0;
            foreach (var student in allStudentList)
            {
                if (studentIdOfTheSubject.Count() <= index) break;
                if (student.id == studentIdOfTheSubject.ElementAt(index))
                {
                    allStudentOfTheSubject.Add(student);
                    index++;
                }
            }


            return allStudentOfTheSubject;
        }
        public List<Teacher> GetTeacherBySubjectId(int id)
        {
            var subjectTeacher = db.SubjectTeacherMapped.ToList();
            var teacherIdOfTheSubject = from subTeacher in subjectTeacher
                                        where subTeacher.SubjectId == id
                                        orderby subTeacher.TeacherId
                                        select subTeacher.TeacherId;
            List<Teacher> allTeacherList = db.Teacher.ToList();

            var allTeacherOfTheSubject = new List<Teacher>();
            int index = 0;
            foreach (var teacher in allTeacherList)
            {
                if (teacherIdOfTheSubject.Count() <= index) break;
                if (teacher.Id == teacherIdOfTheSubject.ElementAt(index))
                {
                    allTeacherOfTheSubject.Add(teacher);
                    index++;
                }
            }


            return allTeacherOfTheSubject;
        }

        public Subject GetSubjectDetailsById(int id)
        {
            var subject = db.Subject.Find(id);
            subject.Depertments = GetDepertmentBySubjectId(id);
            subject.Teachers = GetTeacherBySubjectId(id);
            subject.Students = GetStudentBySubjectId(id);

            return subject;
        }

        public void DeleteSubjectFromSubjectTeacherMapped(int subjectId, int teacherId)
        {
            var subjectTeacherList = db.SubjectTeacherMapped.ToList();
            var subTeacherVar = from sT in (from subTeacher in subjectTeacherList
                                            where subTeacher.SubjectId == subjectId
                                            select subTeacher)
                                where sT.TeacherId == teacherId
                                select sT;
            if (subTeacherVar.Count() > 0)
            {
                db.SubjectTeacherMapped.Remove(subTeacherVar.ElementAt(0));
                db.SaveChanges();
            }
        }

        public void RemoveAllEnrolmentOfTheSubject(int subjectId)
        {
            var subStudent = db.SubjectStudentMapped;
            var enrolmentOfTheSubject = from ss in subStudent where ss.subjectId == subjectId select ss;
            db.SubjectStudentMapped.RemoveRange(enrolmentOfTheSubject);
            db.SaveChanges();
        }


        public void RemoveAllSubTeacherOfTheSubject(int subjectId)
        {
            var subTeacher = db.SubjectTeacherMapped;
            var subTeacherOfTheSubject = from st in subTeacher where st.SubjectId == subjectId select st;
            db.SubjectTeacherMapped.RemoveRange(subTeacher);
            db.SaveChanges();
        }

    }
}
