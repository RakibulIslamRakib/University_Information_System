using University_Information_System.Models;

namespace University_Information_System.Services.ServiceInterfaces
{
    public interface ITeacherService 
    {
        public IQueryable<Teacher> getAllTeacher();
        public void AddTeacher(Teacher teacher);
        public Teacher GetTeacherById(int id);
        public void UpdateTeacher(Teacher teacher);
        public void DeleteTeacher(Teacher teacher);
        public Teacher GetTeacherDetailsById(int id);
        public List<Subject> GetSubjectByTeacherId(int id);
        public void AddSubjectTeacherMapped(SubjectTeacherMapped subjectTeacherMapped);
        public void DeleteSubTeacherMapped(int subjectId, int teacherId);
    }
}
