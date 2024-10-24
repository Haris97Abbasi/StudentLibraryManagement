using Microsoft.AspNetCore.Identity;

namespace StudentLibraryManagement.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
    }
}
