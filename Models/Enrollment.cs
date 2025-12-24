using StudentManagment.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagment.Models
{
    public class Enrollment
    {
        [Key]
        public int EnrollmentId { get; set; }

        public required int StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public Student Student { get; set; } = null!;

        public required int CourseId { get; set; }
        [ForeignKey(nameof(CourseId))]
        public Course Course { get; set; } = null!;

        public required int Year { get; set; }
        public required SemesterEnum Semester { get; set; }

        [MaxLength(5)]
        public string? Grade { get; set; }
    }
}