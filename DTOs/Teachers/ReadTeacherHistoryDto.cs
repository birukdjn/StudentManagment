namespace StudentManagment.DTOs.Teachers
{
    public class ReadTeacherHistoryDto
    {
        public int HistoryId { get; set; }
        public required int DepartmentId { get; set; }
        public required string DepartmentName { get; set; }
        public required DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}