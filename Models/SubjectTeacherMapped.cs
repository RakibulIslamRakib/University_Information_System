using System.ComponentModel.DataAnnotations;

namespace University_Information_System.Models
{
    public class SubjectTeacherMapped
    {
        [Key]
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public int SubjectId { get; set; }
    }
}
