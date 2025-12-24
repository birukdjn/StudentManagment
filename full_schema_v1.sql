CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) NOT NULL,
    `ProductVersion` varchar(32) NOT NULL,
    PRIMARY KEY (`MigrationId`)
);

START TRANSACTION;
CREATE TABLE `Departments` (
    `DepartmentId` int NOT NULL AUTO_INCREMENT,
    `Name` varchar(100) NOT NULL,
    `IsActive` tinyint(1) NOT NULL,
    PRIMARY KEY (`DepartmentId`)
);

CREATE TABLE `Users` (
    `UserId` int NOT NULL AUTO_INCREMENT,
    `FirstName` varchar(100) NOT NULL,
    `LastName` varchar(100) NOT NULL,
    `Gender` longtext NOT NULL,
    `DateOfBirth` date NOT NULL,
    `Phone` varchar(13) NULL,
    `Username` varchar(50) NOT NULL,
    `Role` longtext NOT NULL,
    `Email` varchar(100) NOT NULL,
    `PasswordHash` varchar(255) NOT NULL,
    `IsActive` tinyint(1) NOT NULL,
    `CreatedAt` datetime(6) NOT NULL,
    `LastLogin` datetime(6) NULL,
    `RefreshToken` varchar(500) NULL,
    `RefreshTokenExpiryTime` datetime(6) NULL,
    PRIMARY KEY (`UserId`)
);

CREATE TABLE `Courses` (
    `CourseId` int NOT NULL AUTO_INCREMENT,
    `Code` varchar(10) NOT NULL,
    `Title` varchar(200) NOT NULL,
    `Credits` int NOT NULL,
    `IsActive` tinyint(1) NOT NULL,
    `DepartmentId` int NOT NULL,
    PRIMARY KEY (`CourseId`),
    CONSTRAINT `FK_Courses_Departments_DepartmentId` FOREIGN KEY (`DepartmentId`) REFERENCES `Departments` (`DepartmentId`) ON DELETE CASCADE
);

CREATE TABLE `Students` (
    `StudentId` int NOT NULL AUTO_INCREMENT,
    `EnrollmentDate` date NOT NULL,
    `GPA` double NOT NULL,
    `IsEnrolled` tinyint(1) NOT NULL,
    `ReceivesCashAllowance` tinyint(1) NOT NULL,
    `AccountNumber` varchar(255) NULL,
    `DepartmentId` int NOT NULL,
    `UserId` int NOT NULL,
    PRIMARY KEY (`StudentId`),
    CONSTRAINT `FK_Students_Departments_DepartmentId` FOREIGN KEY (`DepartmentId`) REFERENCES `Departments` (`DepartmentId`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Students_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`UserId`) ON DELETE CASCADE
);

CREATE TABLE `Teachers` (
    `TeacherId` int NOT NULL AUTO_INCREMENT,
    `DepartmentId` int NULL,
    `HireDate` date NOT NULL,
    `UserId` int NOT NULL,
    PRIMARY KEY (`TeacherId`),
    CONSTRAINT `FK_Teachers_Departments_DepartmentId` FOREIGN KEY (`DepartmentId`) REFERENCES `Departments` (`DepartmentId`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Teachers_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`UserId`) ON DELETE CASCADE
);

CREATE TABLE `CafeAccesses` (
    `CafeAccessId` int NOT NULL AUTO_INCREMENT,
    `StudentId` int NOT NULL,
    `ScannableIdCode` varchar(50) NOT NULL,
    `HasAccessedBreakfast` tinyint(1) NOT NULL,
    `HasAccessedLunch` tinyint(1) NOT NULL,
    `HasAccessedDinner` tinyint(1) NOT NULL,
    `LastResetDate` datetime(6) NOT NULL,
    `TotalDailyAccesses` int NOT NULL,
    PRIMARY KEY (`CafeAccessId`),
    CONSTRAINT `FK_CafeAccesses_Students_StudentId` FOREIGN KEY (`StudentId`) REFERENCES `Students` (`StudentId`) ON DELETE CASCADE
);

