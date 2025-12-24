using System.ComponentModel.DataAnnotations;

namespace StudentManagment.DTOs.Users
{
    public class UpdatePasswordDto
    {
       
        [Required(ErrorMessage = "Current Password is required.")]
        public required string CurrentPassword { get; set; }

        [Required(ErrorMessage = "New Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        public required string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirmation password is required.")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public required string ConfirmNewPassword { get; set; }
    }
}