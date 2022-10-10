
using Microsoft.EntityFrameworkCore;
using University_Information_System.Data;
using University_Information_System.Models;
using University_Information_System.Services.ServiceInterfaces;

namespace University_Information_System.Services.ServiceClasses
{
    public class NoticeService : INoticeService
    {

        public readonly ApplicationDbContext db;

        public NoticeService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task AddNotice(Notice notice)
        {
            await db.Notice.AddAsync(notice);
            await db.SaveChangesAsync();
        }

        public async Task<List<Notice>> GetAllNotice()
        {
            return await db.Notice.ToListAsync(); 
        }

        public async Task UpdateNotice(Notice notice)
        {
           db.Notice.Update(notice);
           await db.SaveChangesAsync();
        }

        public async Task<Notice> GetNoticeById(int id)
        {        
            return await db.Notice.FindAsync(id);         
        }


        public async Task DeleteNotice(Notice notice)
        {
           db.Notice.Remove(notice);
           await db.SaveChangesAsync();
        }



        public async Task<Notice> GetNoticeDetailsById(int id)
        {
            var notice = await db.Notice.FindAsync(id);
            return notice;
        }

    }
}
