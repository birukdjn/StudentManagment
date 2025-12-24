using StudentManagment.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagment.Models
{
    public class CourseAssignment
    {
        [Key]
        public int CourseAssignmentId { get; set; }

        public required int CourseId { get; set; }
        [ForeignKey(nameof(CourseId))]
        public Course Course { get; set; } = null!;

        public required int TeacherId { get; set; }
        [ForeignKey(nameof(TeacherId))]
        public Teacher Teacher { get; set; } = null!;

        public required int Year { get; set; }
        public required SemesterEnum Semester { get; set; }
    }
}