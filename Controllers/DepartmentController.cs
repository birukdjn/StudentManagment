using Microsoft.AspNetCore.Mvc;
using StudentManagment.DTOs.Departments;
using StudentManagment.Services.Interfaces;

namespace StudentManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController(IDepartmentService departmentService) : ControllerBase
    {
        private readonly IDepartmentService _departmentService = departmentService;

        [HttpPost]
        public async Task<ActionResult<ReadDepartmentDto>> CreateDepartment(CreateDepartmentDto dto)
        {
            var readDto = await _departmentService.CreateDepartmentAsync(dto);
            // Returns 201 Created status
            return CreatedAtAction(nameof(GetDepartment), new { id = readDto.DepartmentId }, readDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReadDepartmentDto>> GetDepartment(int id)
        {
            var department = await _departmentService.GetDepartmentByIdAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            return Ok(department);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadDepartmentDto>>> GetAllActiveDepartments()
        {
            var departments = await _departmentService.GetAllActiveDepartmentsAsync();
            return Ok(departments);
        }
        [HttpGet("Archived")]
        public async Task<ActionResult<IEnumerable<ReadDepartmentDto>>> GetAllArchivedDepartments()
        {
            var departments = await _departmentService.GetAllArchivedDepartmentsAsync();
            return Ok(departments);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, UpdateDepartmentDto dto)
        {
            if (id != dto.DepartmentId)
            {
                return BadRequest("Department ID mismatch.");
            }

            var success = await _departmentService.UpdateDepartmentAsync(id, dto);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPut("archive/{id}")]
        public async Task<IActionResult> ArchiveDepartment(int id)
        {
            var success = await _departmentService.ArchiveDepartmentAsync(id);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPut("restore/{id}")]
        public async Task<IActionResult> RestoreDepartment(int id)
        {
            var success = await _departmentService.RestoreDepartmentAsync(id);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}