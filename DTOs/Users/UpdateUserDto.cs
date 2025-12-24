using StudentManagment.Enums;
using System.ComponentModel.DataAnnotations;

namespace StudentManagment.DTOs.Users
{
    public class UpdateUserDto
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

        [Required(ErrorMessage = "Username is required.")]
        [MaxLength(50, ErrorMessage = "Username cannot exceed 50 characters.")]
        public required string Username { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format.")]
        [MaxLength(13, ErrorMessage = "Phone number cannot exceed 13 characters.")]
        public string? Phone { get; set; }
    }
}