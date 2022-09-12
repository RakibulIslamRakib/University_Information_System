using System.ComponentModel.DataAnnotations;

namespace University_Information_System.Models
{
    public class SubjectTeacherMapped
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int TeacherId { get; set; }
        [Required]
        public int SubjectId { get; set; }
    }
}
