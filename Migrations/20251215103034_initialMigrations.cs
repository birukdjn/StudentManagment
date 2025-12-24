using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StudentManagment.Migrations
{
    /// <inheritdoc />
    public partial class initialMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.DepartmentId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Gender = table.Column<string>(type: "longtext", nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    Phone = table.Column<string>(type: "varchar(13)", maxLength: 13, nullable: true),
                    Username = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Role = table.Column<string>(type: "longtext", nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RefreshToken = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    CourseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    Title = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    Credits = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.CourseId);
                    table.ForeignKey(
                        name: "FK_Courses_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    EnrollmentDate = table.Column<DateOnly>(type: "date", nullable: false),
                    GPA = table.Column<double>(type: "double", nullable: false),
                    IsEnrolled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ReceivesCashAllowance = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AccountNumber = table.Column<string>(type: "varchar(255)", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.StudentId);
                    table.ForeignKey(
                        name: "FK_Students_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Students_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    TeacherId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    HireDate = table.Column<DateOnly>(type: "date", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.TeacherId);
                    table.ForeignKey(
                        name: "FK_Teachers_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Teachers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CafeAccesses",
                columns: table => new
                {
                    CafeAccessId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    ScannableIdCode = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    HasAccessedBreakfast = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    HasAccessedLunch = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    HasAccessedDinner = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastResetDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TotalDailyAccesses = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CafeAccesses", x => x.CafeAccessId);
                    table.ForeignKey(
                        name: "FK_CafeAccesses_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Enrollments",
                columns: table => new
                {
                    EnrollmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Semester = table.Column<string>(type: "varchar(255)", nullable: false),
                    Grade = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollments", x => x.EnrollmentId);
                    table.ForeignKey(
                        name: "FK_Enrollments_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enrollments_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CourseAssignments",
                columns: table => new
                {
                    CourseAssignmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Semester = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseAssignments", x => x.CourseAssignmentId);
                    table.ForeignKey(
                        name: "FK_CourseAssignments_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseAssignments_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "TeacherId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherDepartmentHistory",
                columns: table => new
                {
                    HistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherDepartmentHistory", x => x.HistoryId);
                    table.ForeignKey(
                        name: "FK_TeacherDepartmentHistory_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeacherDepartmentHistory_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "TeacherId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "DepartmentId", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, true, "Computer Science" },
                    { 2, true, "Applied Mathematics" },
                    { 3, true, "English Literature" },
                    { 4, true, "Physics" },
                    { 5, true, "Business Administration" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "DateOfBirth", "Email", "FirstName", "Gender", "IsActive", "LastLogin", "LastName", "PasswordHash", "Phone", "RefreshToken", "RefreshTokenExpiryTime", "Role", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), new DateOnly(1980, 1, 1), "admin@school.edu", "System", "Male", true, null, "Admin", "$2a$11$KLk9ZOo5rPwRL8rVqVUS.uLbsVbzYvwynCOChUyQhpnpseuUOU19G", "1234567890", null, null, "Admin", "sys.admin" },
                    { 2, new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), new DateOnly(1985, 5, 20), "alice.smith@school.edu", "Alice", "Female", true, null, "Smith", "$2a$11$pUwviYr0BkvXxrccs3Mz2enXLP582dNuxCvSngXbE5viRlF/SVhMC", "0987654321", null, null, "Teacher", "a.smith" },
                    { 3, new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), new DateOnly(2002, 9, 15), "bob.johnson@school.edu", "Bob", "Male", true, null, "Johnson", "$2a$11$IhfNpX72Ez.nqaceXgE5xepe75j6x5LCUIRdWo7jlVAdlGmMsGSNO", "1122334455", null, null, "Student", "b.johnson" },
                    { 4, new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), new DateOnly(2003, 2, 28), "sarah.williams@school.edu", "Sarah", "Female", true, null, "Williams", "$2a$11$IhfNpX72Ez.nqaceXgE5xepe75j6x5LCUIRdWo7jlVAdlGmMsGSNO", "2233445566", null, null, "Student", "s.williams" },
                    { 5, new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), new DateOnly(2002, 7, 10), "michael.brown@school.edu", "Michael", "Male", true, null, "Brown", "$2a$11$IhfNpX72Ez.nqaceXgE5xepe75j6x5LCUIRdWo7jlVAdlGmMsGSNO", "3344556677", null, null, "Student", "m.brown" },
                    { 6, new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), new DateOnly(2003, 11, 5), "emma.davis@school.edu", "Emma", "Female", true, null, "Davis", "$2a$11$IhfNpX72Ez.nqaceXgE5xepe75j6x5LCUIRdWo7jlVAdlGmMsGSNO", "4455667788", null, null, "Student", "e.davis" },
                    { 7, new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), new DateOnly(2002, 4, 22), "james.miller@school.edu", "James", "Male", true, null, "Miller", "$2a$11$IhfNpX72Ez.nqaceXgE5xepe75j6x5LCUIRdWo7jlVAdlGmMsGSNO", "5566778899", null, null, "Student", "j.miller" },
                    { 8, new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), new DateOnly(2003, 8, 30), "olivia.wilson@school.edu", "Olivia", "Female", true, null, "Wilson", "$2a$11$IhfNpX72Ez.nqaceXgE5xepe75j6x5LCUIRdWo7jlVAdlGmMsGSNO", "6677889900", null, null, "Student", "o.wilson" },
                    { 9, new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), new DateOnly(2002, 12, 18), "david.taylor@school.edu", "David", "Male", true, null, "Taylor", "$2a$11$IhfNpX72Ez.nqaceXgE5xepe75j6x5LCUIRdWo7jlVAdlGmMsGSNO", "7788990011", null, null, "Student", "d.taylor" },
                    { 10, new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), new DateOnly(1978, 3, 15), "robert.chen@school.edu", "Robert", "Male", true, null, "Chen", "$2a$11$pUwviYr0BkvXxrccs3Mz2enXLP582dNuxCvSngXbE5viRlF/SVhMC", "5551234567", null, null, "Teacher", "r.chen" }
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "CourseId", "Code", "Credits", "DepartmentId", "IsActive", "Title" },
                values: new object[,]
                {
                    { 1, "CS101", 3, 1, true, "Introduction to Programming" },
                    { 2, "CS201", 4, 1, true, "Data Structures" },
                    { 3, "CS301", 4, 1, true, "Algorithms" },
                    { 4, "MATH101", 3, 2, true, "Calculus I" },
                    { 5, "MATH201", 3, 2, true, "Linear Algebra" },
                    { 6, "ENG101", 3, 3, true, "English Composition" },
                    { 7, "PHY101", 4, 4, true, "Physics I" },
                    { 8, "BUS101", 3, 5, true, "Introduction to Business" }
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "StudentId", "AccountNumber", "DepartmentId", "EnrollmentDate", "GPA", "IsEnrolled", "ReceivesCashAllowance", "UserId" },
                values: new object[,]
                {
                    { 1, null, 1, new DateOnly(2021, 9, 1), 3.5, true, false, 3 },
                    { 2, null, 1, new DateOnly(2021, 9, 1), 3.7999999999999998, true, false, 4 },
                    { 3, null, 2, new DateOnly(2022, 9, 1), 3.2000000000000002, true, false, 5 },
                    { 4, null, 3, new DateOnly(2022, 9, 1), 3.8999999999999999, true, false, 6 },
                    { 5, null, 4, new DateOnly(2023, 9, 1), 3.6000000000000001, true, false, 7 },
                    { 6, null, 5, new DateOnly(2023, 9, 1), 3.3999999999999999, true, false, 8 },
                    { 7, null, 1, new DateOnly(2021, 9, 1), 3.7000000000000002, true, false, 9 }
                });

            migrationBuilder.InsertData(
                table: "Teachers",
                columns: new[] { "TeacherId", "DepartmentId", "HireDate", "UserId" },
                values: new object[,]
                {
                    { 1, 1, new DateOnly(2010, 8, 1), 2 },
                    { 2, 2, new DateOnly(2015, 1, 15), 10 }
                });

            migrationBuilder.InsertData(
                table: "CafeAccesses",
                columns: new[] { "CafeAccessId", "HasAccessedBreakfast", "HasAccessedDinner", "HasAccessedLunch", "LastResetDate", "ScannableIdCode", "StudentId", "TotalDailyAccesses" },
                values: new object[,]
                {
                    { 1, false, false, false, new DateTime(2025, 12, 15, 0, 0, 0, 0, DateTimeKind.Utc), "8C:3C:CE:44", 1, 0 },
                    { 2, false, false, false, new DateTime(2025, 12, 15, 0, 0, 0, 0, DateTimeKind.Utc), "F9:CE:D9:9B", 2, 0 },
                    { 3, false, false, false, new DateTime(2025, 12, 15, 0, 0, 0, 0, DateTimeKind.Utc), "C6:65:D3:EA", 3, 0 },
                    { 4, false, false, false, new DateTime(2025, 12, 15, 0, 0, 0, 0, DateTimeKind.Utc), "79:68:DB:9B", 4, 0 },
                    { 5, false, false, false, new DateTime(2025, 12, 15, 0, 0, 0, 0, DateTimeKind.Utc), "67:EC:4B:84", 5, 0 },
                    { 6, false, false, false, new DateTime(2025, 12, 15, 0, 0, 0, 0, DateTimeKind.Utc), "59:3B:3D:41", 6, 0 },
                    { 7, false, false, false, new DateTime(2025, 12, 15, 0, 0, 0, 0, DateTimeKind.Utc), "55:03:FE:EC", 7, 0 }
                });

            migrationBuilder.InsertData(
                table: "CourseAssignments",
                columns: new[] { "CourseAssignmentId", "CourseId", "Semester", "TeacherId", "Year" },
                values: new object[,]
                {
                    { 1, 1, 0, 1, 2024 },
                    { 2, 2, 0, 1, 2024 },
                    { 3, 3, 1, 1, 2024 },
                    { 4, 4, 0, 2, 2024 },
                    { 5, 5, 1, 2, 2024 }
                });

            migrationBuilder.InsertData(
                table: "Enrollments",
                columns: new[] { "EnrollmentId", "CourseId", "Grade", "Semester", "StudentId", "Year" },
                values: new object[,]
                {
                    { 1, 1, "A", "Semester1", 1, 2024 },
                    { 2, 2, "B+", "Semester1", 1, 2024 },
                    { 3, 1, "A-", "Semester1", 2, 2024 },
                    { 4, 4, "B", "Semester1", 3, 2024 },
                    { 5, 6, "A", "Semester1", 4, 2024 },
                    { 6, 1, "A", "Semester1", 7, 2024 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CafeAccesses_ScannableIdCode",
                table: "CafeAccesses",
                column: "ScannableIdCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CafeAccesses_StudentId",
                table: "CafeAccesses",
                column: "StudentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseAssignments_CourseId",
                table: "CourseAssignments",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseAssignments_TeacherId",
                table: "CourseAssignments",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_Code",
                table: "Courses",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_DepartmentId",
                table: "Courses",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_CourseId",
                table: "Enrollments",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_StudentId_CourseId_Year_Semester",
                table: "Enrollments",
                columns: new[] { "StudentId", "CourseId", "Year", "Semester" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_AccountNumber",
                table: "Students",
                column: "AccountNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_DepartmentId",
                table: "Students",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_UserId",
                table: "Students",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeacherDepartmentHistory_DepartmentId",
                table: "TeacherDepartmentHistory",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherDepartmentHistory_TeacherId",
                table: "TeacherDepartmentHistory",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_DepartmentId",
                table: "Teachers",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_UserId",
                table: "Teachers",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CafeAccesses");

            migrationBuilder.DropTable(
                name: "CourseAssignments");

            migrationBuilder.DropTable(
                name: "Enrollments");

            migrationBuilder.DropTable(
                name: "TeacherDepartmentHistory");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
