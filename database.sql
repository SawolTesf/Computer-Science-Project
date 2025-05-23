CREATE DATABASE AttendanceSystem;
USE AttendanceSystem;

-- Disable FK checks to allow referencing before definition
SET FOREIGN_KEY_CHECKS = 0;

-- Users with login credentials
CREATE TABLE Professor (
    ProfessorID INT AUTO_INCREMENT PRIMARY KEY,
    ID VARCHAR(10) NOT NULL UNIQUE,
    FirstName VARCHAR(255) NOT NULL,
    LastName VARCHAR(255) NOT NULL,
    Username VARCHAR(25) UNIQUE NOT NULL,
    Email VARCHAR(255) UNIQUE NOT NULL,
    PasswordHash VARCHAR(255) NOT NULL
);

-- Imported from CSV, no login credentials
CREATE TABLE Student (
    StudentID INT AUTO_INCREMENT PRIMARY KEY,
    UTDID VARCHAR(10) NOT NULL UNIQUE,
    FirstName VARCHAR(255) NOT NULL,
    LastName VARCHAR(255) NOT NULL,
    Username VARCHAR(10) UNIQUE NOT NULL
);

-- Courses Table
CREATE TABLE Course (
    CourseID INT AUTO_INCREMENT PRIMARY KEY,
    CourseNumber VARCHAR(10) NOT NULL,
    CourseName VARCHAR(255) NOT NULL,
    Section VARCHAR(10) NOT NULL,
    ProfessorID VARCHAR(10) NOT NULL,
    FOREIGN KEY (ProfessorID) REFERENCES Professor(ID) ON DELETE CASCADE
);

CREATE TABLE CourseEnrollment (
    EnrollmentID INT AUTO_INCREMENT PRIMARY KEY,
    EnrollmentDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    CourseID INT NOT NULL,
    UTDID VARCHAR(10) NOT NULL,
    FOREIGN KEY (CourseID) REFERENCES Course(CourseID) ON DELETE CASCADE,
    FOREIGN KEY (UTDID) REFERENCES Student(UTDID) ON DELETE CASCADE,
    UNIQUE KEY (CourseID, UTDID) -- Prevent duplicate enrollments
);

