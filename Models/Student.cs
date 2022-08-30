using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University_Information_System.Models
{
    public class Student
    {
       
            [Key]
            public int id { get; set; }
            [Required]
            public int Reg{ get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public Depertment Depertment{ get; set; }

            [NotMapped]
            public virtual List<Subject> Subjects{ get; set; }
    

    }
}