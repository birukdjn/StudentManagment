using System.ComponentModel.DataAnnotations;

namespace StudentManagment.DTOs.Teachers
{
    public class CreateTeacherDto
    {
        
        [Range(1, int.MaxValue, ErrorMessage = "Department ID must be a positive integer.")]
        public int? DepartmentId { get; set; }

        [Required(ErrorMessage = "Hire Date is required.")]
        public required DateTime HireDate { get; set; }
    }
}
