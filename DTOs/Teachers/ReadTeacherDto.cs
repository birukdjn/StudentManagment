using StudentManagment.Enums;

namespace StudentManagment.DTOs.Teachers
{
    public class ReadTeacherDto
    {
        public int TeacherId { get; set; }

        // Identity fields sourced from User
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public string? Phone { get; set; }
        public GenderEnum Gender { get; set; }
        public DateTime DateOfBirth { get; set; }


        public string? DepartmentName { get; set; } 
        public int? DepartmentId { get; set; } 
        public required DateTime HireDate { get; set; }

        public ICollection<ReadTeacherHistoryDto>? DepartmentHistory { get; set; }

    }
}