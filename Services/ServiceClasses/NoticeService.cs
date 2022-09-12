
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

        public void AddNotice(Notice notice)
        {
            db.Notice.Add(notice);
            db.SaveChanges();
        }

        public IQueryable<Notice> getAllNotice()
        {
            return db.Notice; 
        }

        public void UpdateNotice(Notice notice)
        {
            db.Notice.Update(notice);
            db.SaveChanges();
        }

        public Notice GetNoticeById(int id)
        {

            return db.Notice.Find(id);
        }

        public void DeleteNotice(Notice notice)
        {
            db.Notice.Remove(notice);
            db.SaveChanges();
        }



        public Notice GetNoticeDetailsById(int id)
        {
            var notice = db.Notice.Find(id);


            return notice;
        }

    }
}
