using University_Information_System.Models;

namespace University_Information_System.Services.ServiceInterfaces
{
    public interface ITeacherService 
    {
        public Task<List<Teacher>> getAllTeacher();
        public Task AddTeacher(Teacher teacher);
        public Task<Teacher> GetTeacherById(int id);
        public Task UpdateTeacher(Teacher teacher);
        public Task DeleteTeacher(Teacher teacher);
        public Task <Teacher> GetTeacherDetailsById(int id);
        public Task <List<Subject>> GetSubjectByTeacherId(int id);
        public Task AddSubjectTeacherMapped(SubjectTeacherMapped subjectTeacherMapped);
        public Task DeleteSubTeacherMapped(int subjectId, int teacherId);
    }
}
