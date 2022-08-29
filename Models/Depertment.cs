using System.ComponentModel.DataAnnotations;

namespace University_Information_System.Models
{
    public class Depertment
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string DeptName { get; set; }
        public virtual ICollection<Teacher> Teachers { get; set; }



        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Teacher? CreatedBy { get; set; }
        public Teacher? UpdatedBy { get; set; }


    }
}
    