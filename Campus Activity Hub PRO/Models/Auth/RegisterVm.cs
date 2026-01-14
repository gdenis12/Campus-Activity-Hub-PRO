using System.ComponentModel.DataAnnotations;

namespace Campus_Activity_Hub_PRO.Models.Auth
{
    public class RegisterVm
    {
        [Required]
        [StringLength(60)]
        public string Name { get; set; } = "";

        [Required, EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = "";

        [Required]
        public string Role { get; set; } = "Student"; 
    }
}