CREATE TABLE `Enrollments` (
    `EnrollmentId` int NOT NULL AUTO_INCREMENT,
    `StudentId` int NOT NULL,
    `CourseId` int NOT NULL,
    `Year` int NOT NULL,
    `Semester` varchar(255) NOT NULL,
    `Grade` varchar(5) NULL,
    PRIMARY KEY (`EnrollmentId`),
    CONSTRAINT `FK_Enrollments_Courses_CourseId` FOREIGN KEY (`CourseId`) REFERENCES `Courses` (`CourseId`) ON DELETE CASCADE,
    CONSTRAINT `FK_Enrollments_Students_StudentId` FOREIGN KEY (`StudentId`) REFERENCES `Students` (`StudentId`) ON DELETE CASCADE
);

CREATE TABLE `CourseAssignments` (
    `CourseAssignmentId` int NOT NULL AUTO_INCREMENT,
    `CourseId` int NOT NULL,
    `TeacherId` int NOT NULL,
    `Year` int NOT NULL,
    `Semester` int NOT NULL,
    PRIMARY KEY (`CourseAssignmentId`),
    CONSTRAINT `FK_CourseAssignments_Courses_CourseId` FOREIGN KEY (`CourseId`) REFERENCES `Courses` (`CourseId`) ON DELETE CASCADE,
    CONSTRAINT `FK_CourseAssignments_Teachers_TeacherId` FOREIGN KEY (`TeacherId`) REFERENCES `Teachers` (`TeacherId`) ON DELETE CASCADE
);

CREATE TABLE `TeacherDepartmentHistory` (
    `HistoryId` int NOT NULL AUTO_INCREMENT,
    `TeacherId` int NOT NULL,
    `DepartmentId` int NOT NULL,
    `StartDate` date NOT NULL,
    `EndDate` date NULL,
    PRIMARY KEY (`HistoryId`),
    CONSTRAINT `FK_TeacherDepartmentHistory_Departments_DepartmentId` FOREIGN KEY (`DepartmentId`) REFERENCES `Departments` (`DepartmentId`) ON DELETE CASCADE,
    CONSTRAINT `FK_TeacherDepartmentHistory_Teachers_TeacherId` FOREIGN KEY (`TeacherId`) REFERENCES `Teachers` (`TeacherId`) ON DELETE CASCADE
);

INSERT INTO `Departments` (`DepartmentId`, `IsActive`, `Name`)
VALUES (1, TRUE, 'Computer Science');
SELECT ROW_COUNT();

INSERT INTO `Departments` (`DepartmentId`, `IsActive`, `Name`)
VALUES (2, TRUE, 'Applied Mathematics');
SELECT ROW_COUNT();

INSERT INTO `Departments` (`DepartmentId`, `IsActive`, `Name`)
VALUES (3, TRUE, 'English Literature');
SELECT ROW_COUNT();

INSERT INTO `Departments` (`DepartmentId`, `IsActive`, `Name`)
VALUES (4, TRUE, 'Physics');
SELECT ROW_COUNT();

INSERT INTO `Departments` (`DepartmentId`, `IsActive`, `Name`)
VALUES (5, TRUE, 'Business Administration');
SELECT ROW_COUNT();


INSERT INTO `Users` (`UserId`, `CreatedAt`, `DateOfBirth`, `Email`, `FirstName`, `Gender`, `IsActive`, `LastLogin`, `LastName`, `PasswordHash`, `Phone`, `RefreshToken`, `RefreshTokenExpiryTime`, `Role`, `Username`)
VALUES (1, '2024-01-01 10:00:00.000000', DATE '1980-01-01', 'admin@school.edu', 'System', 'Male', TRUE, NULL, 'Admin', '$2a$11$KLk9ZOo5rPwRL8rVqVUS.uLbsVbzYvwynCOChUyQhpnpseuUOU19G', '1234567890', NULL, NULL, 'Admin', 'sys.admin');
SELECT ROW_COUNT();

