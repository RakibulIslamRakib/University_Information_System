using Microsoft.EntityFrameworkCore;
using University_Information_System.Data;
using University_Information_System.Models;
using University_Information_System.Services.ServiceInterfaces;

namespace University_Information_System.Services.ServiceClasses
{
    public class SubjectService: ISubjectService
    {
        public readonly ApplicationDbContext db;

        public SubjectService(ApplicationDbContext db) {
            this.db = db;
        }


        public async Task AddSubject(Subject subject)
        {
            await db.Subject.AddAsync(subject);
            await db.SaveChangesAsync();
        }


        public async Task<List<Subject>> getAllSubject()
        {
            return await db.Subject.ToListAsync();

        }

        public async Task DeleteSubject(Subject subject)
        {
            var enrolmentOfTheSubject = await db.SubjectStudentMapped.Where(ss=>ss.subjectId==subject.id).ToListAsync();
            db.SubjectStudentMapped.RemoveRange(enrolmentOfTheSubject);
            await db.SaveChangesAsync();

            var subTeacherOfTheSubject = await db.SubjectTeacherMapped.Where(st => st.SubjectId == subject.id).ToListAsync();
            db.SubjectTeacherMapped.RemoveRange(subTeacherOfTheSubject);
            await db.SaveChangesAsync();

            db.Subject.Remove(subject);
            await db.SaveChangesAsync();
        }


        public async Task<Subject> GetSubjectById(int id)
        {
            var subject = await db.Subject.FindAsync(id);
            return subject;
        }

        public async Task UpdateSubject(Subject subject)
        {
            db.Subject.Update(subject);
            await db.SaveChangesAsync();
        }

        public async Task <List<Student>> GetStudentBySubjectId(int id)
        {
            var studentIdOfTheSubject = await db.SubjectStudentMapped.Where(ss => ss.subjectId == id).Select(ss=>ss.studentId).ToListAsync();
            var allStudentOfTheSubject = await db.Student.Where(student => studentIdOfTheSubject.Contains(student.id)).ToListAsync();

            return allStudentOfTheSubject;
        }

        public async Task<List<Teacher>> GetTeacherBySubjectId(int id)
        {
            var teacherIdOfTheSubject = await db.SubjectTeacherMapped.Where(st => st.SubjectId == id).Select(st => st.TeacherId).ToListAsync();
            var allTeacherOfTheSubject = await db.Teacher.Where(teacher => teacherIdOfTheSubject.Contains(teacher.Id)).ToListAsync();

            return allTeacherOfTheSubject;
        }


        public async Task<List<Depertment>> GetDepertmentBySubjectId(int id)
        {
            var depertmentIdOfTheSubject = await db.SubjectDepartmentMapped.Where(sd => sd.subjectId == id).Select(sd => sd.departmentId).ToListAsync();
            var allDepertmentOfTheSubject = await db.Depertment.Where(dept => depertmentIdOfTheSubject.Contains(dept.id)).ToListAsync();

            return allDepertmentOfTheSubject;
        }


        public async Task<Subject> GetSubjectDetailsById(int id)
        {
            var subject = await db.Subject.FindAsync(id);
            if (subject == null) return subject;

            subject.Depertments = await GetDepertmentBySubjectId(id);
            subject.Teachers = await GetTeacherBySubjectId(id);
            subject.Students =await GetStudentBySubjectId(id);

            return subject;
        }

    }
}
