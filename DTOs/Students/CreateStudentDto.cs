using System.ComponentModel.DataAnnotations;

namespace StudentManagment.DTOs.Students
{
    public class CreateStudentDto
    {
        [Required(ErrorMessage = "Enrollment year is required.")]
        [Range(1990, 2099, ErrorMessage = "Enrollment year must be a valid four-digit year.")]
        public required int EnrollmentDate { get; set; }

        [Required(ErrorMessage = "Student's Major Department ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Department ID must be a positive integer.")]
        public required int DepartmentId { get; set; }
    }
}
