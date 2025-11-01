using Microsoft.AspNetCore.Identity;
namespace MovieBuff.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string? ProfilePictureUrl { get; set; }
        public string? Country { get; set; }
    }
}
