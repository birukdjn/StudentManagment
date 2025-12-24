using StudentManagment.DTOs.CafeAccess;

namespace StudentManagment.Services.Interfaces
{
    public interface ICafeAccessService
    {
        Task<ReadCafeAccessDto> CreateCafeAccessAsync(CreateCafeAccessDto dto);
        Task<ReadCafeAccessDto?> GetCafeAccessByStudentIdAsync(int studentId);
        Task<ReadCafeAccessDto?> GetCafeAccessByScannableIdAsync(string scannableIdCode);
        Task<IEnumerable<ReadCafeAccessDto>> GetAllCafeAccessRecordsAsync();

        // Core Access Logic
        Task<CafeAccessResponseDto> AttemptAccessAsync(CafeScanRequestDto dto);

        // Utility/Maintenance
        Task<bool> ResetDailyAccessForSingleStudentAsync(int studentId);
        Task<int> ResetAllDailyAccessAsync();
    }
}
