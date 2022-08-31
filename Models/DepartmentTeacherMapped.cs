using System.ComponentModel.DataAnnotations;

namespace University_Information_System.Models
{
    public class DepartmentTeacherMapped
    {
        [Key]
        public int id { get; set; }
        public int teacherId { get; set; }
        public int departmentId { get; set; }
    }
}
