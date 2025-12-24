using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using StudentManagment.Enums;

namespace StudentManagment.Models
{
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(Username), IsUnique = true)]
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required, MaxLength(100)]
        public required string FirstName { get; set; }

        [Required, MaxLength(100)]
        public required string LastName { get; set; }

        [Required]
        public GenderEnum Gender { get; set; }

        [Required]
        public DateOnly DateOfBirth { get; set; }

        [Phone, MaxLength(13)]
        public string? Phone { get; set; }

        [Required, MaxLength(50)]
        public required string Username { get; set; }

        public UserRoleEnum Role { get; set; } = UserRoleEnum.Guest;

        [Required, EmailAddress, MaxLength(100)]
        public required string Email { get; set; }

        [Required, MaxLength(255)]
        public required string PasswordHash { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastLogin { get; set; }

        [MaxLength(500)]
        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }

        // Navigation properties
        public Student? Student { get; set; }
        public Teacher? Teacher { get; set; }
    }
}
