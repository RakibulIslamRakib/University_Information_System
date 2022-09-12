using System.ComponentModel.DataAnnotations;

namespace University_Information_System.Models
{
    public class SubjectDepartmentMapped
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int departmentId { get; set; }
        [Required]
        public int subjectId { get; set; }
    }
}
