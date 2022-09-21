using Microsoft.Build.Framework;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace University_Information_System.Models
{
    public class SignUpModel
    {
        [Display(Name ="Email Address" )]
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage ="Please enter a valid email")]
        public string Email { get; set; }

        [Display(Name = "First Name ")]
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }

        [Display(Name = "About")]
        [Required(ErrorMessage = "About is required.")]
        public string About { get; set; }

        [Display(Name = "Date Of Birth ")]
        [DataType(DataType.DateTime)]
        public DateTime DateOfBirth { get; set; }


        [Display(Name = "Password")]
        [Compare("ConfirmPassword", ErrorMessage ="Password does not match")]
        [Required(ErrorMessage = "Please Enter a strong password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Please confirm your password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
