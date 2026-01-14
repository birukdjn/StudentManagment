using System.ComponentModel.DataAnnotations;

namespace StudentManagment.Models
{
    public class Classroom
    {
        [Key]
        public int RoomId { get; set; }
        public required string RoomName { get; set; }
        // For Geofencing
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        // A secret rotated by the teacher's dashboard to generate the QR
        public string? DynamicSecret { get; set; }
    }
}
