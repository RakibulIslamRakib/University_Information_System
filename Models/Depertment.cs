﻿using System.ComponentModel.DataAnnotations;

namespace University_Information_System.Models
{
    public class Depertment
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string DeptName { get; set; }
        public string Descryption { get; set; }

        public virtual List<Teacher> Teachers { get; set; }
        public virtual List<Subject> Subjects { get; set; }
        public virtual List<Student> Students { get; set; }



        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }


    }
}
    