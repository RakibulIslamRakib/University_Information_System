using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
using University_Information_System.Data;
using University_Information_System.Models;
using University_Information_System.Services.ServiceInterfaces;

namespace University_Information_System.Services.ServiceClasses
{
    public class TeacherService: ITeacherService
    {

        public readonly ApplicationDbContext db;

        private readonly ISubjectService subjectService;
        private readonly IAccountService accountService;
        public TeacherService(ApplicationDbContext db, 
            IAccountService accountService, ISubjectService subjectService)
        {
            this.db = db;
            this.subjectService = subjectService;
            this.accountService = accountService;
        }



        public async Task AddTeacher(ApplicationUser user)
        {
            await accountService.AddRole(user,"Teacher");
            await db.SaveChangesAsync();
        }

        public async Task DeleteSubTeacherMapped(int subjectId, string teacherId)
        {
            var subTeacher = await db.SubjectTeacherMapped.FirstOrDefaultAsync(
                st => st.TeacherId == teacherId && st.SubjectId == subjectId);

            if (subTeacher != null)
            {
                db.SubjectTeacherMapped.Remove(subTeacher);
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteTeacher(ApplicationUser teacher)
        {
            var subTeacherOfTheTeacher = await db.SubjectTeacherMapped.Where(
                st => st.TeacherId == teacher.Id).ToListAsync();
            db.SubjectTeacherMapped.RemoveRange(subTeacherOfTheTeacher);
            await accountService.RemoveRole(teacher, "Teacher");
            await db.SaveChangesAsync();

        }



        public async Task<List<ApplicationUser>> getAllTeacher()
        {
            var result = await accountService.GetUsersInRole("Teacher");
            return result.ToList();
        }



        public async Task<ApplicationUser> GetTeacherById(string id)
        {
            var result = await accountService.GetUserById(id);
            return result;
        }





        public async Task<List<Subject>> GetSubjectByTeacherId(string id)
        {
            var subjectIdOfTheTeacher = await db.SubjectTeacherMapped.Where(
                st => st.TeacherId == id).Select(st=>st.SubjectId).ToListAsync();
            var allSubjectOfTheTeacher = await db.Subject.Where(
                sb=> subjectIdOfTheTeacher.Contains(sb.id)).ToListAsync();

            return allSubjectOfTheTeacher;
        }

       

        public async Task AddSubjectTeacherMapped(SubjectTeacherMapped subjectTeacherMapped)
        {
            await db.SubjectTeacherMapped.AddAsync(subjectTeacherMapped);
            await db.SaveChangesAsync();
        }


        public async Task<ApplicationUser> GetTeacherDetailsById(string id)
        {
            var teacher = await GetTeacherById(id);
            if (teacher == null) return teacher;
            teacher.Subjects = await GetSubjectByTeacherId(id);

            teacher.Depertments = new List<Depertment>();
          
            foreach (var subject in teacher.Subjects)
            {
                teacher.Depertments.AddRange(await 
                    subjectService.GetDepertmentBySubjectId(subject.id));
            }
            var depertmentSet = new HashSet<Depertment>(teacher.Depertments);
            teacher.Depertments = new List<Depertment>(depertmentSet);


            teacher.Students = new List<ApplicationUser>();

            foreach (var subject in teacher.Subjects)
            {
                teacher.Students.AddRange(await
                    subjectService.GetStudentBySubjectId(subject.id));
            }
            var studenttSet = new HashSet<ApplicationUser>(teacher.Students);
            teacher.Students = new List<ApplicationUser>(studenttSet);

            return teacher;
        }

    }
}