INSERT INTO `Users` (`UserId`, `CreatedAt`, `DateOfBirth`, `Email`, `FirstName`, `Gender`, `IsActive`, `LastLogin`, `LastName`, `PasswordHash`, `Phone`, `RefreshToken`, `RefreshTokenExpiryTime`, `Role`, `Username`)
VALUES (2, '2024-01-01 10:00:00.000000', DATE '1985-05-20', 'alice.smith@school.edu', 'Alice', 'Female', TRUE, NULL, 'Smith', '$2a$11$pUwviYr0BkvXxrccs3Mz2enXLP582dNuxCvSngXbE5viRlF/SVhMC', '0987654321', NULL, NULL, 'Teacher', 'a.smith');
SELECT ROW_COUNT();

INSERT INTO `Users` (`UserId`, `CreatedAt`, `DateOfBirth`, `Email`, `FirstName`, `Gender`, `IsActive`, `LastLogin`, `LastName`, `PasswordHash`, `Phone`, `RefreshToken`, `RefreshTokenExpiryTime`, `Role`, `Username`)
VALUES (3, '2024-01-01 10:00:00.000000', DATE '2002-09-15', 'bob.johnson@school.edu', 'Bob', 'Male', TRUE, NULL, 'Johnson', '$2a$11$IhfNpX72Ez.nqaceXgE5xepe75j6x5LCUIRdWo7jlVAdlGmMsGSNO', '1122334455', NULL, NULL, 'Student', 'b.johnson');
SELECT ROW_COUNT();

INSERT INTO `Users` (`UserId`, `CreatedAt`, `DateOfBirth`, `Email`, `FirstName`, `Gender`, `IsActive`, `LastLogin`, `LastName`, `PasswordHash`, `Phone`, `RefreshToken`, `RefreshTokenExpiryTime`, `Role`, `Username`)
VALUES (4, '2024-01-01 10:00:00.000000', DATE '2003-02-28', 'sarah.williams@school.edu', 'Sarah', 'Female', TRUE, NULL, 'Williams', '$2a$11$IhfNpX72Ez.nqaceXgE5xepe75j6x5LCUIRdWo7jlVAdlGmMsGSNO', '2233445566', NULL, NULL, 'Student', 's.williams');
SELECT ROW_COUNT();

INSERT INTO `Users` (`UserId`, `CreatedAt`, `DateOfBirth`, `Email`, `FirstName`, `Gender`, `IsActive`, `LastLogin`, `LastName`, `PasswordHash`, `Phone`, `RefreshToken`, `RefreshTokenExpiryTime`, `Role`, `Username`)
VALUES (5, '2024-01-01 10:00:00.000000', DATE '2002-07-10', 'michael.brown@school.edu', 'Michael', 'Male', TRUE, NULL, 'Brown', '$2a$11$IhfNpX72Ez.nqaceXgE5xepe75j6x5LCUIRdWo7jlVAdlGmMsGSNO', '3344556677', NULL, NULL, 'Student', 'm.brown');
SELECT ROW_COUNT();

INSERT INTO `Users` (`UserId`, `CreatedAt`, `DateOfBirth`, `Email`, `FirstName`, `Gender`, `IsActive`, `LastLogin`, `LastName`, `PasswordHash`, `Phone`, `RefreshToken`, `RefreshTokenExpiryTime`, `Role`, `Username`)
VALUES (6, '2024-01-01 10:00:00.000000', DATE '2003-11-05', 'emma.davis@school.edu', 'Emma', 'Female', TRUE, NULL, 'Davis', '$2a$11$IhfNpX72Ez.nqaceXgE5xepe75j6x5LCUIRdWo7jlVAdlGmMsGSNO', '4455667788', NULL, NULL, 'Student', 'e.davis');
SELECT ROW_COUNT();

