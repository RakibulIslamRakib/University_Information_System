using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University_Information_System.Models
{
    public class Depertment:Base
    {
        [Key]
        public int id { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string DeptName { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(400)]
        public string Descryption { get; set; }


        [NotMapped]
        public  List<ApplicationUser>? Teachers { get; set; }
        [NotMapped]
        public  List<Subject>? Subjects { get; set; }
        [NotMapped]
        public  List<ApplicationUser>? Students { get; set; }


        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

  
    }
}
    