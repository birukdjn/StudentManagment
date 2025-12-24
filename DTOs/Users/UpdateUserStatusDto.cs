using System.ComponentModel.DataAnnotations;

namespace StudentManagment.DTOs.Users
{
    public class UpdateUserStatusDto
    {
        [Required(ErrorMessage = "User ID is required.")]
        public required int UserId { get; set; }

        [Required(ErrorMessage = "Active status is required.")]
        public required bool IsActive { get; set; }
    }
}