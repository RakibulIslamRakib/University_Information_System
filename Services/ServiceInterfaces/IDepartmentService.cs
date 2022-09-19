using University_Information_System.Data;
using University_Information_System.Models;

namespace University_Information_System.Services.ServiceInterfaces
{
    public interface IDepartmentService 
    {
        public Task AddDepertment(Depertment depertment);
        public Task<List<Depertment>> GetAllDepertment();
        public Task<Depertment> GetDepertmentById(int id);
        public Task UpdateDepertment(Depertment depertment);
        public Task DeleteDepertment(Depertment depertment);

        public Task<Depertment> GetDepertmentDetailsById(int id);

        public Task<List<Student>> GetStudentByDepertmentId(int id);
        public Task<List<Subject>> GetSubjectByDepertmentId(int id);
        public Task<List<Subject>> SubjectOutOfDept(int depertmentId);
        public Task<List<Teacher>> GetTeacherByDepertmentId(int id);

        public Task AddSubjectDapertmentMapped(SubjectDepartmentMapped subjectDapertmentMapped);
        public Task DeleteSubjectFromSubjectDepartmentMapped(int subjectId, int deptId);
        public Task DeleteStudentFromDept(int studentId);
    }
}