INSERT INTO `Users` (`UserId`, `CreatedAt`, `DateOfBirth`, `Email`, `FirstName`, `Gender`, `IsActive`, `LastLogin`, `LastName`, `PasswordHash`, `Phone`, `RefreshToken`, `RefreshTokenExpiryTime`, `Role`, `Username`)
VALUES (7, '2024-01-01 10:00:00.000000', DATE '2002-04-22', 'james.miller@school.edu', 'James', 'Male', TRUE, NULL, 'Miller', '$2a$11$IhfNpX72Ez.nqaceXgE5xepe75j6x5LCUIRdWo7jlVAdlGmMsGSNO', '5566778899', NULL, NULL, 'Student', 'j.miller');
SELECT ROW_COUNT();

INSERT INTO `Users` (`UserId`, `CreatedAt`, `DateOfBirth`, `Email`, `FirstName`, `Gender`, `IsActive`, `LastLogin`, `LastName`, `PasswordHash`, `Phone`, `RefreshToken`, `RefreshTokenExpiryTime`, `Role`, `Username`)
VALUES (8, '2024-01-01 10:00:00.000000', DATE '2003-08-30', 'olivia.wilson@school.edu', 'Olivia', 'Female', TRUE, NULL, 'Wilson', '$2a$11$IhfNpX72Ez.nqaceXgE5xepe75j6x5LCUIRdWo7jlVAdlGmMsGSNO', '6677889900', NULL, NULL, 'Student', 'o.wilson');
SELECT ROW_COUNT();

INSERT INTO `Users` (`UserId`, `CreatedAt`, `DateOfBirth`, `Email`, `FirstName`, `Gender`, `IsActive`, `LastLogin`, `LastName`, `PasswordHash`, `Phone`, `RefreshToken`, `RefreshTokenExpiryTime`, `Role`, `Username`)
VALUES (9, '2024-01-01 10:00:00.000000', DATE '2002-12-18', 'david.taylor@school.edu', 'David', 'Male', TRUE, NULL, 'Taylor', '$2a$11$IhfNpX72Ez.nqaceXgE5xepe75j6x5LCUIRdWo7jlVAdlGmMsGSNO', '7788990011', NULL, NULL, 'Student', 'd.taylor');
SELECT ROW_COUNT();

INSERT INTO `Users` (`UserId`, `CreatedAt`, `DateOfBirth`, `Email`, `FirstName`, `Gender`, `IsActive`, `LastLogin`, `LastName`, `PasswordHash`, `Phone`, `RefreshToken`, `RefreshTokenExpiryTime`, `Role`, `Username`)
VALUES (10, '2024-01-01 10:00:00.000000', DATE '1978-03-15', 'robert.chen@school.edu', 'Robert', 'Male', TRUE, NULL, 'Chen', '$2a$11$pUwviYr0BkvXxrccs3Mz2enXLP582dNuxCvSngXbE5viRlF/SVhMC', '5551234567', NULL, NULL, 'Teacher', 'r.chen');
SELECT ROW_COUNT();


INSERT INTO `Courses` (`CourseId`, `Code`, `Credits`, `DepartmentId`, `IsActive`, `Title`)
VALUES (1, 'CS101', 3, 1, TRUE, 'Introduction to Programming');
SELECT ROW_COUNT();

INSERT INTO `Courses` (`CourseId`, `Code`, `Credits`, `DepartmentId`, `IsActive`, `Title`)
VALUES (2, 'CS201', 4, 1, TRUE, 'Data Structures');
SELECT ROW_COUNT();

INSERT INTO `Courses` (`CourseId`, `Code`, `Credits`, `DepartmentId`, `IsActive`, `Title`)
VALUES (3, 'CS301', 4, 1, TRUE, 'Algorithms');
SELECT ROW_COUNT();

INSERT INTO `Courses` (`CourseId`, `Code`, `Credits`, `DepartmentId`, `IsActive`, `Title`)
VALUES (4, 'MATH101', 3, 2, TRUE, 'Calculus I');
SELECT ROW_COUNT();

