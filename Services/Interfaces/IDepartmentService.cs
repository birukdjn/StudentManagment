using StudentManagment.DTOs.Departments;

namespace StudentManagment.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<ReadDepartmentDto> CreateDepartmentAsync(CreateDepartmentDto departmentDto);
        Task<ReadDepartmentDto?> GetDepartmentByIdAsync(int id);
        Task<IEnumerable<ReadDepartmentDto>> GetAllActiveDepartmentsAsync();
        Task<IEnumerable<ReadDepartmentDto>> GetAllArchivedDepartmentsAsync();
        Task<bool> UpdateDepartmentAsync(int id, UpdateDepartmentDto departmentDto);

        Task<bool> ArchiveDepartmentAsync(int id);
        Task<bool> RestoreDepartmentAsync(int id);
    }
}