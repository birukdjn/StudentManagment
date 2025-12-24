using StudentManagment.Data;
using StudentManagment.DTOs.CafeAccess;
using StudentManagment.Models;
using StudentManagment.Services.Interfaces;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace StudentManagment.Services
{
    public class CafeAccessService(SchoolContext context) : ICafeAccessService
    {
        private readonly SchoolContext _context = context;
        // Business Rules for meal times (can be moved to configuration)
        private static readonly TimeSpan BreakfastEnd = new(8, 30, 0); // 8:30
        private static readonly TimeSpan LunchEnd = new(12, 30, 0);   // 12:30
        private static readonly TimeSpan DinnerEnd = new(17, 30, 0);  // 5:30 
        private const int MaxAccessPerDay = 3;

        // Helper function to map Model to Read DTO
        private static ReadCafeAccessDto MapToReadDto(CafeAccess ca)
        {
            return new ReadCafeAccessDto
            {
                CafeAccessId = ca.CafeAccessId,
                StudentId = ca.StudentId,
                StudentName = $"{ca.Student.User?.FirstName} {ca.Student.User?.LastName}",
                ScannableIdCode = ca.ScannableIdCode,
                HasAccessedBreakfast = ca.HasAccessedBreakfast,
                HasAccessedLunch = ca.HasAccessedLunch,
                HasAccessedDinner = ca.HasAccessedDinner,
                LastResetDate = ca.LastResetDate,
                TotalDailyAccesses = ca.TotalDailyAccesses
            };
        }

        public async Task<ReadCafeAccessDto> CreateCafeAccessAsync(CreateCafeAccessDto dto)
        {
            // 1. Ensure Student exists and load related data for validation
            var student = await _context.Students
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.StudentId == dto.StudentId)
                ?? throw new KeyNotFoundException($"Student with ID {dto.StudentId} not found.");

            // 2. Check for uniqueness of ScannableIdCode
            if (await _context.CafeAccesses.AnyAsync(ca => ca.ScannableIdCode == dto.ScannableIdCode))
            {
                throw new InvalidOperationException($"Scannable ID Code '{dto.ScannableIdCode}' is already assigned.");
            }

            // 3. Create the new entity
            var newAccess = new CafeAccess
            {
                StudentId = dto.StudentId,
                ScannableIdCode = dto.ScannableIdCode,
                Student = student
            };

            var validationResults = newAccess.Validate(new ValidationContext(newAccess)).ToList();
            if (validationResults.Count > 0)
            {
                var message = string.Join("; ", validationResults.Select(v => v.ErrorMessage));
                throw new InvalidOperationException($"Cannot create CafeAccess record for this student: {message}");
            }

            _context.CafeAccesses.Add(newAccess);
            await _context.SaveChangesAsync();

            // Return DTO
            return MapToReadDto(newAccess);
        }

        public async Task<ReadCafeAccessDto?> GetCafeAccessByStudentIdAsync(int studentId)
        {
            var cafeAccess = await _context.CafeAccesses
                .Include(ca => ca.Student)
                .ThenInclude(s => s.User)
                .FirstOrDefaultAsync(ca => ca.StudentId == studentId);

            return cafeAccess == null ? null : MapToReadDto(cafeAccess);
        }

        public async Task<ReadCafeAccessDto?> GetCafeAccessByScannableIdAsync(string scannableIdCode)
        {
            var cafeAccess = await _context.CafeAccesses
                .Include(ca => ca.Student)
                .ThenInclude(s => s.User)
                .FirstOrDefaultAsync(ca => ca.ScannableIdCode == scannableIdCode);

            return cafeAccess == null ? null : MapToReadDto(cafeAccess);
        }

        public async Task<IEnumerable<ReadCafeAccessDto>> GetAllCafeAccessRecordsAsync()
        {
            return await _context.CafeAccesses
                .Include(ca => ca.Student)
                .ThenInclude(s => s.User)
                .Select(ca => MapToReadDto(ca))
                .ToListAsync();
        }

        public async Task<CafeAccessResponseDto> AttemptAccessAsync(CafeScanRequestDto dto)
        {
            var now = DateTime.UtcNow;
            var today = now.Date;
            var currentTime = now.TimeOfDay;

            var accessRecord = await _context.CafeAccesses
                .Include(ca => ca.Student)
                .ThenInclude(s => s.User)
                .FirstOrDefaultAsync(ca => ca.ScannableIdCode == dto.ScannableIdCode);

            if (accessRecord == null)
            {
                return new CafeAccessResponseDto { AccessGranted = false, Message = "Invalid ID Code. Access denied." };
            }

            // 1. Daily Reset Check
            if (accessRecord.LastResetDate < today)
            {
                await ResetDailyAccessForSingleStudentAsync(accessRecord.StudentId);
                // Reload record after reset
                accessRecord = await _context.CafeAccesses.FindAsync(accessRecord.CafeAccessId);
                if (accessRecord == null) // Should not happen after find
                {
                    return new CafeAccessResponseDto { AccessGranted = false, Message = "System Error during reset. Access denied." };
                }
            }

            // 2. Maximum Access Check
            if (accessRecord.TotalDailyAccesses >= MaxAccessPerDay)
            {
                return new CafeAccessResponseDto { AccessGranted = false, Message = $"Maximum daily access limit ({MaxAccessPerDay}) reached. Access denied." };
            }

            string grantedMeal;
            bool alreadyAccessed;

            // 3. Determine current meal period and check status
            if (currentTime < BreakfastEnd)
            {
                grantedMeal = "Breakfast";
                alreadyAccessed = accessRecord.HasAccessedBreakfast;
                accessRecord.HasAccessedBreakfast = true;
            }
            else if (currentTime < LunchEnd)
            {
                grantedMeal = "Lunch";
                alreadyAccessed = accessRecord.HasAccessedLunch;
                accessRecord.HasAccessedLunch = true;
            }
            else if (currentTime < DinnerEnd)
            {
                grantedMeal = "Dinner";
                alreadyAccessed = accessRecord.HasAccessedDinner;
                accessRecord.HasAccessedDinner = true;
            }
            else
            {
                return new CafeAccessResponseDto { AccessGranted = false, Message = "The cafeteria is currently closed." };
            }

            // 4. Check if already accessed this meal
            if (alreadyAccessed)
            {
                return new CafeAccessResponseDto { AccessGranted = false, Message = $"Access denied. {accessRecord.Student.User?.FirstName} has already accessed {grantedMeal} today." };
            }

            // 5. Grant access and save changes
            accessRecord.TotalDailyAccesses++;
            accessRecord.LastResetDate = today;
            await _context.SaveChangesAsync();

            return new CafeAccessResponseDto
            {
                AccessGranted = true,
                Message = $"Welcome {accessRecord.Student.User?.FirstName}! Access granted for {grantedMeal}.",
                StudentName = $"{accessRecord.Student.User?.FirstName} {accessRecord.Student.User?.LastName}",
                StudentId = accessRecord.StudentId,
                GrantedMeal = grantedMeal
            };
        }

        public async Task<bool> ResetDailyAccessForSingleStudentAsync(int studentId)
        {
            var accessRecord = await _context.CafeAccesses.FirstOrDefaultAsync(ca => ca.StudentId == studentId);
            if (accessRecord == null) return false;

            accessRecord.HasAccessedBreakfast = false;
            accessRecord.HasAccessedLunch = false;
            accessRecord.HasAccessedDinner = false;
            accessRecord.TotalDailyAccesses = 0;
            accessRecord.LastResetDate = DateTime.UtcNow.Date;

            await _context.SaveChangesAsync();
            return true;
        }

        // Simulates a nightly job to reset all records
        public async Task<int> ResetAllDailyAccessAsync()
        {
            var today = DateTime.UtcNow.Date;

            // Find records that haven't been reset today
            var recordsToReset = await _context.CafeAccesses
                .Where(ca => ca.LastResetDate < today)
                .ToListAsync();

            foreach (var record in recordsToReset)
            {
                record.HasAccessedBreakfast = false;
                record.HasAccessedLunch = false;
                record.HasAccessedDinner = false;
                record.TotalDailyAccesses = 0;
                record.LastResetDate = today;
            }

            await _context.SaveChangesAsync();
            return recordsToReset.Count;
        }
    }
}