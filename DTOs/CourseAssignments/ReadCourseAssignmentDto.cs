namespace StudentManagment.DTOs.CourseAssignments
{
    public class ReadCourseAssignmentDto
    {
        public int CourseAssignmentId { get; set; }

        public int CourseId { get; set; }
        public required string CourseTitle { get; set; }

        public int TeacherId { get; set; }
        public required string TeacherName { get; set; }

        public int Year { get; set; }
        public required string Semester { get; set; }
    }
}