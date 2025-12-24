using System.ComponentModel.DataAnnotations;

namespace StudentManagment.DTOs.CourseAssignments
{
    public class CreateCourseAssignmentDto
    {
        [Required(ErrorMessage = "Course ID is required.")]
        public required int CourseId { get; set; }

        [Required(ErrorMessage = "Teacher ID is required.")]
        public required int TeacherId { get; set; }

        [Required(ErrorMessage = "Year of assignment is required.")]
        public required int Year { get; set; }

        [Required(ErrorMessage = "Semester is required.")]
        [RegularExpression("^(Fall|Spring|Summer)$", ErrorMessage = "Semester must be Fall, Spring, or Summer.")]
        public required string Semester { get; set; }
    }
}