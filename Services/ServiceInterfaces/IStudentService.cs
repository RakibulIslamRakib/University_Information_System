
using University_Information_System.Models;

namespace University_Information_System.Services.ServiceInterfaces
{
    public interface IStudentService
    {
        public Task<List<ApplicationUser>> GetAllStudent();
        public Task AddStudent(string userId, int deptId);
 
        public Task UpdateStudent(ApplicationUser student);
        public Task DeleteStudent(ApplicationUser student);

        public Task<ApplicationUser> GetStudentById(string id);
        public Task<ApplicationUser> GetStudentDetailsById(string id);
        public Task<List<Subject>> GetSubjectByStudentId(string id);
        public Task AddSubjectStudentMapped(SubjectStudentMapped subjectStudentMapped);
        public Task DeleteEnrolmentFromSubjectStudentMapped(int subjectId, string studentId);
        Task<Depertment> GetDeptByStudentId(string id);
    }
}
