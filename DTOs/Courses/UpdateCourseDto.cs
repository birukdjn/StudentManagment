using System.ComponentModel.DataAnnotations;

namespace StudentManagment.DTOs.Courses
{
    public class UpdateCourseDto
    {
        [Required(ErrorMessage = "Course ID is required for update.")]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Course Code is required.")]
        [MaxLength(10)]
        public required string Code { get; set; }

        [Required(ErrorMessage = "Course Title is required.")]
        [MaxLength(200)]
        public required string Title { get; set; }

        [Required(ErrorMessage = "Credits are required.")]
        [Range(1, 10, ErrorMessage = "Credits must be between 1 and 10.")]
        public int Credits { get; set; }

        [Required(ErrorMessage = "Active status is required.")]
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Department ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Department ID must be a positive integer.")]
        public required int DepartmentId { get; set; }
    }
}