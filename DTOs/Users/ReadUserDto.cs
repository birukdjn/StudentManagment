using StudentManagment.Enums;

namespace StudentManagment.DTOs.Users
{
    public class ReadUserDto
    {
        public int UserId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public string? Phone { get; set; }
        public required string Username { get; set; }
        public UserRoleEnum Role { get; set; }
        public GenderEnum Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? StudentId { get; set; }
        public int? TeacherId { get; set; }
    }
}