INSERT INTO `Courses` (`CourseId`, `Code`, `Credits`, `DepartmentId`, `IsActive`, `Title`)
VALUES (5, 'MATH201', 3, 2, TRUE, 'Linear Algebra');
SELECT ROW_COUNT();

INSERT INTO `Courses` (`CourseId`, `Code`, `Credits`, `DepartmentId`, `IsActive`, `Title`)
VALUES (6, 'ENG101', 3, 3, TRUE, 'English Composition');
SELECT ROW_COUNT();

INSERT INTO `Courses` (`CourseId`, `Code`, `Credits`, `DepartmentId`, `IsActive`, `Title`)
VALUES (7, 'PHY101', 4, 4, TRUE, 'Physics I');
SELECT ROW_COUNT();

INSERT INTO `Courses` (`CourseId`, `Code`, `Credits`, `DepartmentId`, `IsActive`, `Title`)
VALUES (8, 'BUS101', 3, 5, TRUE, 'Introduction to Business');
SELECT ROW_COUNT();


INSERT INTO `Students` (`StudentId`, `AccountNumber`, `DepartmentId`, `EnrollmentDate`, `GPA`, `IsEnrolled`, `ReceivesCashAllowance`, `UserId`)
VALUES (1, NULL, 1, DATE '2021-09-01', 3.5, TRUE, FALSE, 3);
SELECT ROW_COUNT();

INSERT INTO `Students` (`StudentId`, `AccountNumber`, `DepartmentId`, `EnrollmentDate`, `GPA`, `IsEnrolled`, `ReceivesCashAllowance`, `UserId`)
VALUES (2, NULL, 1, DATE '2021-09-01', 3.7999999999999998, TRUE, FALSE, 4);
SELECT ROW_COUNT();

INSERT INTO `Students` (`StudentId`, `AccountNumber`, `DepartmentId`, `EnrollmentDate`, `GPA`, `IsEnrolled`, `ReceivesCashAllowance`, `UserId`)
VALUES (3, NULL, 2, DATE '2022-09-01', 3.2000000000000002, TRUE, FALSE, 5);
SELECT ROW_COUNT();

INSERT INTO `Students` (`StudentId`, `AccountNumber`, `DepartmentId`, `EnrollmentDate`, `GPA`, `IsEnrolled`, `ReceivesCashAllowance`, `UserId`)
VALUES (4, NULL, 3, DATE '2022-09-01', 3.8999999999999999, TRUE, FALSE, 6);
SELECT ROW_COUNT();

INSERT INTO `Students` (`StudentId`, `AccountNumber`, `DepartmentId`, `EnrollmentDate`, `GPA`, `IsEnrolled`, `ReceivesCashAllowance`, `UserId`)
VALUES (5, NULL, 4, DATE '2023-09-01', 3.6000000000000001, TRUE, FALSE, 7);
SELECT ROW_COUNT();

INSERT INTO `Students` (`StudentId`, `AccountNumber`, `DepartmentId`, `EnrollmentDate`, `GPA`, `IsEnrolled`, `ReceivesCashAllowance`, `UserId`)
VALUES (6, NULL, 5, DATE '2023-09-01', 3.3999999999999999, TRUE, FALSE, 8);
SELECT ROW_COUNT();

INSERT INTO `Students` (`StudentId`, `AccountNumber`, `DepartmentId`, `EnrollmentDate`, `GPA`, `IsEnrolled`, `ReceivesCashAllowance`, `UserId`)
VALUES (7, NULL, 1, DATE '2021-09-01', 3.7000000000000002, TRUE, FALSE, 9);
SELECT ROW_COUNT();


INSERT INTO `Teachers` (`TeacherId`, `DepartmentId`, `HireDate`, `UserId`)
VALUES (1, 1, DATE '2010-08-01', 2);
SELECT ROW_COUNT();

INSERT INTO `Teachers` (`TeacherId`, `DepartmentId`, `HireDate`, `UserId`)
VALUES (2, 2, DATE '2015-01-15', 10);
SELECT ROW_COUNT();


