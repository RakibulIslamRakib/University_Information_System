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

        public TeacherService(ApplicationDbContext db, ISubjectService subjectService)
        {
            this.db = db;
            this.subjectService = subjectService;
        }


        public async Task AddTeacher(Teacher teacher)
        {
           await db.Teacher.AddAsync(teacher);
           await db.SaveChangesAsync();
        }

        public async Task DeleteSubTeacherMapped(int subjectId, int teacherId)
        {
            var subTeacher = await db.SubjectTeacherMapped.FirstOrDefaultAsync(st => st.TeacherId == teacherId && st.SubjectId == subjectId);

            if (subTeacher != null)
            {
                db.SubjectTeacherMapped.Remove(subTeacher);
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteTeacher(Teacher teacher)
        {
            var subTeacherOfTheTeacher = await db.SubjectTeacherMapped.Where(st => st.TeacherId == teacher.Id).ToListAsync();
            db.SubjectTeacherMapped.RemoveRange(subTeacherOfTheTeacher);
            await db.SaveChangesAsync();

            db.Teacher.Remove(teacher);
            await db.SaveChangesAsync();
        }



        public async Task<List<Teacher>> getAllTeacher()
        {
            return await db.Teacher.ToListAsync();

        }



        public async Task<Teacher> GetTeacherById(int id)
        {
            return await db.Teacher.FindAsync(id);
        }



        public async Task UpdateTeacher(Teacher teacher)
        {
            db.Teacher.Update(teacher);
            await db.SaveChangesAsync();
        }



        public async Task<List<Subject>> GetSubjectByTeacherId(int id)
        {
            var subjectIdOfTheTeacher = await db.SubjectTeacherMapped.Where(st => st.TeacherId == id).Select(st=>st.SubjectId).ToListAsync();
            var allSubjectOfTheTeacher = await db.Subject.Where(sb=> subjectIdOfTheTeacher.Contains(sb.id)).ToListAsync();

            return allSubjectOfTheTeacher;
        }

       

        public async Task AddSubjectTeacherMapped(SubjectTeacherMapped subjectTeacherMapped)
        {
            await db.SubjectTeacherMapped.AddAsync(subjectTeacherMapped);
            await db.SaveChangesAsync();
        }


        public async Task<Teacher> GetTeacherDetailsById(int id)
        {
            var teacher = await GetTeacherById(id);
            if (teacher == null) return teacher;
            teacher.Subjects = await GetSubjectByTeacherId(id);

            teacher.Depertments = new List<Depertment>();
          
            foreach (var subject in teacher.Subjects)
            {
                teacher.Depertments.AddRange(await subjectService.GetDepertmentBySubjectId(subject.id));
            }
            var depertmentSet = new HashSet<Depertment>(teacher.Depertments);
            teacher.Depertments = new List<Depertment>(depertmentSet);


            teacher.Students = new List<Student>();

            foreach (var subject in teacher.Subjects)
            {
                teacher.Students.AddRange(await subjectService.GetStudentBySubjectId(subject.id));
            }
            var studenttSet = new HashSet<Student>(teacher.Students);
            teacher.Students = new List<Student>(studenttSet);

            return teacher;
        }

    }
}
