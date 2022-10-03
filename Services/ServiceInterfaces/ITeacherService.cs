using University_Information_System.Models;

namespace University_Information_System.Services.ServiceInterfaces
{
    public interface ITeacherService 
    {
        public Task<List<ApplicationUser>> getAllTeacher();
        public Task AddTeacher(ApplicationUser teacher);
        public Task<ApplicationUser> GetTeacherById(string id);
        public Task DeleteTeacher(ApplicationUser teacher);
        public Task <ApplicationUser> GetTeacherDetailsById(string id);
        public Task <List<Subject>> GetSubjectByTeacherId(string id);
        public Task AddSubjectTeacherMapped(SubjectTeacherMapped subjectTeacherMapped);
        public Task DeleteSubTeacherMapped(int subjectId, string teacherId);
    }
}
