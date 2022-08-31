using System.ComponentModel.DataAnnotations;

namespace University_Information_System.Models
{
    public class SubjectStudentMapped
    {
        [Key]
        public int id { get; set; }
        public int studentId { get; set; }
        public int subjectId { get; set; }
    }
}
