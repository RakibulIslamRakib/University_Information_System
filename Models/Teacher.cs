﻿using System.ComponentModel.DataAnnotations;
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


        [NotMapped]
        public  List<Subject> Subjects { get; set; }
        [NotMapped]
        public List<Depertment> Depertments { get; set; }
    }
}