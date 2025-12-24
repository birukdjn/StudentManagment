using StudentManagment.Enums;
using System.ComponentModel.DataAnnotations;

namespace StudentManagment.DTOs.Enrollments
{
    public class UpdateEnrollmentDto
    {
        [Required(ErrorMessage = "Enrollment ID is required for update.")]
        public int EnrollmentId { get; set; }


        [Required(ErrorMessage = "Year of enrollment is required.")]
        public required int Year { get; set; }

        [Required(ErrorMessage = "Semester is required.")]
        public required SemesterEnum Semester { get; set; }

        [MaxLength(5)]
        public string? Grade { get; set; }
    }
}