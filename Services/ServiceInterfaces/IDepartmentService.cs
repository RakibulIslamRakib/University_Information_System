using University_Information_System.Data;
using University_Information_System.Models;

namespace University_Information_System.Services.ServiceInterfaces
{
    public interface IDepartmentService: IMainCommonService
    {
        public void AddDepertment(Depertment depertment);
        public void UpdateDepertment(Depertment depertment);
        public void DeleteDepertment(Depertment depertment);
        public Depertment GetDepertmentDetailsById(int id);
        public List<Student> GetStudentByDepertmentId(int id);

        public void AddSubjectDapertmentMapped(SubjectDepartmentMapped subjectDapertmentMapped);

        public void DeleteSubjectFromSubjectDepartmentMapped(int subjectId, int deptId);
    }
}
