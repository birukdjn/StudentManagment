using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using StudentManagment.Enums;
using StudentManagment.Models;

namespace StudentManagment.Data
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            // --- 1. Hashing Passwords ---

            var staticCreationDate = new DateTime(2024, 1, 1, 10, 0, 0, DateTimeKind.Utc);
            // These hashes are needed for the initial users to log in securely.
            var adminHash = BCrypt.Net.BCrypt.HashPassword("Admin123!");
            var teacherHash = BCrypt.Net.BCrypt.HashPassword("Teacher123!");
            var studentHash = BCrypt.Net.BCrypt.HashPassword("Student123!");

            // --- 2. Departments ---
           
            modelBuilder.Entity<Department>().HasData(
                new Department { DepartmentId = 1, Name = "Computer Science" },
                new Department { DepartmentId = 2, Name = "Applied Mathematics" },
                new Department { DepartmentId = 3, Name = "English Literature" },
                new Department { DepartmentId = 4, Name = "Physics" },
                new Department { DepartmentId = 5, Name = "Business Administration" }
            );

            // --- 3. Users (Admin, Teacher, Student) ---
            modelBuilder.Entity<User>().HasData(
                // ADMIN USER (ID 1)
                new User
                {
                    UserId = 1,
                    FirstName = "System",
                    LastName = "Admin",
                    Gender = GenderEnum.Male,
                    DateOfBirth = new DateOnly(1980, 1, 1),
                    Phone = "1234567890",
                    Username = "sys.admin",
                    Role = UserRoleEnum.Admin,
                    Email = "admin@school.edu",
                    PasswordHash = adminHash,
                    IsActive = true,
                    CreatedAt = staticCreationDate
                },
                // TEACHER USER (ID 2)
                new User
                {
                    UserId = 2,
                    FirstName = "Alice",
                    LastName = "Smith",
                    Gender = GenderEnum.Female,
                    DateOfBirth = new DateOnly(1985, 5, 20),
                    Phone = "0987654321",
                    Username = "a.smith",
                    Role = UserRoleEnum.Teacher,
                    Email = "alice.smith@school.edu",
                    PasswordHash = teacherHash,
                    IsActive = true,
                    CreatedAt = staticCreationDate
                },
                // STUDENT USER (ID 3)

                new User
                {
                    UserId = 10,
                    FirstName = "Robert",
                    LastName = "Chen",
                    Gender = GenderEnum.Male,
                    DateOfBirth = new DateOnly(1978, 3, 15),
                    Phone = "5551234567",
                    Username = "r.chen",
                    Role = UserRoleEnum.Teacher,
                    Email = "robert.chen@school.edu",
                    PasswordHash = teacherHash,
                    IsActive = true,
                    CreatedAt = staticCreationDate
                },
                // STUDENT USERS
                new User
                {
                    UserId = 3,
                    FirstName = "Bob",
                    LastName = "Johnson",
                    Gender = GenderEnum.Male,
                    DateOfBirth = new DateOnly(2002, 9, 15),
                    Phone = "1122334455",
                    Username = "b.johnson",
                    Role = UserRoleEnum.Student,
                    Email = "bob.johnson@school.edu",
                    PasswordHash = studentHash,
                    IsActive = true,
                    CreatedAt = staticCreationDate
                },
                new User
                {
                    UserId = 4,
                    FirstName = "Sarah",
                    LastName = "Williams",
                    Gender = GenderEnum.Female,
                    DateOfBirth = new DateOnly(2003, 2, 28),
                    Phone = "2233445566",
                    Username = "s.williams",
                    Role = UserRoleEnum.Student,
                    Email = "sarah.williams@school.edu",
                    PasswordHash = studentHash,
                    IsActive = true,
                    CreatedAt = staticCreationDate
                },
                new User
                {
                    UserId = 5,
                    FirstName = "Michael",
                    LastName = "Brown",
                    Gender = GenderEnum.Male,
                    DateOfBirth = new DateOnly(2002, 7, 10),
                    Phone = "3344556677",
                    Username = "m.brown",
                    Role = UserRoleEnum.Student,
                    Email = "michael.brown@school.edu",
                    PasswordHash = studentHash,
                    IsActive = true,
                    CreatedAt = staticCreationDate
                },
                new User
                {
                    UserId = 6,
                    FirstName = "Emma",
                    LastName = "Davis",
                    Gender = GenderEnum.Female,
                    DateOfBirth = new DateOnly(2003, 11, 5),
                    Phone = "4455667788",
                    Username = "e.davis",
                    Role = UserRoleEnum.Student,
                    Email = "emma.davis@school.edu",
                    PasswordHash = studentHash,
                    IsActive = true,
                    CreatedAt = staticCreationDate
                },
                new User
                {
                    UserId = 7,
                    FirstName = "James",
                    LastName = "Miller",
                    Gender = GenderEnum.Male,
                    DateOfBirth = new DateOnly(2002, 4, 22),
                    Phone = "5566778899",
                    Username = "j.miller",
                    Role = UserRoleEnum.Student,
                    Email = "james.miller@school.edu",
                    PasswordHash = studentHash,
                    IsActive = true,
                    CreatedAt = staticCreationDate
                },
                new User
                {
                    UserId = 8,
                    FirstName = "Olivia",
                    LastName = "Wilson",
                    Gender = GenderEnum.Female,
                    DateOfBirth = new DateOnly(2003, 8, 30),
                    Phone = "6677889900",
                    Username = "o.wilson",
                    Role = UserRoleEnum.Student,
                    Email = "olivia.wilson@school.edu",
                    PasswordHash = studentHash,
                    IsActive = true,
                    CreatedAt = staticCreationDate
                },
                new User
                {
                    UserId = 9,
                    FirstName = "David",
                    LastName = "Taylor",
                    Gender = GenderEnum.Male,
                    DateOfBirth = new DateOnly(2002, 12, 18),
                    Phone = "7788990011",
                    Username = "d.taylor",
                    Role = UserRoleEnum.Student,
                    Email = "david.taylor@school.edu",
                    PasswordHash = studentHash,
                    IsActive = true,
                    CreatedAt = staticCreationDate
                }

            );

            // --- 4. Teachers (Linked to User ID 2) ---
            modelBuilder.Entity<Teacher>().HasData(
               
            new Teacher
            {
                TeacherId = 1,
                UserId = 2, // Alice Smith
                DepartmentId = 1,
                HireDate = new DateOnly(2010, 8, 1),
                
            },
                new Teacher
                {
                    TeacherId = 2,
                    UserId = 10, // Robert Chen
                    DepartmentId = 2,
                    HireDate = new DateOnly(2015, 1, 15),
                    
                }
            );


            // --- 5. Students (Linked to User ID 3) ---
            modelBuilder.Entity<Student>().HasData(
                new Student
                {
                    StudentId = 1,
                    UserId = 3, // Bob Johnson
                    DepartmentId = 1, // Computer Science
                    EnrollmentDate = new DateOnly(2021, 9, 1),
                    GPA = 3.5,
                },
                new Student
                {
                    StudentId = 2,
                    UserId = 4, // Sarah Williams
                    DepartmentId = 1, // Computer Science
                    EnrollmentDate = new DateOnly(2021, 9, 1),
                    GPA = 3.8,
                },
                new Student
                {
                    StudentId = 3,
                    UserId = 5, // Michael Brown
                    DepartmentId = 2, // Applied Mathematics
                    EnrollmentDate = new DateOnly(2022, 9, 1),
                    GPA = 3.2,
                },
                new Student
                {
                    StudentId = 4,
                    UserId = 6, // Emma Davis
                    DepartmentId = 3, // English Literature
                    EnrollmentDate = new DateOnly(2022, 9, 1),
                    GPA = 3.9,
                },
                new Student
                {
                    StudentId = 5,
                    UserId = 7, // James Miller
                    DepartmentId = 4, // Physics
                    EnrollmentDate = new DateOnly(2023, 9, 1),
                    GPA = 3.6,
                },
                new Student
                {
                    StudentId = 6,
                    UserId = 8, // Olivia Wilson
                    DepartmentId = 5, // Business Administration
                    EnrollmentDate = new DateOnly(2023, 9, 1),
                    GPA = 3.4,
                },
                new Student
                {
                    StudentId = 7,
                    UserId = 9, // David Taylor
                    DepartmentId = 1, // Computer Science
                    EnrollmentDate = new DateOnly(2021, 9, 1),
                    GPA = 3.7,
                }      
            );

            // --- 6. Cafe Access (Linked to Student ID 1) ---
            modelBuilder.Entity<CafeAccess>().HasData(
                new CafeAccess
                {
                    CafeAccessId = 1,
                    StudentId = 1,
                    ScannableIdCode = "8C:3C:CE:44",
                },
                new CafeAccess
                {
                    CafeAccessId = 2,
                    StudentId = 2,
                    ScannableIdCode = "F9:CE:D9:9B",
                },
                new CafeAccess
                {
                    CafeAccessId = 3,
                    StudentId = 3,
                    ScannableIdCode = "C6:65:D3:EA",
                },
                new CafeAccess
                {
                    CafeAccessId = 4,
                    StudentId = 4,
                    ScannableIdCode = "79:68:DB:9B",
                },
                new CafeAccess
                {
                    CafeAccessId = 5,
                    StudentId = 5,
                    ScannableIdCode = "67:EC:4B:84",
                },
                new CafeAccess
                {
                    CafeAccessId = 6,
                    StudentId = 6,
                    ScannableIdCode = "59:3B:3D:41",

                },
                new CafeAccess
                {
                    CafeAccessId = 7,
                    StudentId = 7,
                    ScannableIdCode = "55:03:FE:EC",

                }

            );

            // --- 7. Courses ---
            modelBuilder.Entity<Course>().HasData(
                 new Course { CourseId = 1, DepartmentId = 1, Code = "CS101", Title = "Introduction to Programming", Credits = 3 },
                 new Course { CourseId = 2, DepartmentId = 1, Code = "CS201", Title = "Data Structures", Credits = 4 },
                 new Course { CourseId = 3, DepartmentId = 1, Code = "CS301", Title = "Algorithms", Credits = 4 },
                 new Course { CourseId = 4, DepartmentId = 2, Code = "MATH101", Title = "Calculus I", Credits = 3 },
                 new Course { CourseId = 5, DepartmentId = 2, Code = "MATH201", Title = "Linear Algebra", Credits = 3 },
                 new Course { CourseId = 6, DepartmentId = 3, Code = "ENG101", Title = "English Composition", Credits = 3 },
                 new Course { CourseId = 7, DepartmentId = 4, Code = "PHY101", Title = "Physics I", Credits = 4 },
                 new Course { CourseId = 8, DepartmentId = 5, Code = "BUS101", Title = "Introduction to Business", Credits = 3 }
             );


            // --- 8. Enrollments (Student 1 in CS101) ---
            modelBuilder.Entity<Enrollment>().HasData(
                 new Enrollment
                 {
                     EnrollmentId = 1,
                     StudentId = 1,
                     CourseId = 1,
                     Year = 2024,
                     Semester = SemesterEnum.Semester1,
                     Grade = "A"
                 },
                 new Enrollment
                 {
                     EnrollmentId = 2,
                     StudentId = 1,
                     CourseId = 2,
                     Year = 2024,
                     Semester = SemesterEnum.Semester1,
                     Grade = "B+"
                 },
                 new Enrollment
                 {
                     EnrollmentId = 3,
                     StudentId = 2,
                     CourseId = 1,
                     Year = 2024,
                     Semester = SemesterEnum.Semester1,
                     Grade = "A-"
                 },
                 new Enrollment
                 {
                     EnrollmentId = 4,
                     StudentId = 3,
                     CourseId = 4,
                     Year = 2024,
                     Semester = SemesterEnum.Semester1,
                     Grade = "B"
                 },
                 new Enrollment
                 {
                     EnrollmentId = 5,
                     StudentId = 4,
                     CourseId = 6,
                     Year = 2024,
                     Semester = SemesterEnum.Semester1,
                     Grade = "A"
                 },
                 new Enrollment
                 {
                     EnrollmentId = 6,
                     StudentId = 7,
                     CourseId = 1,
                     Year = 2024,
                     Semester = SemesterEnum.Semester1,
                     Grade = "A"
                 }
             );

            // --- 9. Course Assignment (Teacher 1 teaches CS101) ---
            modelBuilder.Entity<CourseAssignment>().HasData(
                new CourseAssignment
                {
                    CourseAssignmentId = 1,
                    CourseId = 1,
                    TeacherId = 1,
                    Year = 2024,
                    Semester = SemesterEnum.Semester1
                },
                new CourseAssignment
                {
                    CourseAssignmentId = 2,
                    CourseId = 2,
                    TeacherId = 1,
                    Year = 2024,
                    Semester = SemesterEnum.Semester1
                },
                new CourseAssignment
                {
                    CourseAssignmentId = 3,
                    CourseId = 3,
                    TeacherId = 1,
                    Year = 2024,
                    Semester = SemesterEnum.Semester2
                },
                new CourseAssignment
                {
                    CourseAssignmentId = 4,
                    CourseId = 4,
                    TeacherId = 2,
                    Year = 2024,
                    Semester = SemesterEnum.Semester1
                },
                new CourseAssignment
                {
                    CourseAssignmentId = 5,
                    CourseId = 5,
                    TeacherId = 2,
                    Year = 2024,
                    Semester = SemesterEnum.Semester2
                }
            );

        }
    }
}