using System.ComponentModel.DataAnnotations;

namespace StudentManagment.DTOs.Departments
{
    public class UpdateDepartmentDto
    {
        public required int DepartmentId { get; set; }

        [Required(ErrorMessage = "Department Name is required.")]
        [MaxLength(100, ErrorMessage = "Department name cannot exceed 100 characters.")]
        public required string Name { get; set; }

        public bool IsActive { get; set; }
    }
}
