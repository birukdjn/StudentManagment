using Microsoft.EntityFrameworkCore;
using StudentManagment.Data;
using StudentManagment.DTOs.Enrollments;
using StudentManagment.Models;
using StudentManagment.Services.Interfaces;

namespace StudentManagment.Services
{
    public class EnrollmentService(SchoolContext context) : IEnrollmentService
    {
        private readonly SchoolContext _context = context;

        // --- CREATE ---
        public async Task<ReadEnrollmentDto> CreateEnrollmentAsync(CreateEnrollmentDto dto)
        {
            var student = await _context.Students
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.StudentId == dto.StudentId);
            var course = await _context.Courses.FindAsync(dto.CourseId);

            if (student == null) throw new ArgumentException("Invalid Student ID.");
            if (course == null) throw new ArgumentException("Invalid Course ID.");

            var exists = await _context.Enrollments.AnyAsync(e =>
                e.StudentId == dto.StudentId &&
                e.CourseId == dto.CourseId &&
                e.Year == dto.Year &&
                e.Semester == dto.Semester);

            if (exists)
            {
                throw new InvalidOperationException("Student is already enrolled in this course for the specified semester and year.");
            }

            var enrollment = new Enrollment
            {
                StudentId = dto.StudentId,
                CourseId = dto.CourseId,
                Year = dto.Year,
                Semester = dto.Semester,
                
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            return new ReadEnrollmentDto
            {
                EnrollmentId = enrollment.EnrollmentId,
                StudentId = student.StudentId,
                StudentName = $"{student.User?.FirstName} {student.User?.LastName}",
                CourseId = course.CourseId,
                CourseTitle = course.Title,
                CourseCode = course.Code,
                Year = enrollment.Year,
                Semester = enrollment.Semester,
                Grade = enrollment.Grade
            };
        }

        // --- READ ---
        public async Task<ReadEnrollmentDto?> GetEnrollmentByIdAsync(int id)
        {
            var enrollment = await _context.Enrollments
                .Include(e => e.Student)
                    .ThenInclude(s => s.User)
                .Include(e => e.Course)
                .Where(e => e.EnrollmentId == id)
                .Select(e => new ReadEnrollmentDto
                {
                    EnrollmentId = e.EnrollmentId,
                    StudentId = e.StudentId,
                    StudentName = $"{e.Student.User.FirstName} {e.Student.User.LastName}",
                    CourseId = e.CourseId,
                    CourseTitle = e.Course.Title,
                    CourseCode = e.Course.Code,
                    Year = e.Year,
                    Semester = e.Semester,
                    Grade = e.Grade
                })
                .FirstOrDefaultAsync();

            return enrollment;
        }

        public async Task<IEnumerable<ReadEnrollmentDto>> GetAllEnrollmentsAsync()
        {
            var enrollments = await _context.Enrollments
                .Include(e => e.Student)
                    .ThenInclude(s => s.User)
                .Include(e => e.Course)
                .Select(e => new ReadEnrollmentDto
                {
                    EnrollmentId = e.EnrollmentId,
                    StudentId = e.StudentId,
                    StudentName = $"{e.Student.User.FirstName} {e.Student.User.LastName}",
                    CourseId = e.CourseId,
                    CourseTitle = e.Course.Title,
                    CourseCode = e.Course.Code,
                    Year = e.Year,
                    Semester = e.Semester,
                    Grade = e.Grade
                })
                .ToListAsync();

            return enrollments;
        }

        public async Task<bool> ActivateEnrollmentAsync(int id)
        {
            var enrollmentToActivate = await _context.Enrollments.FindAsync(id);
            if (enrollmentToActivate == null) return false;

            // Only activate if currently withdrawn
            if (enrollmentToActivate.Grade != "W")
            {
                throw new InvalidOperationException("Enrollment is not withdrawn.");
            }

            // Clear the withdrawal grade (set to null)
            enrollmentToActivate.Grade = null;

            await _context.SaveChangesAsync();
            return true;
        }

        // --- UPDATE / GRADE POSTING ---
        public async Task<bool> UpdateEnrollmentAsync(int id, UpdateEnrollmentDto dto)
        {
            var enrollmentToUpdate = await _context.Enrollments.FindAsync(id);

            if (enrollmentToUpdate == null) return false;

            enrollmentToUpdate.Year = dto.Year;
            enrollmentToUpdate.Semester = dto.Semester;
            enrollmentToUpdate.Grade = dto.Grade;

            await _context.SaveChangesAsync();
            return true;
        }

        // --- WITHDRAWAL (Soft Delete concept) ---
        public async Task<bool> WithdrawCourseAsync(int id, string withdrawalCode = "W")
        {
            var enrollmentToWithdraw = await _context.Enrollments.FindAsync(id);
            if (enrollmentToWithdraw == null) return false;

            // Business logic: Check if a grade is already posted (cannot withdraw if grade is final)
            if (enrollmentToWithdraw.Grade != null && enrollmentToWithdraw.Grade != "I") // "I" for incomplete might be an exception
            {
                throw new InvalidOperationException("Cannot withdraw from course; a final grade has already been posted.");
            }

            // Set the grade to the withdrawal code (e.g., "W")
            enrollmentToWithdraw.Grade = withdrawalCode.ToUpper();

            await _context.SaveChangesAsync();
            return true;
        }

        // --- HARD DELETE (For administrative error only) ---
        public async Task<bool> DeleteEnrollmentRecordAsync(int id)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null) return false;

            // CRITICAL BUSINESS RULE: Only allow deletion if the enrollment is currently active (no grade posted)
            if (enrollment.Grade != null)
            {
                // This prevents deleting an official record that affects a student's transcript/GPA.
                throw new InvalidOperationException("Cannot hard delete enrollment; a grade or withdrawal status has been recorded.");
            }

            // Further checks (e.g., check against add/drop deadline date) would go here.

            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}