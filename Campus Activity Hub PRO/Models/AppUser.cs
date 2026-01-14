using Microsoft.AspNetCore.Identity;

namespace Campus_Activity_Hub_PRO.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; } = "";
    }
}
