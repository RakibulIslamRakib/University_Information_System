using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University_Information_System.Models
{
    public class Teacher
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string LastName { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(200)]
        public string Descryption { get; set; }


        [NotMapped]
        public  List<Subject>? Subjects { get; set; }
        [NotMapped]
        public List<Depertment>? Depertments { get; set; }
        [NotMapped]
        public List<Student>? Students { get; set; }
    }
}