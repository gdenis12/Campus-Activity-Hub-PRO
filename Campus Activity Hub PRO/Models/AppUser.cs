using Microsoft.AspNetCore.Identity;

namespace Campus_Activity_Hub_PRO_comand_second.Models
{
    public class AppUser : IdentityUser<int>
    {
        public string Name { get; set; } = "";

    }
}