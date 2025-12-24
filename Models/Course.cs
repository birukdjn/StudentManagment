using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagment.Models
{
    [Index(nameof(Code), IsUnique = true)]
    public class Course
    {
        [Key]
        public int CourseId { get; set; }

        [Required]
        [MaxLength(10)]
        public required string Code { get; set; }

        [Required]
        [MaxLength(200)]
        public required string Title { get; set; }
        public int Credits { get; set; }
        public bool IsActive { get; set; } = true;
        public required int DepartmentId { get; set; }
        [ForeignKey(nameof(DepartmentId))]
        public Department Department { get; set; } = null!;

        public ICollection<Enrollment> Enrollments { get; set; } = [];
        public ICollection<CourseAssignment> CourseAssignments { get; set; } = [];

    }
}