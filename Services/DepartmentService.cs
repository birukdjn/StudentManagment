using Microsoft.EntityFrameworkCore;
using StudentManagment.Data;
using StudentManagment.DTOs.Departments;
using StudentManagment.Models;
using StudentManagment.Services.Interfaces;

namespace StudentManagment.Services
{
    public class DepartmentService(SchoolContext context) : IDepartmentService
    {
        private readonly SchoolContext _context = context;

        // --- CREATE ---
        public async Task<ReadDepartmentDto> CreateDepartmentAsync(CreateDepartmentDto dto)
        {
            var department = new Department
            {
                Name = dto.Name,
            };

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return new ReadDepartmentDto
            {
                DepartmentId = department.DepartmentId,
                Name = department.Name,
                IsActive = department.IsActive
            };
        }

        // --- READ ---
        public async Task<ReadDepartmentDto?> GetDepartmentByIdAsync(int id)
        {
            var department = await _context.Departments
                .Where(d => d.DepartmentId == id)
                .Select(d => new ReadDepartmentDto
                {
                    DepartmentId = d.DepartmentId,
                    Name = d.Name,
                    IsActive = d.IsActive
                })
                .FirstOrDefaultAsync();

            return department;
        }

        // --- READ ALL (Active Only) ---
        public async Task<IEnumerable<ReadDepartmentDto>> GetAllActiveDepartmentsAsync()
        {
            var departments = await _context.Departments
                .Where(d => d.IsActive)
                .Select(d => new ReadDepartmentDto
                {
                    DepartmentId = d.DepartmentId,
                    Name = d.Name,
                    IsActive = d.IsActive
                })
                .ToListAsync();

            return departments;
        }
        public async Task<IEnumerable<ReadDepartmentDto>> GetAllArchivedDepartmentsAsync()
        {
            var departments = await _context.Departments
                .Where(d =>!d.IsActive)
                .Select(d => new ReadDepartmentDto
                {
                    DepartmentId = d.DepartmentId,
                    Name = d.Name,
                    IsActive = d.IsActive
                })
                .ToListAsync();

            return departments;
        }

        // --- UPDATE ---
        public async Task<bool> UpdateDepartmentAsync(int id, UpdateDepartmentDto dto)
        {
            var departmentToUpdate = await _context.Departments.FindAsync(id);

            if (departmentToUpdate == null)
            {
                return false;
            }

            // Update fields from DTO
            departmentToUpdate.Name = dto.Name;
            departmentToUpdate.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();
            return true;
        }

        // --- SOFT DELETE / ARCHIVE ---
        public async Task<bool> ArchiveDepartmentAsync(int id)
        {
            var departmentToArchive = await _context.Departments.FindAsync(id);

            if (departmentToArchive == null || !departmentToArchive.IsActive)
            {
                return false;
            }

            // Set the IsActive flag to false (Soft Delete)
            departmentToArchive.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        // --- RESTORE ---
        public async Task<bool> RestoreDepartmentAsync(int id)
        {
            var departmentToRestore = await _context.Departments.FindAsync(id);

            if (departmentToRestore == null || departmentToRestore.IsActive)
            {
                return false;
            }

            // Set the IsActive flag back to true
            departmentToRestore.IsActive = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}