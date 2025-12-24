using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagment.Models
{
    public class Teacher
    {
        [Key]
        public int TeacherId { get; set; }

        
        public int? DepartmentId { get; set; }
        [ForeignKey(nameof(DepartmentId))]
        public Department? Department { get; set; } = null!;
        public required DateOnly HireDate { get; set; }
        public required int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
        public ICollection<TeacherDepartmentHistory>? DepartmentHistory { get; set; }
        public ICollection<CourseAssignment>? CourseAssignments { get; set; }
    }
}