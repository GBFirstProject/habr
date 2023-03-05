using Microsoft.AspNetCore.Identity;

namespace HabrParser.Models.APIAuth
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsBlocked { get; set; }
    }
}
