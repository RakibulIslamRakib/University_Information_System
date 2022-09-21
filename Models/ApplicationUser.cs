using Microsoft.AspNetCore.Identity;

namespace University_Information_System.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LasttName { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
