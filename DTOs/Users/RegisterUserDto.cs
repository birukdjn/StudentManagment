using StudentManagment.Enums;
using System.ComponentModel.DataAnnotations;

namespace StudentManagment.DTOs.Users
{
    public class RegisterUserDto
    {

        [Required(ErrorMessage = "First Name is required.")]
        [MaxLength(100, ErrorMessage = "First Name cannot exceed 100 characters.")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [MaxLength(100, ErrorMessage = "Last Name cannot exceed 100 characters.")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public required GenderEnum Gender { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        public required DateOnly DateOfBirth { get; set; }


        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "The email address format is invalid. Example: user@domain.com")]
        [MaxLength(100, ErrorMessage = "Email address cannot exceed 100 characters.")]
        public required string Email { get; set; }

        [MaxLength(50, ErrorMessage = "Username cannot exceed 50 characters.")]
        public string? Username { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format.")]
        [MaxLength(13, ErrorMessage = "Phone number cannot exceed 13 characters.")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Confirmation password is required.")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public required string ConfirmPassword { get; set; }
    }
}