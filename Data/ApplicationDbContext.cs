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
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Course>Course { get; set; }
       


    }
}
 