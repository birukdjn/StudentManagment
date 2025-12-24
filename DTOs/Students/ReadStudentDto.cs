using StudentManagment.Enums;

namespace StudentManagment.DTOs.Students
{
    public class ReadStudentDto
    {
        public int StudentId { get; set; }

        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public string? Phone { get; set; }
        public GenderEnum Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public required DateOnly EnrollmentDate { get; set; }
        public double GPA { get; set; }
        public bool IsEnrolled { get; set; }
        public bool IsAlumni { get; set; }
        public required string DepartmentName { get; set; }
        public required int DepartmentId { get; set; }
    }
}
