using University_Information_System.Models;

namespace University_Information_System.Services
{
    public interface IMainService
    {

        public List<Depertment> getAllDepertment();
        public void AddDepertment(Depertment depertment);
        public Depertment GetDepertmentById(int id);
        public void UpdateDepertment(Depertment depertment);
        public void DeleteDepertment(Depertment depertment);


        public List<Subject> getAllSubject();
        public void AddSubject(Subject subject);
        public Depertment GetSubjectById(int id);
        public void UpdateSubject(Subject subject);
        public void DeleteSubject(Subject subject);

        public List<Teacher> getAllTeacher();
        public void AddTeacher(Teacher teacher);
        public Depertment GetTeacherById(int id);
        public void UpdateTeacher(Teacher teacher);
        public void DeleteTeacher(Teacher teacher);

        public List<Student> getAllStudent();
        public void AddStudent(Student student);
        public Depertment GetStudentById(int id);
        public void UpdateStudent(Student student);
        public void DeleteStudent(Student student);

    }
}