-- Each class meeting with password/quiz config
CREATE TABLE ClassSession (
    SessionID INT AUTO_INCREMENT PRIMARY KEY,
    SessionDateTime DATETIME NOT NULL,
    Password VARCHAR(255) NOT NULL,
    QuizStartTime DATETIME NOT NULL,
    QuizEndTime DATETIME NOT NULL,
    QuestionBankID INT NOT NULL,
    CourseID INT NOT NULL,
    AccessCode VARCHAR(36) NOT NULL DEFAULT (UUID()) UNIQUE, -- this value is automatically generated and will be used as the access code for the session (EX: http://localhost:5225/123e4567-e89b-12d3-a456-426614174000)
    FOREIGN KEY (CourseID) REFERENCES Course(CourseID) ON DELETE CASCADE
);

-- Map questions to sessions (which questions used in each session)
CREATE TABLE SessionQuestion (
    SessionQuestionID INT AUTO_INCREMENT PRIMARY KEY,
    SessionID INT NOT NULL,
    QuestionID INT NOT NULL,
    FOREIGN KEY (SessionID) REFERENCES ClassSession(SessionID) ON DELETE CASCADE,
    FOREIGN KEY (QuestionID) REFERENCES QuizQuestion(QuestionID) ON DELETE CASCADE,
    UNIQUE KEY (SessionID, QuestionID)
);

-- Group of questions for a session
CREATE TABLE QuizQuestionBank (
    QuestionBankID INT AUTO_INCREMENT PRIMARY KEY,
    BankName VARCHAR(255) NOT NULL,
    CourseID INT NOT NULL,
    FOREIGN KEY (CourseID) REFERENCES Course(CourseID) ON DELETE CASCADE
);

-- Quiz Questions - Multiple-choice
CREATE TABLE QuizQuestion (
    QuestionID INT AUTO_INCREMENT PRIMARY KEY,
    QuestionBankID INT NOT NULL,
    QuestionText TEXT NOT NULL,
    Option1 TEXT NOT NULL,
    Option2 TEXT NOT NULL,
    Option3 TEXT,
    Option4 TEXT,
    Answer INT NOT NULL,
    FOREIGN KEY (QuestionBankID) REFERENCES QuizQuestionBank(QuestionBankID) ON DELETE CASCADE
);

-- Student submissions
CREATE TABLE Attendance (
    AttendanceID INT AUTO_INCREMENT PRIMARY KEY,
    SessionID INT NOT NULL,
    UTDID VARCHAR(10) NOT NULL,
    SubmissionTime DATETIME NOT NULL,
    IPAddress VARCHAR(45) NOT NULL,
    AttendanceType ENUM('Present', 'Excused', 'Unexcused') NOT NULL DEFAULT 'Present',
    FOREIGN KEY (SessionID) REFERENCES ClassSession(SessionID) ON DELETE CASCADE,
    FOREIGN KEY (UTDID) REFERENCES Student(UTDID) ON DELETE CASCADE
);

-- Quiz Responses (Student answers)
CREATE TABLE QuizResponse (
    ResponseID INT AUTO_INCREMENT PRIMARY KEY,
    AttendanceID INT NOT NULL,
    QuestionID INT NOT NULL,
    SelectedOption INT NOT NULL,
    FOREIGN KEY (AttendanceID) REFERENCES Attendance(AttendanceID) ON DELETE CASCADE,
    FOREIGN KEY (QuestionID) REFERENCES QuizQuestion(QuestionID) ON DELETE CASCADE
);

-- Sample Data Insertion

-- Re-enable FK checks
SET FOREIGN_KEY_CHECKS = 1;

-- Insert Professors (no dependencies)
INSERT INTO Professor (ProfessorID, ID, FirstName, LastName, Username, Email, PasswordHash)
VALUES
   (1, '5400000001', 'John', 'Smith', 'jsmith', 'john.smith@example.com', '$2a$11$mFhtH8Qi9XNaVBqVCVvX.O0HLHzNTR/Vuz8i27yQ74wennq7qfkYK'), -- "testpassword" is the password
   (2, '5400000002', 'Jane', 'Doe', 'jdoe', 'jane.doe@example.com', 'hashedpassword2'),
   (3, '5400000003', 'Robert', 'Johnson', 'rjohnson', 'robert.johnson@example.com', 'hashedpassword3');

-- Insert Students (no dependencies)
INSERT INTO Student (StudentID, UTDID, FirstName, LastName, Username)
VALUES
     (1, '2100000001', 'Alice', 'Williams', 'axw3456432'),
     (2, '2100000002', 'Bob', 'Brown', 'bxb3456432'),
     (3, '2100000003', 'Charlie', 'Davis', 'cxd3456432'),
     (4, '2100000004', 'Diana', 'Evans', 'dxe3456432'),
     (5, '2100000005', 'Ethan', 'Fisher', 'exf3456432');

-- Insert Courses (depends on Professor)
INSERT INTO Course (CourseID, CourseNumber, CourseName, Section, ProfessorID)
VALUES
    (1, 'CS4485', 'Computer Science Project', '001', '5400000001'),
    (2, 'CS4361', 'Computer Graphics', '002', '5400000002'),
    (3, 'CS4390', 'Computer Networks', '001', '5400000003'),
    (4, 'CS4348', 'Operating Systems', '003', '5400000001');

-- Insert QuizQuestionBanks (depends on Course)
INSERT INTO QuizQuestionBank (QuestionBankID, BankName, CourseID)
VALUES
    (1, 'Week 1 Quiz', 1),
    (2, 'Week 2 Quiz', 1),
    (3, 'Midterm Review', 2),
    (4, 'Final Review', 3);

-- Insert QuizQuestions (depends on QuizQuestionBank)
INSERT INTO QuizQuestion (QuestionID, QuestionBankID, QuestionText, Option1, Option2, Option3, Option4, Answer)
VALUES
    (1, 1, 'Sample question 1 for Week 1?', 'Option A', 'Option B', 'Option C', 'Option D', 2),
    (2, 1, 'Sample question 2 for Week 1?', 'Option A', 'Option B', 'Option C', 'Option D', 4),
    (3, 2, 'Sample question 1 for Week 2?', 'Option A', 'Option B', 'Option C', 'Option D', 1),
    (4, 3, 'Sample question 1 for Midterm Review?', 'Option A', 'Option B', 'Option C', 'Option D', 1),
    (5, 4, 'Sample question 1 for Final Review?', 'Option A', 'Option B', 'Option C', 'Option D', 3);

-- Insert ClassSessions (depends on Course and QuizQuestionBank)
INSERT INTO ClassSession (SessionID, SessionDateTime, Password, QuizStartTime, QuizEndTime, QuestionBankID, CourseID)
VALUES
    (1, '2025-03-24 10:00:00', 'password123', '2025-03-24 10:30:00', '2025-05-24 10:45:00', 1, 1),
    (2, '2025-03-31 10:00:00', 'password456', '2025-03-31 10:30:00', '2025-05-31 10:45:00', 2, 1),
    (3, '2025-04-02 14:00:00', 'password789', '2025-04-02 14:30:00', '2025-05-02 14:45:00', 3, 2),
    (4, '2025-04-04 15:00:00', 'passwordabc', '2025-04-04 15:30:00', '2025-05-04 15:45:00', 4, 3);

-- Insert Attendance (depends on ClassSession and Student)
INSERT INTO Attendance (AttendanceID, SessionID, UTDID, SubmissionTime, IPAddress, AttendanceType)
VALUES
    (1, 1, '2100000001', '2025-03-24 10:05:00', '192.168.1.101', 'Present'),
    (2, 1, '2100000002', '2025-03-24 10:06:00', '192.168.1.102', 'Present'),
    (3, 1, '2100000003', '2025-03-24 10:07:00', '192.168.1.103', 'Present'),
    (4, 2, '2100000001', '2025-03-31 10:04:00', '192.168.1.101', 'Present'),
    (5, 2, '2100000002', '2025-03-31 10:05:00', '192.168.1.102', 'Present'),
    (6, 3, '2100000004', '2025-04-02 14:03:00', '192.168.1.104', 'Present'),
    (7, 3, '2100000005', '2025-04-02 14:04:00', '192.168.1.105', 'Present'),
    (8, 4, '2100000003', '2025-04-04 15:10:00', '192.168.1.103', 'Present'),
    (9, 1, '2100000004', '2025-03-24 10:00:00', '192.168.1.104', 'Unexcused'),
    (10, 2, '2100000003', '2025-03-31 10:30:00', '192.168.1.103', 'Excused');

-- Insert QuizResponses (depends on Attendance and QuizQuestion)
INSERT INTO QuizResponse (ResponseID, AttendanceID, QuestionID, SelectedOption)
VALUES
    (1, 1, 1, 2),  -- Alice chose option 2 for question 1
    (2, 1, 2, 3),  -- Alice chose option 3 for question 2
    (3, 2, 1, 2),  -- Bob chose option 2 for question 1
    (4, 2, 2, 3),  -- Bob chose option 3 for question 2
    (5, 3, 1, 1),  -- Charlie chose option 1 for question 1
    (6, 3, 2, 3),  -- Charlie chose option 3 for question 2
    (7, 4, 3, 1),  -- Alice chose option 1 for question 3
    (8, 5, 3, 1),  -- Bob chose option 1 for question 3
    (9, 6, 4, 3),  -- Diana chose option 3 for question 4
    (10, 7, 4, 3); -- Ethan chose option 3 for question 4

-- Insert CourseEnrollments (sample data)
INSERT INTO CourseEnrollment (EnrollmentID, EnrollmentDate, CourseID, UTDID)
VALUES
    (1, '2025-03-23 09:00:00', 1, '2100000001'),
    (2, '2025-03-23 09:05:00', 1, '2100000002'),
    (3, '2025-03-23 09:10:00', 1, '2100000003'),
    (4, '2025-03-30 09:00:00', 2, '2100000001'),
    (5, '2025-03-30 09:05:00', 2, '2100000002'),
    (6, '2025-04-02 13:55:00', 3, '2100000004'),
    (7, '2025-04-02 13:56:00', 3, '2100000005'),
    (8, '2025-04-04 14:50:00', 4, '2100000003');

-- Insert SessionQuestion mappings (sample data)
INSERT INTO SessionQuestion (SessionQuestionID, SessionID, QuestionID)
VALUES
    (1, 1, 1),
    (2, 1, 2),
    (3, 2, 1),
    (4, 2, 2),
    (5, 3, 3),
    (6, 3, 5),
    (7, 4, 4),
    (8, 4, 5);