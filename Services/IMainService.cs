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
        public Subject GetSubjectById(int id);
        public void UpdateSubject(Subject subject);
        public void DeleteSubject(Subject subject);

        public List<Teacher> getAllTeacher();
        public void AddTeacher(Teacher teacher);
        public Teacher GetTeacherById(int id);
        public void UpdateTeacher(Teacher teacher);
        public void DeleteTeacher(Teacher teacher);

        public List<Student> getAllStudent();
        public void AddStudent(Student student);
        public Student GetStudentById(int id);
        public void UpdateStudent(Student student);
        public void DeleteStudent(Student student);

        public List<Subject> GetSubjectByStudentId(int id);


        public Depertment GetDepertmentDetailsById(int id);
        public Subject GetSubjectDetailsById(int id);
        public Teacher GetTeacherDetailsById(int id);
        public Student GetStudentDetailsById(int id);
        public List<Subject> GetSubjectByDepertmentId(int id);
        public List<Subject> GetSubjectByTeacherId(int id);

        public List<Student> GetStudentByDepertmentId(int id);


        public void AddSubjectDapertmentMapped(SubjectDepartmentMapped subjectDapertmentMapped);
        public void AddSubjectStudentMapped(SubjectStudentMapped subjectStudentMapped);
        public void DeleteSubjectFromSubjectDepartmentMapped(int subjectId, int deptId);
        public void AddSubjectTeacherMapped(SubjectTeacherMapped subjectTeacherMapped);

        public void DeleteSubjectFromSubjectTeacherMapped(int subjectId, int teacherId);
        public void DeleteEnrolmentFromSubjectStudentMapped(int subjectId, int studentId);
    }
}
