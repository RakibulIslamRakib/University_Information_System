using Microsoft.EntityFrameworkCore;
 
using University_Information_System.Models;

namespace University_Information_System.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Depertment> Depertment { get; set; }
        public DbSet<Teacher> Teacher { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<Subject> Subject { get; set; }
        public DbSet<SubjectStudentMapped> SubjectStudentMapped { get; set; }
        public DbSet<SubjectTeacherMapped> SubjectTeacherMapped { get; set; }
        public DbSet<SubjectDepartmentMapped> SubjectDepartmentMapped { get; set; }
        public DbSet<DepartmentTeacherMapped> DepartmentTeacherMapped { get; set; }



    }
}
 