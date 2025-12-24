using System.ComponentModel.DataAnnotations;

namespace StudentManagment.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Department Name is required.")]
        [MaxLength(100)]
        public required string Name { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<Teacher>? Teachers { get; set; }
        public ICollection<Student>? Students { get; set; }
        public ICollection<TeacherDepartmentHistory>? TeacherHistoryLinks { get; set; }
        public ICollection<Course>? Courses { get; set; }
    }
}