using StudentManagment.DTOs.Enrollments;

namespace StudentManagment.Services.Interfaces
{
    public interface IEnrollmentService
    {
        // CRUD Operations
        Task<ReadEnrollmentDto> CreateEnrollmentAsync(CreateEnrollmentDto enrollmentDto);
        Task<ReadEnrollmentDto?> GetEnrollmentByIdAsync(int id);
        Task<IEnumerable<ReadEnrollmentDto>> GetAllEnrollmentsAsync();
        Task<bool> ActivateEnrollmentAsync(int id);

        Task<bool> UpdateEnrollmentAsync(int id, UpdateEnrollmentDto enrollmentDto);

        Task<bool> WithdrawCourseAsync(int id, string withdrawalCode);
        Task<bool> DeleteEnrollmentRecordAsync(int id);
    }
}