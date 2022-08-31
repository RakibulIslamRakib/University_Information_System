using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University_Information_System.Models
{
    public class Student
    {
       
            [Key]
            public int id { get; set; }
            public int Reg{ get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int DepertmentId{ get; set; }

            [NotMapped]
            public  List<Subject> Subjects{ get; set; }

            [NotMapped]
            public string DeptName { get; internal set; }
    }
}