using StudentManagment.Enums;
using System.ComponentModel.DataAnnotations;

namespace StudentManagment.Models
{
    public class AttendanceRecord
    {
        [Key]
        public int AttendanceId { get; set; }
        public int StudentId { get; set; }
        public Student? Student { get; set; }
        public int ClassroomId { get; set; }
        public Classroom? Classroom { get; set; }

        public AttendanceEventType EventType { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;


        // Security metadata
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool IsVerified { get; set; }
    }
}
