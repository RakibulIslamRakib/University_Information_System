using University_Information_System.Models;

namespace University_Information_System.Services.ServiceInterfaces
{
    public interface ISubjectService: IMainCommonService
    {
        public void AddSubject(Subject subject);
        public Subject GetSubjectById(int id);
        public void UpdateSubject(Subject subject);
        public void DeleteSubject(Subject subject);
        public Subject GetSubjectDetailsById(int id);
    }
}
