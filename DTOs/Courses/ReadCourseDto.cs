namespace StudentManagment.DTOs.Courses
{
    public class ReadCourseDto
    {
        public int CourseId { get; set; }
        public required string Code { get; set; }
        public required string Title { get; set; }
        public int Credits { get; set; }
        public required string DepartmentName { get; set; }
        public bool IsActive { get; set; } = true;
    }
}