using Microsoft.EntityFrameworkCore;
using University_Information_System.Data;
using University_Information_System.Models;
using University_Information_System.Services.ServiceInterfaces;

namespace University_Information_System.Services.ServiceClasses
{
    public class StudentService:  IStudentService
    {

        public readonly ApplicationDbContext db;

        public StudentService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task AddStudent(Student student)
        {
           await db.Student.AddAsync(student);
           await db.SaveChangesAsync();
        }

        public async Task<List<Student>> getAllStudent()
        {
            return await db.Student.ToListAsync(); 
        }

        public async Task UpdateStudent(Student student)
        {
            db.Student.Update(student);
            await db.SaveChangesAsync();
        }

        public async Task<Student> GetStudentById(int id)
        {
            return await db.Student.FindAsync(id);
        }

        public async Task DeleteStudent(Student student)
        {
            var subStudentVar = await db.SubjectStudentMapped.Where(ss => ss.studentId == student.id).ToListAsync();
            
            db.SubjectStudentMapped.RemoveRange(subStudentVar);
            await db.SaveChangesAsync();
            db.Student.Remove(student);
            await db.SaveChangesAsync();
        }



        public async Task<Student> GetStudentDetailsById(int id)
        {
            var student = await db.Student.FindAsync(id);
            if(student == null)return new Student();

            var dept = await db.Depertment.FindAsync(student.DepertmentId);

            if(dept == null)return new Student();

            student.DeptName = dept.DeptName;
            student.Subjects = await GetSubjectByStudentId(id);

            return student;
        }


 



        public async Task AddSubjectStudentMapped(SubjectStudentMapped subjectStudentMapped)
        {
            await db.SubjectStudentMapped.AddAsync(subjectStudentMapped);
            await db.SaveChangesAsync();
        }

        public async Task DeleteEnrolmentFromSubjectStudentMapped(int subjectId, int studentId)
        {
            var subStudentVar = await db.SubjectStudentMapped.FirstOrDefaultAsync(ss=>ss.studentId == studentId && ss.subjectId==subjectId);
            if (subStudentVar != null)
            {
                db.SubjectStudentMapped.Remove(subStudentVar);
                await db.SaveChangesAsync();
            }
        }



        public async Task<List<Subject>> GetSubjectByStudentId(int id)
        {
            var subjectIdOfTheStudent = await db.SubjectStudentMapped.Where(ss => ss.studentId == id).Select(ss => ss.subjectId).ToListAsync();
            var allSubjectOfTheStudent = await db.Subject.Where(subject => subjectIdOfTheStudent.Contains(subject.id)).ToListAsync();

            return allSubjectOfTheStudent;
        }


    }
}