INSERT INTO `CafeAccesses` (`CafeAccessId`, `HasAccessedBreakfast`, `HasAccessedDinner`, `HasAccessedLunch`, `LastResetDate`, `ScannableIdCode`, `StudentId`, `TotalDailyAccesses`)
VALUES (1, FALSE, FALSE, FALSE, '2025-12-15 00:00:00.000000', '8C:3C:CE:44', 1, 0);
SELECT ROW_COUNT();

INSERT INTO `CafeAccesses` (`CafeAccessId`, `HasAccessedBreakfast`, `HasAccessedDinner`, `HasAccessedLunch`, `LastResetDate`, `ScannableIdCode`, `StudentId`, `TotalDailyAccesses`)
VALUES (2, FALSE, FALSE, FALSE, '2025-12-15 00:00:00.000000', 'F9:CE:D9:9B', 2, 0);
SELECT ROW_COUNT();

INSERT INTO `CafeAccesses` (`CafeAccessId`, `HasAccessedBreakfast`, `HasAccessedDinner`, `HasAccessedLunch`, `LastResetDate`, `ScannableIdCode`, `StudentId`, `TotalDailyAccesses`)
VALUES (3, FALSE, FALSE, FALSE, '2025-12-15 00:00:00.000000', 'C6:65:D3:EA', 3, 0);
SELECT ROW_COUNT();

INSERT INTO `CafeAccesses` (`CafeAccessId`, `HasAccessedBreakfast`, `HasAccessedDinner`, `HasAccessedLunch`, `LastResetDate`, `ScannableIdCode`, `StudentId`, `TotalDailyAccesses`)
VALUES (4, FALSE, FALSE, FALSE, '2025-12-15 00:00:00.000000', '79:68:DB:9B', 4, 0);
SELECT ROW_COUNT();

INSERT INTO `CafeAccesses` (`CafeAccessId`, `HasAccessedBreakfast`, `HasAccessedDinner`, `HasAccessedLunch`, `LastResetDate`, `ScannableIdCode`, `StudentId`, `TotalDailyAccesses`)
VALUES (5, FALSE, FALSE, FALSE, '2025-12-15 00:00:00.000000', '67:EC:4B:84', 5, 0);
SELECT ROW_COUNT();

INSERT INTO `CafeAccesses` (`CafeAccessId`, `HasAccessedBreakfast`, `HasAccessedDinner`, `HasAccessedLunch`, `LastResetDate`, `ScannableIdCode`, `StudentId`, `TotalDailyAccesses`)
VALUES (6, FALSE, FALSE, FALSE, '2025-12-15 00:00:00.000000', '59:3B:3D:41', 6, 0);
SELECT ROW_COUNT();

INSERT INTO `CafeAccesses` (`CafeAccessId`, `HasAccessedBreakfast`, `HasAccessedDinner`, `HasAccessedLunch`, `LastResetDate`, `ScannableIdCode`, `StudentId`, `TotalDailyAccesses`)
VALUES (7, FALSE, FALSE, FALSE, '2025-12-15 00:00:00.000000', '55:03:FE:EC', 7, 0);
SELECT ROW_COUNT();


INSERT INTO `CourseAssignments` (`CourseAssignmentId`, `CourseId`, `Semester`, `TeacherId`, `Year`)
VALUES (1, 1, 0, 1, 2024);
SELECT ROW_COUNT();

INSERT INTO `CourseAssignments` (`CourseAssignmentId`, `CourseId`, `Semester`, `TeacherId`, `Year`)
VALUES (2, 2, 0, 1, 2024);
SELECT ROW_COUNT();

INSERT INTO `CourseAssignments` (`CourseAssignmentId`, `CourseId`, `Semester`, `TeacherId`, `Year`)
VALUES (3, 3, 1, 1, 2024);
SELECT ROW_COUNT();

