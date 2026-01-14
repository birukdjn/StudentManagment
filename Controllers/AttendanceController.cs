using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagment.DTOs.Attendance;
using StudentManagment.Services;
using System.Security.Claims;

namespace StudentManagment.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AttendanceController(AttendanceService attendanceService) : ControllerBase
    {
        [HttpPost("checkin")]
        public async Task<IActionResult> CheckIn([FromBody] CheckInDto dto)
        {
            // Reuse your existing logic to get UserID from JWT
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId)) return Unauthorized();

            var success = await attendanceService.ProcessCheckInAsync(
                userId, dto.QrToken, dto.Latitude, dto.Longitude, dto.ClassroomId
            );

            if (!success) return BadRequest("Invalid Token or you are outside the classroom range.");

            return Ok(new { message = "Attendance marked successfully!" });
        }
    }
}