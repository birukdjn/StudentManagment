using StudentManagment.Enums;
using System.ComponentModel.DataAnnotations;

namespace StudentManagment.DTOs.Enrollments
{
    public class CreateEnrollmentDto
    {
        [Required(ErrorMessage = "Student ID is required.")]
        public required int StudentId { get; set; }

        [Required(ErrorMessage = "Course ID is required.")]
        public required int CourseId { get; set; }

        [Required(ErrorMessage = "Year of enrollment is required.")]
        public required int Year { get; set; }

        [Required(ErrorMessage = "Semester is required.")]
        public required SemesterEnum Semester { get; set; }

    }
}