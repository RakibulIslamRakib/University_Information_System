using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University_Information_System.Models
{
    public class Notice
    {
        [Key]
        public int id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(40)]
        public string NoticeTitle { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(400)]
        public string Descryption { get; set; }

        [NotMapped]
        public string? Creator { get; set; }



        [Required]
        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [Required]
        public int CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }


    }
}
