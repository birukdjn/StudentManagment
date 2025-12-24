using System.ComponentModel.DataAnnotations;

namespace StudentManagment.DTOs.Users
{
    public class LoginUserDto
    {
        [Required(ErrorMessage = "Email or Username is required.")]
        [MaxLength(100, ErrorMessage = "Input cannot exceed 100 characters.")]
        public required string EmailOrUsername { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public required string Password { get; set; }
    }
}