namespace StudentManagment.DTOs.CafeAccess
{
    public class ReadCafeAccessDto
    {
        public int CafeAccessId { get; set; }
        public int StudentId { get; set; }
        public required string StudentName { get; set; }
        public required string ScannableIdCode { get; set; }
        public bool HasAccessedBreakfast { get; set; }
        public bool HasAccessedLunch { get; set; }
        public bool HasAccessedDinner { get; set; }

        public DateTime LastResetDate { get; set; }
        public int TotalDailyAccesses { get; set; }
    }
}
