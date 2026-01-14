namespace StudentManagment.DTOs.Attendance
{
    public record CheckInDto(
         string QrToken,
         double Latitude,
         double Longitude,
         int ClassroomId
     );
}
