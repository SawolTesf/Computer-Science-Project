﻿/*
This file allows us to share DTOs between the Attendance API and professor App
*/

namespace AttendanceShared.DTOs;

public enum AttendanceType {
    Present,
    Excused,
    Unexcused
}

public class RegisterDTO
{
    public string ID { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class LoginDTO
{
    public string Identifier { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class AuthResponseDTO
{
    public string Token { get; set; } = null!;
    public ProfessorDTO User { get; set; } = null!;
}

// Simple DTO for Professor to avoid circular reference
public class ProfessorDTO
{
    public string ID { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
}

public class StudentDTO
{
    public string UTDID { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Username { get; set; } = null!;
}

public class CourseDTO
{
    public string CourseNumber { get; set; } = null!;
    public string CourseName { get; set; } = null!;
    public string Section { get; set; } = null!;
    public string ProfessorID { get; set; } = null!;
    public int? CourseID { get; set; }
}

public class CourseEnrollmentDTO
{
    public int EnrollmentID { get; set; }
    public string CourseNumber { get; set; } = null!;
    public string UTDID { get; set; } = null!;
    public int? CourseID { get; set; }
}

public class CourseEnrollmentDetailDTO
{
    public int EnrollmentID { get; set; }
    public StudentDTO Student { get; set; } = null!;
}

public class AttendanceDTO{
    public int AttendanceID { get; set; }

    public int SessionID { get; set; } 

    public string UTDID { get; set; } = string.Empty; 

    public DateTime SubmissionTime { get; set; }

    public string IPAddress { get; set; } = string.Empty;

    public AttendanceType AttendanceType { get; set; } = AttendanceType.Present;
}


public class ClassSessionDTO{
    public int SessionID { get; set; }
    public int? CourseID { get; set; }
    public DateTime SessionDateTime { get; set; }
    public string Password { get; set; } = string.Empty;
    public DateTime QuizStartTime { get; set; }
    public DateTime QuizEndTime { get; set; }
    public int QuestionBankID { get; set; }
    public string AccessCode { get; set; } = string.Empty;
}

// DTO returned when students join via AccessCode
public class SessionInfoDTO {
    public string AccessCode { get; set; } = string.Empty;
    public string CourseTitle { get; set; } = string.Empty; // e.g. CS4485.001 – Computer Science Project
    public string QuizBankName { get; set; } = string.Empty;
}

public class ClassSessionFormatDTO
{
    public string Date { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Duration { get; set; } = string.Empty;
    public string Quiz { get; set; } = string.Empty;
    public int SessionID { get; set; }
    public string SessionNumber { get; set; } = string.Empty;
    public string AccessCode { get; set; } = string.Empty;
}

// DTO for session-question mapping
public class SessionQuestionDTO {
    public int SessionQuestionID { get; set; }
    public int SessionID { get; set; }
    public int QuestionID { get; set; }
}

public class QuizQuestionBankDTO{
    public int QuestionBankID { get; set; }
    public string BankName { get; set; } = string.Empty;
    public string CourseNumber { get; set; } = string.Empty; 
    public CourseDTO? Course { get; set; } = null!;
    public int CourseID { get; set; }
}
public class QuizQuestionDTO{
    public int QuestionID { get; set; }
    public int QuestionBankID { get; set; } 
    public string QuestionText { get; set; } = string.Empty;
    public string Option1 { get; set; } = string.Empty;
    public string Option2 { get; set; } = string.Empty;
    public int Answer { get; set; } = 0;
    // Option3 and Option4 are optional
    public string? Option3 { get; set; }
    public string? Option4 { get; set; }
}

public class QuizResponseDTO{
    public int ResponseID { get; set; }
    public int AttendanceID { get; set; }  
    public int QuestionID { get; set; }    
    public int SelectedOption { get; set; }
}