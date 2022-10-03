using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.Arm;
using University_Information_System.Data;
using University_Information_System.Models;
using University_Information_System.Services.ServiceInterfaces;

namespace University_Information_System.Services.ServiceClasses
{
    public class StudentService : IStudentService
    {

        public readonly ApplicationDbContext db;
        private readonly IAccountService accountService;

        public StudentService(ApplicationDbContext db, IAccountService accountService)
        {
            this.db = db;
            this.accountService = accountService;
        }

        public async Task AddStudent(string userId, int deptId)
        {
            var user = await accountService.GetUserById(userId);
            await accountService.AddRole(user, "Student");
            await AddStudentDeptMapped(userId, deptId);
            await db.SaveChangesAsync();
        }

        public async Task AddStudentDeptMapped(string userId, int deptId)
        {
            var studentDept = new StudentDepertment
            {
                studentId = userId,
                departmentId = deptId
            };
            await db.StudentDepertment.AddAsync(studentDept);
            await db.SaveChangesAsync();    
        }

        public async Task<List<ApplicationUser>> GetAllStudent()
        {
            var result = await accountService.GetUsersInRole("Student");
            return result.ToList();
        }

        public async Task UpdateStudent(ApplicationUser student)
        {
            await accountService.UpdateUser(student);
            await db.SaveChangesAsync();
        }

        public async Task<ApplicationUser> GetStudentById(string id)
        {
            var result = await accountService.GetUserById(id);
            return result;
        }

        public async Task DeleteStudent(ApplicationUser student)
        {
            var subStudentVar = await db.SubjectStudentMapped.Where(
                ss => ss.studentId == student.Id).ToListAsync();
            
            db.SubjectStudentMapped.RemoveRange(subStudentVar);
            var deptStudent = await db.StudentDepertment.FirstOrDefaultAsync(
                sd => sd.studentId == student.Id);
            if (deptStudent != null)
            {
                db.StudentDepertment.RemoveRange(deptStudent);
            }
             var result = await accountService.RemoveRole(student, "Student");
            await db.SaveChangesAsync();
        }



        public async Task<ApplicationUser> GetStudentDetailsById(string id)
        {
            var student = await accountService.GetUserById(id);
            if(student == null)return new ApplicationUser();
            var dept = await GetDeptByStudentId(id);
            var depts = new List<Depertment>
            {
                dept
            };

            student.Depertments = depts;
            student.Subjects = await GetSubjectByStudentId(id);

            return student;
        }


 



        public async Task AddSubjectStudentMapped(SubjectStudentMapped subjectStudentMapped)
        {
            await db.SubjectStudentMapped.AddAsync(subjectStudentMapped);
            await db.SaveChangesAsync();
        }

        public async Task DeleteEnrolmentFromSubjectStudentMapped(
            int subjectId, string studentId)
        {
            var subStudentVar = await db.SubjectStudentMapped.FirstOrDefaultAsync(
                ss=>ss.studentId == studentId && ss.subjectId==subjectId);
            if (subStudentVar != null)
            {
                db.SubjectStudentMapped.Remove(subStudentVar);
                await db.SaveChangesAsync();
            }
        }



        public async Task<List<Subject>> GetSubjectByStudentId(string id)
        {
            var subjectIdOfTheStudent = await db.SubjectStudentMapped.Where(
                ss => ss.studentId == id).Select(ss => ss.subjectId).ToListAsync();
            var allSubjectOfTheStudent = await db.Subject.Where(
                subject => subjectIdOfTheStudent.Contains(subject.id)).ToListAsync();

            return allSubjectOfTheStudent;
        }

        public async Task<Depertment> GetDeptByStudentId(string id)
        {
            var deptStudent = await  db.StudentDepertment.FirstOrDefaultAsync(
                sd => sd.studentId == id);
 
            var deptId = deptStudent?.departmentId;
            var dept = await db.Depertment.FindAsync(deptId);


            return dept;
        }

    }
}
