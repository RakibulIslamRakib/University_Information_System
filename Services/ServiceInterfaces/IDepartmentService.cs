using University_Information_System.Data;
using University_Information_System.Models;

namespace University_Information_System.Services.ServiceInterfaces
{
    public interface IDepartmentService 
    {
        public void AddDepertment(Depertment depertment);
        public IQueryable<Depertment> getAllDepertment();
        public Depertment GetDepertmentById(int id);
        public void UpdateDepertment(Depertment depertment);
        public void DeleteDepertment(Depertment depertment);

        public Depertment GetDepertmentDetailsById(int id);

        public List<Student> GetStudentByDepertmentId(int id);
        public List<Subject> GetSubjectByDepertmentId(int id);
        public List<Subject> SubjectOutOfDept(int depertmentId);


        public void AddSubjectDapertmentMapped(SubjectDepartmentMapped subjectDapertmentMapped);
        public void DeleteSubjectFromSubjectDepartmentMapped(int subjectId, int deptId);
        public void DeleteStudentFromDept(int studentId);
    }
}
