using University_Information_System.Models;

namespace University_Information_System.Services
{
    public interface IMainCommonService
    {
        public List<Subject> getAllSubject();
        public List<Depertment> getAllDepertment();
        public Depertment GetDepertmentById(int id);
        public void DeleteStudent(Student student);
        public Student GetStudentById(int id);
        public List<Subject> GetSubjectByDepertmentId(int id);
        public void AddSubjectTeacherMapped(SubjectTeacherMapped subjectTeacherMapped);
    }
}
