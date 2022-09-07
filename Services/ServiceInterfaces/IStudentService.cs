using University_Information_System.Migrations;
using University_Information_System.Models;

namespace University_Information_System.Services.ServiceInterfaces
{
    public interface IStudentService
    {
        public List<Student> getAllStudent();
        public void AddStudent(Student student);
 
        public void UpdateStudent(Student student);
        public void DeleteStudent(Student student);

        public Student GetStudentById(int id);
        public Student GetStudentDetailsById(int id);
        public List<Subject> GetSubjectByStudentId(int id);
        public void AddSubjectStudentMapped(SubjectStudentMapped subjectStudentMapped);
        public void DeleteEnrolmentFromSubjectStudentMapped(int subjectId, int studentId);
    }
}
