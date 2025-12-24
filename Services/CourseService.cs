using Microsoft.EntityFrameworkCore;
using StudentManagment.Data;
using StudentManagment.DTOs.Courses;
using StudentManagment.Models;
using StudentManagment.Services.Interfaces;

namespace StudentManagment.Services
{
    public class CourseService(SchoolContext context) : ICourseService
    {
        private readonly SchoolContext _context = context;

        // --- CREATE ---
        public async Task<ReadCourseDto> CreateCourseAsync(CreateCourseDto dto)
        {
            var course = new Course
            {
                Code = dto.Code,
                Title = dto.Title,
                Credits = dto.Credits,
                DepartmentId = dto.DepartmentId,
                IsActive = true
            };

            var department = await _context.Departments.FindAsync(dto.DepartmentId) ?? throw new ArgumentException("Invalid Department ID.");

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return new ReadCourseDto
            {
                CourseId = course.CourseId,
                Code = course.Code,
                Title = course.Title,
                Credits = course.Credits,
                DepartmentName = department.Name,
                IsActive = course.IsActive
            };
        }

        public async Task<ReadCourseDto?> GetCourseByIdAsync(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Department)
                .Where(c => c.CourseId == id)
                .Select(c => new ReadCourseDto
                {
                    CourseId = c.CourseId,
                    Code = c.Code,
                    Title = c.Title,
                    Credits = c.Credits,
                    DepartmentName = c.Department.Name
                })
                .FirstOrDefaultAsync();

            return course;
        }

        // --- READ ALL (Active Only) ---
        public async Task<IEnumerable<ReadCourseDto>> GetAllActiveCoursesAsync()
        {
            var courses = await _context.Courses
                .Where(c => c.IsActive) 
                .Include(c => c.Department)
                .Select(c => new ReadCourseDto
                {
                    CourseId = c.CourseId,
                    Code = c.Code,
                    Title = c.Title,
                    Credits = c.Credits,
                    DepartmentName = c.Department.Name,
                    IsActive = c.IsActive
                })
                .ToListAsync();

            return courses;
        }

        public async Task<IEnumerable<ReadCourseDto>> GetArchivedCoursesAsync()
        {
            var courses = await _context.Courses
                .Where(c => !c.IsActive)
                .Include(c => c.Department)
                .Select(c => new ReadCourseDto
                {
                    CourseId = c.CourseId,
                    Code = c.Code,
                    Title = c.Title,
                    Credits = c.Credits,
                    DepartmentName = c.Department.Name,
                    IsActive = c.IsActive
                })
                .ToListAsync();
            return courses;
        }

        // --- UPDATE ---
        public async Task<bool> UpdateCourseAsync(int id, UpdateCourseDto dto)
        {
            var courseToUpdate = await _context.Courses.FindAsync(id);

            if (courseToUpdate == null)
            {
                return false;
            }

            if (dto.DepartmentId != courseToUpdate.DepartmentId)
            {
                var newDepartment = await _context.Departments.FindAsync(dto.DepartmentId) ?? throw new ArgumentException("Invalid Department ID provided for update.");
            }

            courseToUpdate.Code = dto.Code;
            courseToUpdate.Title = dto.Title;
            courseToUpdate.Credits = dto.Credits;
            courseToUpdate.DepartmentId = dto.DepartmentId;
            courseToUpdate.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();
            return true;
        }

        // --- SOFT DELETE / ARCHIVE ---
        public async Task<bool> ArchiveCourseAsync(int id)
        {
            var courseToArchive = await _context.Courses.FindAsync(id);

            if (courseToArchive == null || !courseToArchive.IsActive)
            {
                return false;
            }

            courseToArchive.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        // --- RESTORE ---
        public async Task<bool> RestoreCourseAsync(int id)
        {
            var courseToRestore = await _context.Courses.FindAsync(id);

            if (courseToRestore == null || courseToRestore.IsActive)
            {
                return false;
            }

            courseToRestore.IsActive = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}