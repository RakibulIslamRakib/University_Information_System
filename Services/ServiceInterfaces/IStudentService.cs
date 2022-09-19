
using University_Information_System.Models;

namespace University_Information_System.Services.ServiceInterfaces
{
    public interface IStudentService
    {
        public Task<List<Student>> getAllStudent();
        public Task AddStudent(Student student);
 
        public Task UpdateStudent(Student student);
        public Task DeleteStudent(Student student);

        public Task<Student> GetStudentById(int id);
        public Task<Student> GetStudentDetailsById(int id);
        public Task<List<Subject>> GetSubjectByStudentId(int id);
        public Task AddSubjectStudentMapped(SubjectStudentMapped subjectStudentMapped);
        public Task DeleteEnrolmentFromSubjectStudentMapped(int subjectId, int studentId);
    }
}
