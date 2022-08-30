using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University_Information_System.Models
{
    public class Teacher
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Descryption { get; set; }

        public virtual Depertment Depertment { get; set; }


        [NotMapped]
        public virtual List<Subject> Subjects { get; set; }
    }
}