using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University_Information_System.Models
{
    public class Depertment
    {
        [Key]
        public int id { get; set; }
        public string DeptName { get; set; }
        public string Descryption { get; set; }

        [NotMapped]
        public  List<Teacher> Teachers { get; set; }
        [NotMapped]
        public  List<Subject> Subjects { get; set; }
        [NotMapped]
        public  List<Student> Students { get; set; }



        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }


    }
}
    