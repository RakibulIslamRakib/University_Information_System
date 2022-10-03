using System.ComponentModel.DataAnnotations;

namespace University_Information_System.Models
{
    public class SubjectStudentMapped
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string studentId { get; set; }
        [Required]
        public int subjectId { get; set; }
    }
}
