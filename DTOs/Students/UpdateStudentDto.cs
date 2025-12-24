using System.ComponentModel.DataAnnotations;

namespace StudentManagment.DTOs.Students
{
    public class UpdateStudentDto
    {
        [Required(ErrorMessage = "Student ID is required for updating the record.")]
        [Range(1, int.MaxValue, ErrorMessage = "Student ID must be a positive integer.")]
        public required int StudentId { get; set; }

        [Required(ErrorMessage = "Student's Major Department ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Department ID must be a positive integer.")]
        public required int DepartmentId { get; set; }

        [Required(ErrorMessage = "Enrollment Date is required.")]
        public required DateOnly EnrollmentDate { get; set; }

        [Range(0.0, 4.0, ErrorMessage = "GPA must be between 0.0 and 4.0.")]
        public double GPA { get; set; } = 0.0;

        public bool IsEnrolled { get; set; }

        public bool IsAlumni { get; set; }
    }
}