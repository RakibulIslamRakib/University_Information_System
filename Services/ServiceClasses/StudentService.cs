using University_Information_System.Data;
using University_Information_System.Migrations;
using University_Information_System.Models;
using University_Information_System.Services.ServiceInterfaces;

namespace University_Information_System.Services.ServiceClasses
{
    public class StudentService:  IStudentService
    {

        public readonly ApplicationDbContext db;
        private readonly IDepartmentService departmentService;

        public StudentService(ApplicationDbContext db, IDepartmentService departmentService)
        {
            this.db = db;
            this.departmentService = departmentService;
        }

        public void AddStudent(Student student)
        {
            db.Student.Add(student);
            db.SaveChanges();
        }

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

        public Student GetStudentById(int id)
        {

            return db.Student.Find(id);
        }

        public void DeleteStudent(Student student)
        {
            var subStudentVar = db.SubjectStudentMapped.Where(ss => ss.studentId == student.id );
            
            if (subStudentVar.Count() > 0)
            {
                db.SubjectStudentMapped.RemoveRange(subStudentVar);
                db.SaveChanges();
            }

            db.Student.Remove(student);
            db.SaveChanges();
        }



        public Student GetStudentDetailsById(int id)
        {
            var student = db.Student.Find(id);
            student.DeptName = departmentService.GetDepertmentById(student.DepertmentId).DeptName;
            student.Subjects = GetSubjectByStudentId(id);

            return student;
        }


 



        public void AddSubjectStudentMapped(SubjectStudentMapped subjectStudentMapped)
        {
            db.SubjectStudentMapped.Add(subjectStudentMapped);
            db.SaveChanges();
        }

        public void DeleteEnrolmentFromSubjectStudentMapped(int subjectId, int studentId)
        {
            var subStudentVar = db.SubjectStudentMapped.Where(ss=>ss.studentId == studentId && ss.subjectId==subjectId);
            if (subStudentVar.Count() > 0)
            {
                db.SubjectStudentMapped.Remove(subStudentVar.ElementAt(0));
                db.SaveChanges();
            }
        }



        public List<Subject> GetSubjectByStudentId(int id)
        {
            var subjectIdOfTheStudent = db.SubjectStudentMapped.Where(ss => ss.studentId == id).Select(ss=>ss.subjectId).ToList();
            var deptId = db.Student.Find(id).DepertmentId;
            var allSubjectListOfTheDept = departmentService.GetSubjectByDepertmentId(deptId);
            var allSubjectOfTheStudent = new List<Subject>();

            foreach (var subjectId in subjectIdOfTheStudent)
            {
                allSubjectOfTheStudent.Add(allSubjectListOfTheDept.FirstOrDefault(x => x.id == subjectId));
                
            }

            return allSubjectOfTheStudent;
        }

    }
}