INSERT INTO `CourseAssignments` (`CourseAssignmentId`, `CourseId`, `Semester`, `TeacherId`, `Year`)
VALUES (4, 4, 0, 2, 2024);
SELECT ROW_COUNT();

INSERT INTO `CourseAssignments` (`CourseAssignmentId`, `CourseId`, `Semester`, `TeacherId`, `Year`)
VALUES (5, 5, 1, 2, 2024);
SELECT ROW_COUNT();


INSERT INTO `Enrollments` (`EnrollmentId`, `CourseId`, `Grade`, `Semester`, `StudentId`, `Year`)
VALUES (1, 1, 'A', 'Semester1', 1, 2024);
SELECT ROW_COUNT();

INSERT INTO `Enrollments` (`EnrollmentId`, `CourseId`, `Grade`, `Semester`, `StudentId`, `Year`)
VALUES (2, 2, 'B+', 'Semester1', 1, 2024);
SELECT ROW_COUNT();

INSERT INTO `Enrollments` (`EnrollmentId`, `CourseId`, `Grade`, `Semester`, `StudentId`, `Year`)
VALUES (3, 1, 'A-', 'Semester1', 2, 2024);
SELECT ROW_COUNT();

INSERT INTO `Enrollments` (`EnrollmentId`, `CourseId`, `Grade`, `Semester`, `StudentId`, `Year`)
VALUES (4, 4, 'B', 'Semester1', 3, 2024);
SELECT ROW_COUNT();

INSERT INTO `Enrollments` (`EnrollmentId`, `CourseId`, `Grade`, `Semester`, `StudentId`, `Year`)
VALUES (5, 6, 'A', 'Semester1', 4, 2024);
SELECT ROW_COUNT();

INSERT INTO `Enrollments` (`EnrollmentId`, `CourseId`, `Grade`, `Semester`, `StudentId`, `Year`)
VALUES (6, 1, 'A', 'Semester1', 7, 2024);
SELECT ROW_COUNT();


CREATE UNIQUE INDEX `IX_CafeAccesses_ScannableIdCode` ON `CafeAccesses` (`ScannableIdCode`);

CREATE UNIQUE INDEX `IX_CafeAccesses_StudentId` ON `CafeAccesses` (`StudentId`);

CREATE INDEX `IX_CourseAssignments_CourseId` ON `CourseAssignments` (`CourseId`);

CREATE INDEX `IX_CourseAssignments_TeacherId` ON `CourseAssignments` (`TeacherId`);

CREATE UNIQUE INDEX `IX_Courses_Code` ON `Courses` (`Code`);

CREATE INDEX `IX_Courses_DepartmentId` ON `Courses` (`DepartmentId`);

CREATE INDEX `IX_Enrollments_CourseId` ON `Enrollments` (`CourseId`);

CREATE UNIQUE INDEX `IX_Enrollments_StudentId_CourseId_Year_Semester` ON `Enrollments` (`StudentId`, `CourseId`, `Year`, `Semester`);

CREATE UNIQUE INDEX `IX_Students_AccountNumber` ON `Students` (`AccountNumber`);

CREATE INDEX `IX_Students_DepartmentId` ON `Students` (`DepartmentId`);

CREATE UNIQUE INDEX `IX_Students_UserId` ON `Students` (`UserId`);

CREATE INDEX `IX_TeacherDepartmentHistory_DepartmentId` ON `TeacherDepartmentHistory` (`DepartmentId`);

CREATE INDEX `IX_TeacherDepartmentHistory_TeacherId` ON `TeacherDepartmentHistory` (`TeacherId`);

CREATE INDEX `IX_Teachers_DepartmentId` ON `Teachers` (`DepartmentId`);

CREATE UNIQUE INDEX `IX_Teachers_UserId` ON `Teachers` (`UserId`);

CREATE UNIQUE INDEX `IX_Users_Email` ON `Users` (`Email`);

CREATE UNIQUE INDEX `IX_Users_Username` ON `Users` (`Username`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20251215103034_initialMigrations', '10.0.1');

COMMIT;

