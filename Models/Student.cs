using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University_Information_System.Models
{
    public class Student
    {
       
            [Key]
            public int id { get; set; }

            [Required]
            [MinLength(4)]
            [MaxLength(10)]
            public string Reg{ get; set; }

            [Required]
            [MinLength(3)]
            [MaxLength(20)]
            public string FirstName { get; set; }

            [Required]
            [MinLength(3)]
            [MaxLength(20)]
            public string LastName { get; set; }

            [Required]
            public int DepertmentId{ get; set; }

            [NotMapped]
            public  List<Subject>? Subjects{ get; set; }

            [NotMapped]
            public string? DeptName { get; internal set; }
    }
}