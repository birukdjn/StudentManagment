using Microsoft.EntityFrameworkCore;
using StudentManagment.Data;
using StudentManagment.Models;

namespace StudentManagment.Services
{
    public class AttendanceService(SchoolContext context)
    {
        private readonly SchoolContext _context = context;


        public async Task<bool> ProcessCheckInAsync(int userId, string token, double latitude, double longitude, int roomId)
        {
            var room = await _context.Classrooms.FindAsync(roomId);
            var student = await _context.Students.FirstOrDefaultAsync(s=> s.UserId == userId);
            double distance = CalculateDistance(latitude, longitude, room.Latitude, room.Longitude);


            if (room == null || student == null) return false;

            if (room.DynamicSecret != token) return false;

            if (distance > 50) return false;

            
            var record = new AttendanceRecord
            {
                StudentId = student.StudentId,
                ClassroomId = roomId,
                IsVerified = true
            };

            _context.AttendanceRecords.Add(record);
            await _context.SaveChangesAsync();
            return true;
        }


        private double CalculateDistance(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            var r = 6371e3; // Earth radius in Meters
            var phi1 = latitude1 * Math.PI / 180;
            var phi2 = latitude2 * Math.PI / 180;
            var dPhi = (latitude2 - latitude1) * Math.PI / 180;
            var dLambda = (longitude2 - longitude1) * Math.PI / 180;

            var a = Math.Sin(dPhi / 2) * Math.Sin(dPhi / 2) +
                    Math.Cos(phi1) * Math.Cos(phi2) *
                    Math.Sin(dLambda / 2) * Math.Sin(dLambda / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return r * c; // Distance in meters
        }
    }

}
