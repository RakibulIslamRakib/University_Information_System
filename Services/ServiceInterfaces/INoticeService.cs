
using University_Information_System.Models;

namespace University_Information_System.Services.ServiceInterfaces
{
    public interface INoticeService
    {
        public Task<List<Notice>> GetAllNotice();
        public Task AddNotice(Notice notice);
 
        public Task UpdateNotice(Notice notice);
        public Task DeleteNotice(Notice notice);

        public Task<Notice> GetNoticeById(int id);
        public Task<Notice> GetNoticeDetailsById(int id);
    }
}
