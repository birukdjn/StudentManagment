using StudentManagment.Enums;
using System.ComponentModel.DataAnnotations;

namespace StudentManagment.DTOs.Users
{
    public class UpdateUserRoleDto
    {
        [Required(ErrorMessage = "User ID is required.")]
        public required int UserId { get; set; }

        [Required(ErrorMessage = "New role is required.")]
        public required UserRoleEnum NewRole { get; set; }
    }
}