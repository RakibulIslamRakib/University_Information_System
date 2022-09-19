using University_Information_System.Models;

namespace University_Information_System.Services.ServiceInterfaces
{
    public interface ISubjectService
    {
        public Task AddSubject(Subject subject);
        public Task<List<Subject>> getAllSubject();
        public Task<Subject> GetSubjectById(int id);
        public Task UpdateSubject(Subject subject);
        public Task DeleteSubject(Subject subject);
        public Task<Subject> GetSubjectDetailsById(int id);
        public Task<List<Teacher>> GetTeacherBySubjectId(int id);
        public Task<List<Depertment>> GetDepertmentBySubjectId(int id);
        public Task<List<Student>> GetStudentBySubjectId(int id);
    }
}
