using System.ComponentModel.DataAnnotations;

namespace University_Information_System.Models
{
    public class StudentDepertment
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int departmentId { get; set; }
        [Required]
        public string studentId { get; set; }
    }
}
