using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.User
{
    public class CreateUserVM
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }


        [Required]
        [EmailAddress]
        public string Email { get; set; }


        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }


        [Required]
        public string Role { get; set; }
    }
}
