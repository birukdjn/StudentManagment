using StudentManagment.DTOs.Courses;

namespace StudentManagment.Services.Interfaces
{
    public interface ICourseService
    {
        // CRUD Operations
        Task<ReadCourseDto> CreateCourseAsync(CreateCourseDto courseDto);
        Task<ReadCourseDto?> GetCourseByIdAsync(int id);
        Task<IEnumerable<ReadCourseDto>> GetAllActiveCoursesAsync();
        Task<IEnumerable<ReadCourseDto>> GetArchivedCoursesAsync();
        
        Task<bool> UpdateCourseAsync(int id, UpdateCourseDto courseDto);

        // Soft Deletion/Restoration
        Task<bool> ArchiveCourseAsync(int id);
        Task<bool> RestoreCourseAsync(int id);
    }
}