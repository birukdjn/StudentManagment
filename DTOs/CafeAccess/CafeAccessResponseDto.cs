namespace StudentManagment.DTOs.CafeAccess
{
    public class CafeAccessResponseDto
    {
        public bool AccessGranted { get; set; }
        public required string Message { get; set; }
        public string? StudentName { get; set; }
        public int? StudentId { get; set; }
        public string? GrantedMeal { get; set; } // "Breakfast", "Lunch", or "Dinner"
        public DateTime ScanTime { get; set; } = DateTime.UtcNow;
    }
}
