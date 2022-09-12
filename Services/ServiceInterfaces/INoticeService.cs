
using University_Information_System.Models;

namespace University_Information_System.Services.ServiceInterfaces
{
    public interface INoticeService
    {
        public IQueryable<Notice> getAllNotice();
        public void AddNotice(Notice notice);
 
        public void UpdateNotice(Notice notice);
        public void DeleteNotice(Notice notice);

        public Notice GetNoticeById(int id);
        public  Notice GetNoticeDetailsById(int id);
    }
}
