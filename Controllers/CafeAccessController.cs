using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagment.DTOs.CafeAccess;
using StudentManagment.Services.Interfaces;

namespace StudentManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CafeAccessController(ICafeAccessService cafeAccessService) : ControllerBase
    {
        private readonly ICafeAccessService _cafeAccessService = cafeAccessService;

        // POST: api/CafeAccess/scan - Endpoint for the physical scanning device
        [HttpPost("scan")]
        [AllowAnonymous] // Access attempt must be possible without user login (e.g., dedicated scan device)
        public async Task<ActionResult<CafeAccessResponseDto>> ScanAccess([FromBody] CafeScanRequestDto dto)
        {
            var response = await _cafeAccessService.AttemptAccessAsync(dto);

            if (response.AccessGranted)
            {
                return Ok(response);
            }
            // Use 403 Forbidden to clearly indicate a denied access based on rules
            return StatusCode(403, response);
        }

        // POST: api/CafeAccess (Admin creates the record and assigns ID)
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<ReadCafeAccessDto>> CreateCafeAccess([FromBody] CreateCafeAccessDto dto)
        {
            try
            {
                var readDto = await _cafeAccessService.CreateCafeAccessAsync(dto);
                return CreatedAtAction(nameof(GetCafeAccessByStudentId), new { studentId = readDto.StudentId }, readDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/CafeAccess/student/5
        [HttpGet("student/{studentId}")]
        //[Authorize(Roles = "Admin,Student")] // Admin can see all, Students can see their own (requires policy check)
        public async Task<ActionResult<ReadCafeAccessDto>> GetCafeAccessByStudentId(int studentId)
        {
            // Note: In a real app, you would add logic here to check if the authenticated 
            // user is the studentId they are requesting, unless they are an Admin.

            var cafeAccess = await _cafeAccessService.GetCafeAccessByStudentIdAsync(studentId);

            if (cafeAccess == null)
            {
                return NotFound();
            }

            return Ok(cafeAccess);
        }

        // GET: api/CafeAccess
        [HttpGet]
        //[Authorize(Roles = "Admin")]
        [AllowAnonymous] // For testing purposes, allow anonymous access to view all records
        public async Task<ActionResult<IEnumerable<ReadCafeAccessDto>>> GetAllCafeAccessRecords()
        {
            var records = await _cafeAccessService.GetAllCafeAccessRecordsAsync();
            return Ok(records);
        }

        // PUT: api/CafeAccess/reset/5 (Manual reset for a specific student)
        [HttpPut("reset/{studentId}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> ResetSingleStudentAccess(int studentId)
        {
            var success = await _cafeAccessService.ResetDailyAccessForSingleStudentAsync(studentId);
            return success ? NoContent() : NotFound();
        }

        // PUT: api/CafeAccess/resetall (Manual run of the daily reset job)
        [HttpPut("resetall")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> ResetAllAccess()
        {
            var count = await _cafeAccessService.ResetAllDailyAccessAsync();
            return Ok(new { Message = $"Successfully reset daily access for {count} records.", Count = count });
        }
    }
}