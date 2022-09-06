using University_Information_System.Data;
using University_Information_System.Migrations;
using University_Information_System.Models;
using University_Information_System.Services.ServiceInterfaces;

namespace University_Information_System.Services.ServiceClasses
{
    public class StudentService: CommonUtilityClass, IStudentService
    {
        public StudentService(ApplicationDbContext db) : base(db){    }

        public List<Student> getAllStudent()
        {
            var students = db.Student.ToList();

            return students;
        }

        public void UpdateStudent(Student student)
        {
            db.Student.Update(student);
            db.SaveChanges();
        }

        public Student GetStudentDetailsById(int id)
        {
            var student = db.Student.Find(id);
            student.DeptName = GetDepertmentById(student.DepertmentId).DeptName;
            student.Subjects = GetSubjectByStudentId(id);

            return student;
        }


        public void AddStudent(Student student)
        {
            db.Student.Add(student);
            db.SaveChanges();
        }



        public void AddSubjectStudentMapped(SubjectStudentMapped subjectStudentMapped)
        {
            db.SubjectStudentMapped.Add(subjectStudentMapped);
            db.SaveChanges();
        }

        public void DeleteEnrolmentFromSubjectStudentMapped(int subjectId, int studentId)
        {
            var enrolmentList = db.SubjectStudentMapped.ToList();
            var subStudentVar = from ss in (from subStudent in enrolmentList
                                            where subStudent.subjectId == subjectId
                                            select subStudent)
                                where ss.studentId == studentId
                                select ss;
            if (subStudentVar.Count() > 0)
            {
                db.SubjectStudentMapped.Remove(subStudentVar.ElementAt(0));
                db.SaveChanges();
            }
        }



        public List<Subject> GetSubjectByStudentId(int id)
        {
            var subjectStudent = db.SubjectStudentMapped.ToList();
            var subjectIdOfTheStudent = from subStudent in subjectStudent
                                        where subStudent.studentId == id
                                        orderby subStudent.subjectId
                                        select subStudent.subjectId;
            var deptId = db.Student.Find(id).DepertmentId;

            List<Subject> allSubjectListOfTheDept = GetSubjectByDepertmentId(deptId);

            var allSubjectOfTheStudent = new List<Subject>();
            int index = 0;
            foreach (var subject in allSubjectListOfTheDept)
            {
                if (subjectIdOfTheStudent.Count() <= index) break;
                if (subject.id == subjectIdOfTheStudent.ElementAt(index))
                {
                    allSubjectOfTheStudent.Add(subject);
                    index++;
                }
            }


            return allSubjectOfTheStudent;
        }

    }
}
