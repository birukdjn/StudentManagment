using Microsoft.AspNetCore.Mvc;
using StudentManagment.DTOs.Enrollments;
using StudentManagment.Services.Interfaces;

namespace StudentManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController(IEnrollmentService enrollmentService) : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService = enrollmentService;

        // POST: api/Enrollment
        [HttpPost]
        public async Task<ActionResult<ReadEnrollmentDto>> CreateEnrollment(CreateEnrollmentDto dto)
        {
            try
            {
                var readDto = await _enrollmentService.CreateEnrollmentAsync(dto);
                return CreatedAtAction(nameof(GetEnrollment), new { id = readDto.EnrollmentId }, readDto);
            }
            catch (ArgumentException ex) 
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex) 
            {
                return Conflict(ex.Message); 
            }
        }

        // GET: api/Enrollment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadEnrollmentDto>>> GetAllEnrollments()
        {
            var enrollments = await _enrollmentService.GetAllEnrollmentsAsync();
            return Ok(enrollments); 
        }

        // GET: api/Enrollment/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReadEnrollmentDto>> GetEnrollment(int id)
        {
            var enrollment = await _enrollmentService.GetEnrollmentByIdAsync(id);

            if (enrollment == null)
            {
                return NotFound(); 
            }

            return Ok(enrollment); 
        }

        // PUT: api/Enrollment/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEnrollment(int id, UpdateEnrollmentDto dto)
        {
            if (dto == null)
                return BadRequest("Request body is required.");

            // ensure route id and dto id match (helps catch client mistakes)
            if (dto.EnrollmentId != 0 && dto.EnrollmentId != id)
                return BadRequest("Route id and EnrollmentId in body do not match.");

            var success = await _enrollmentService.UpdateEnrollmentAsync(id, dto);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        // PUT (Withdrawal): api/Enrollment/5/withdraw
        [HttpPut("{id}/withdraw")]
        public async Task<IActionResult> WithdrawCourse(int id, [FromQuery] string withdrawalCode = "W")
        {
            try
            {
                var success = await _enrollmentService.WithdrawCourseAsync(id, withdrawalCode);

                if (!success)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/activate")]
        public async Task<IActionResult> ActivateEnrollment(int id)
        {
            try
            {
                var success = await _enrollmentService.ActivateEnrollmentAsync(id);

                if (!success)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE (Hard Delete): api/Enrollment/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnrollmentRecord(int id)
        {
            try
            {
                var success = await _enrollmentService.DeleteEnrollmentRecordAsync(id);

                if (!success)
                {
                    return NotFound();
                }

                return NoContent(); 
            }
            catch (InvalidOperationException ex) 
            {
                return Conflict(ex.Message);
            }
        }
    }
}