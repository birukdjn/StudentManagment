using Microsoft.AspNetCore.Mvc;
using StudentManagment.DTOs.Courses;
using StudentManagment.Services.Interfaces;

namespace StudentManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController(ICourseService courseService) : ControllerBase
    {
        private readonly ICourseService _courseService = courseService;

        // POST: api/Course
        [HttpPost]
        public async Task<ActionResult<ReadCourseDto>> CreateCourse(CreateCourseDto dto)
        {
            try
            {
                var readDto = await _courseService.CreateCourseAsync(dto);
                return CreatedAtAction(nameof(GetCourse), new { id = readDto.CourseId }, readDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Course
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadCourseDto>>> GetAllActiveCourses()
        {
            var courses = await _courseService.GetAllActiveCoursesAsync();
            return Ok(courses);
        }

       
        [HttpGet("archived")]
        public async Task<ActionResult<IEnumerable<ReadCourseDto>>>GetArchivedCoursesAsync()
        {
            var courses = await _courseService.GetArchivedCoursesAsync();
            return Ok(courses);

        }

        // GET: api/Course/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReadCourseDto>> GetCourse(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return Ok(course);
        }

        // PUT: api/Course/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, UpdateCourseDto dto)
        {
            if (id != dto.CourseId)
            {
                return BadRequest("Course ID mismatch.");
            }

            try
            {
                var success = await _courseService.UpdateCourseAsync(id, dto);

                if (!success)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // PUT (Soft Delete): api/Course/archive/5
        [HttpPut("archive/{id}")]
        public async Task<IActionResult> ArchiveCourse(int id)
        {
            var success = await _courseService.ArchiveCourseAsync(id);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        // PUT (Restore): api/Course/restore/5
        [HttpPut("restore/{id}")]
        public async Task<IActionResult> RestoreCourse(int id)
        {
            var success = await _courseService.RestoreCourseAsync(id);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}