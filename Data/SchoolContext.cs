using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using StudentManagment.Enums;
using StudentManagment.Models;

namespace StudentManagment.Data
{
    public class SchoolContext(DbContextOptions<SchoolContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<TeacherDepartmentHistory> TeacherDepartmentHistory { get; set; }
        public DbSet<CafeAccess> CafeAccesses { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<CourseAssignment> CourseAssignments { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<AttendanceRecord> AttendanceRecords { get; set; }




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning)
            );

            // You must always call the base implementation if you override OnConfiguring
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();


            modelBuilder.Entity<User>()
                .Property(u => u.Gender)
                .HasConversion<string>();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<Student>()
                .HasOne(s => s.User)
                .WithOne(u => u.Student)
                .HasForeignKey<Student>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Department)
                .WithMany(d => d.Students)
                .HasForeignKey(s => s.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Teacher>()
                .HasOne(t => t.User)
                .WithOne(u => u.Teacher)
                .HasForeignKey<Teacher>(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Teacher>()
                .HasOne(t => t.Department)
                .WithMany(d => d.Teachers)
                .HasForeignKey(t => t.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CafeAccess>()
                .HasOne(ca => ca.Student)
                .WithOne(s => s.CafeAccess)
                .HasForeignKey<CafeAccess>(ca => ca.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CafeAccess>()
                .HasIndex(ca => ca.ScannableIdCode)
                .IsUnique();

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentId);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId);

            modelBuilder.Entity<Enrollment>()
                .HasIndex(e => new { e.StudentId, e.CourseId, e.Year, e.Semester })
                .IsUnique();

            modelBuilder.Entity<Enrollment>()
                .Property(e => e.Semester)
                .HasConversion<string>();

            modelBuilder.Entity<CourseAssignment>()
                .HasOne(ca => ca.Teacher)
                .WithMany(t => t.CourseAssignments)
                .HasForeignKey(ca => ca.TeacherId);

            modelBuilder.Entity<CourseAssignment>()
                .HasOne(ca => ca.Course)
                .WithMany(c => c.CourseAssignments)
                .HasForeignKey(ca => ca.CourseId);

            modelBuilder.Seed();


        }

    }
}
