using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using University_Information_System.Data;
using University_Information_System.Models;
using University_Information_System.Services.ServiceInterfaces;

namespace University_Information_System.Services.ServiceClasses
{
    public class DepartmentService: IDepartmentService
    {

        public readonly ApplicationDbContext db;
        private readonly ISubjectService subjectService;
        private readonly IStudentService studentService;
 

        public DepartmentService(ApplicationDbContext db , ISubjectService subjectService,
            IStudentService studentService )
        {
            this.db = db;
            this.subjectService = subjectService;
            this.studentService = studentService;
   
        }

        public async Task AddDepertment(Depertment depertment)
        {
            await db.Depertment.AddAsync(depertment);
            await db.SaveChangesAsync();
        }

        public async Task<List<Depertment> > GetAllDepertment()
        {

            return await db.Depertment.ToListAsync() ;
        }

        public async Task<Depertment> GetDepertmentById(int id)
        {
          var department = await db.Depertment.FindAsync(id);
            if (department == null) return new Depertment();

            return department;
        }

        public async Task UpdateDepertment(Depertment depertment)
        {
            var dept = await GetDepertmentById(depertment.id);
            dept.DeptName = depertment.DeptName;
            dept.Descryption = depertment.Descryption;
            dept.UpdatedDate = depertment.UpdatedDate;
            dept.UpdatedBy = depertment.UpdatedBy;
            db.Depertment.Update(dept);
           await db.SaveChangesAsync();
        }

        public async Task DeleteDepertment(Depertment depertment)
        {
            var studentOdTheDept = await GetStudentByDepertmentId(depertment.id);
            var enrolMentsOfTheStudentsOfTheDept = new List<SubjectStudentMapped>();

            foreach (var student in studentOdTheDept)
            {
                 enrolMentsOfTheStudentsOfTheDept.AddRange(db.SubjectStudentMapped.Where(enrol => enrol.studentId == student.Id));
            }
            db.SubjectStudentMapped.RemoveRange(enrolMentsOfTheStudentsOfTheDept);
            await db.SaveChangesAsync();

            //remove student role from those user
            await db.SaveChangesAsync();

            var subDeptOfTheDept = await db.SubjectDepartmentMapped.Where(sd=>sd.departmentId==depertment.id).ToListAsync();
            db.SubjectDepartmentMapped.RemoveRange(subDeptOfTheDept);
            await db.SaveChangesAsync();

            db.Depertment.Remove(depertment);
            await db.SaveChangesAsync();
        }



        public async Task<List<ApplicationUser> > GetTeacherByDepertmentId(int id)
        {
            var subjects = await GetSubjectByDepertmentId(id);
            var teachers = new List<ApplicationUser>();

            foreach (var subject in subjects)
            {
                teachers.AddRange( await subjectService.GetTeacherBySubjectId(subject.id));
            }
            var teacher = new HashSet<ApplicationUser>(teachers);
            teachers = new List<ApplicationUser>(teacher);

            return teachers;
        }



        public async Task<Depertment> GetDepertmentDetailsById(int id)
        {
            var depertment = await db.Depertment.FindAsync(id);
            if (depertment == null) return new Depertment();

            depertment.Students = await GetStudentByDepertmentId(id);
            depertment.Subjects = await GetSubjectByDepertmentId(id);
            depertment.Teachers = await GetTeacherByDepertmentId(id);

            return depertment;
        }



        public async Task<List<Subject>> GetSubjectByDepertmentId(int id)
        {
            var subjectIdOfTheDepertment = await db.SubjectDepartmentMapped.Where(
                sd => sd.departmentId == id).Select(sd => sd.subjectId).ToListAsync();
            
            
            var allSubjectOfTheDepertment = await db.Subject.Where(subject=> subjectIdOfTheDepertment.Contains(subject.id)).ToListAsync();

            return  allSubjectOfTheDepertment;
        }
        


        public async Task<List<ApplicationUser>> GetStudentByDepertmentId(int id)
        {
            var studentIdOfDept = await db.StudentDepertment.Where(sd=>sd.departmentId==id).Select(sd => sd.studentId).ToListAsync();
            var students = await studentService.GetAllStudent();
            var studentOfDept = students.Where(st => studentIdOfDept.Contains(st.Id)).ToList();
            return studentOfDept;
        }


        public async Task DeleteStudentFromDept(string studentId)
        {
            var student = await studentService.GetStudentById(studentId);

            if (student != null)
            {
                await studentService.DeleteStudent(student);
            }
    
        }

        public async Task AddSubjectDapertmentMapped(SubjectDepartmentMapped subjectDapertmentMapped)
        {
           await db.SubjectDepartmentMapped.AddAsync(subjectDapertmentMapped);
           await db.SaveChangesAsync();
        }


        public async Task DeleteSubjectFromSubjectDepartmentMapped(int subjectId, int deptId)
        {
            var subDeptVar = await db.SubjectDepartmentMapped.FirstOrDefaultAsync(sd => sd.departmentId == deptId && sd.subjectId==subjectId);
            
            if (subDeptVar != null)
            {
                db.SubjectDepartmentMapped.Remove(subDeptVar);
                await db.SaveChangesAsync();
            }
        }


        public async Task<List<Subject>> SubjectOutOfDept(int depertmentId)
        {
            var subjects = await subjectService.getAllSubject(); 
            var subjectOftheDept = await GetSubjectByDepertmentId(depertmentId);
            var subjectOutOftheDept = subjects.Except(subjectOftheDept).ToList();
            return subjectOutOftheDept;
        }

    }
}
