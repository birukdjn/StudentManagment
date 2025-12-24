using StudentManagment.Enums;

namespace StudentManagment.DTOs.Enrollments
{
    public class ReadEnrollmentDto
    {
        public int EnrollmentId { get; set; }

        // Student Info
        public int StudentId { get; set; }
        public required string StudentName { get; set; } // e.g., "Jane Doe"

        // Course Info
        public int CourseId { get; set; }
        public required string CourseTitle { get; set; }
        public required string CourseCode { get; set; }

        public int Year { get; set; }
        public required SemesterEnum Semester { get; set; }
        public string? Grade { get; set; }
    }
}