using System.Net.Http.Json;
using AttendanceShared.DTOs;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace ProfessorApp.Services
{
    public class ClientService
    {
        private readonly HttpClient _httpClient;
        public string? ProfessorId { get; private set; }
        public ProfessorDTO? CurrentProfessor { get; private set; }

        public ClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<AuthResponseDTO?> LoginAsync(string identifier, string password)
        {
            var loginDto = new LoginDTO { Identifier = identifier, Password = password };
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginDto);
            if (response.IsSuccessStatusCode)
            {
                var auth = await response.Content.ReadFromJsonAsync<AuthResponseDTO>();
                if (auth != null)
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth.Token);
                    ProfessorId = auth.User.ID;
                    CurrentProfessor = auth.User;
                }
                return auth;
            }
            return null;
        }

        // gets register address and waits for a response
        public async Task<AuthResponseDTO?> RegisterAsync(string id, string firstName, string lastName, string username, string email, string password)
        {
            var registerDto = new RegisterDTO { ID = id, FirstName = firstName, LastName = lastName, Username = username, Email = email, Password = password };
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", registerDto);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AuthResponseDTO>();
            }
            return null;
        }


        // Student Methods
        public async Task<List<StudentDTO>?> GetAllStudentsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<StudentDTO>>("api/student");
        }

        public async Task<StudentDTO?> GetStudentByUTDIdAsync(string utdId)
        {
            return await _httpClient.GetFromJsonAsync<StudentDTO>($"api/student/id/{utdId}");
        }

        public async Task<StudentDTO?> GetStudentByUsernameAsync(string username)
        {
            return await _httpClient.GetFromJsonAsync<StudentDTO>($"api/student/username/{username}");
        }

        public async Task<bool> AddStudentAsync(StudentDTO student)
        {
            var response = await _httpClient.PostAsJsonAsync("api/student", student);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateStudentAsync(StudentDTO student)
        {
            var response = await _httpClient.PutAsJsonAsync("api/student", student);
            return response.IsSuccessStatusCode;
        }

        public async Task<string?> DeleteStudentByUTDIdAsync(string utdId)
        {
            var response = await _httpClient.DeleteAsync($"api/student/{utdId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            return null;
        }

        // QuizBank Methods
        public async Task<List<QuizQuestionBankDTO>?> GetAllQuizQuestionBanksAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<QuizQuestionBankDTO>>("api/quizquestionbank");
            return response;
        }

        public async Task<QuizQuestionBankDTO?> GetQuizQuestionBankByIDAsync(int bankId)
        {
            var response = await _httpClient.GetFromJsonAsync<QuizQuestionBankDTO>($"api/quizquestionbank/{bankId}");
            return response;
        }

        public async Task<bool> CreateQuizQuestionBankAsync(QuizQuestionBankDTO bank)
        {
            var response = await _httpClient.PostAsJsonAsync("api/quizquestionbank", bank);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateQuizQuestionBankAsync(QuizQuestionBankDTO bank)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/quizquestionbank/{bank.QuestionBankID}", bank);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteQuizQuestionBankAsync(int bankId)
        {
            var response = await _httpClient.DeleteAsync($"api/quizquestionbank/{bankId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<string>?> GetAllQuizBankNamesAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<string>>("api/quizquestionbank/banknames");
            return response;
        }

        public async Task<int> GetQuestionBankIdByNameAsync(string bankName)
        {
            var response = await _httpClient.GetFromJsonAsync<int>($"api/quizquestionbank/GetBankIdByName?bankName={bankName}");
            return response;
        }
        public async Task<List<QuizQuestionBankDTO>?> GetQuizBanksByCourseIdAsync(int courseId)
        {
            var response = await _httpClient.GetFromJsonAsync<List<QuizQuestionBankDTO>>($"api/quizquestionbank/course/{courseId}");
            return response;
        }



        // Quizquestion Methods
        public async Task<List<QuizQuestionDTO>?> GetAllQuizQuestionAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<QuizQuestionDTO>>("api/quizquestion");
            return response;
        }

        public async Task<List<QuizQuestionDTO>?> GetQuizQuestionByIdAsync(int questionId)
        {
            var response = await _httpClient.GetFromJsonAsync<List<QuizQuestionDTO>>($"api/quizquestion/{questionId}");
            return response;
        }
        public async Task<bool> CreateQuizQuestionAsync(QuizQuestionDTO question)
        {
            var response = await _httpClient.PostAsJsonAsync("api/quizquestion", question);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateQuizQuestionAsync(QuizQuestionDTO question)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/quizquestion/{question.QuestionID}", question);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteQuizQuestionAsync(int questionId)
        {
            var response = await _httpClient.DeleteAsync($"api/quizquestion/{questionId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<QuizQuestionDTO>?> GetQuestionsByBankIdAsync(int bankId)
        {
            var httpResponse = await _httpClient.GetAsync($"api/quizquestion/GetByBankId/{bankId}");

            if (httpResponse.IsSuccessStatusCode)
            {
                //Only try to read JSON if status is success
                var questions = await httpResponse.Content.ReadFromJsonAsync<List<QuizQuestionDTO>>();
                return questions;
            }
            else if (httpResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            else
            {
                throw new Exception($"Unexpected error: {httpResponse.StatusCode}");
            }
        }

        public async Task<int?> GetQuestionIdByTextAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return null;

            var response = await _httpClient.GetAsync($"api/quizquestion/GetQuestionIdByText?text={Uri.EscapeDataString(text)}");
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<int?>()
                : null;
        }


        // Courses
        public async Task<List<CourseDTO>?> GetAllCoursesAsync()
            => await _httpClient.GetFromJsonAsync<List<CourseDTO>>("api/course");

        public async Task<List<CourseDTO>?> GetCoursesByProfessorAsync()
        {
            if (ProfessorId == null) return null;
            return await _httpClient.GetFromJsonAsync<List<CourseDTO>>($"api/course/professor/{ProfessorId}");
        }

        public async Task<bool> AddCourseAsync(CourseDTO course)
        {
            var response = await _httpClient.PostAsJsonAsync("api/course", course);
            return response.IsSuccessStatusCode;
        }
        public async Task<CourseDTO?> GetCourseByIdAsync(int? id)
        {
            return await _httpClient.GetFromJsonAsync<CourseDTO>($"api/course/{id}");
        }
        public async Task<string?> DeleteCourseByIDAsync(int? id)
        {
            var response = await _httpClient.DeleteAsync($"api/course/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync(); 
            }
            return null;
        }

        // Enrollments
        public async Task<List<CourseEnrollmentDetailDTO>?> GetEnrollmentsAsync(int? courseID)
            => await _httpClient.GetFromJsonAsync<List<CourseEnrollmentDetailDTO>>($"api/courseenrollment/course/{courseID}");

        public async Task<bool> EnrollStudentToCourseAsync(CourseEnrollmentDTO dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/courseenrollment", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UnenrollStudentAsync(int enrollmentId)
        {
            var response = await _httpClient.DeleteAsync($"api/courseenrollment/{enrollmentId}");
            return response.IsSuccessStatusCode;
        }

        // course sessions
        // get course sessions by the course's ID
        public async Task<List<ClassSessionDTO>?> GetSessionsByCourseIDAsync(int? courseID)
            => await _httpClient.GetFromJsonAsync<List<ClassSessionDTO>>($"api/classsession/course/{courseID}");

        public async Task<List<ClassSessionDTO>?> GetSessionsAsync()
            => await _httpClient.GetFromJsonAsync<List<ClassSessionDTO>>("api/classsession/");
        public async Task<List<ClassSessionDTO>?> GetSessionBySessionDateTimeAsync(DateTime sessionDateTime)
             => await _httpClient.GetFromJsonAsync<List<ClassSessionDTO>>($"api/classsession/datetime/{sessionDateTime:yyyy-MM-dd}");

        // get course session by the session ID
        public async Task<ClassSessionDTO?> GetSessionByIDAsync(int? sessionID)
            => await _httpClient.GetFromJsonAsync<ClassSessionDTO>($"api/classsession/{sessionID}");

        // add a class session
        public async Task<bool> AddClassSessionAsync(ClassSessionDTO dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/classsession", dto);
            return response.IsSuccessStatusCode;
        }

        // update a class session
        public async Task<bool> UpdateClassSessionAsync(ClassSessionDTO dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/classsession/{dto.SessionID}", dto);
            return response.IsSuccessStatusCode;
        }

        // remove a class session by ID
        public async Task<bool> RemoveClassSessionAsync(int sessionID)
        {
            var response = await _httpClient.DeleteAsync($"api/classsession/{sessionID}");
            return response.IsSuccessStatusCode;
        }

        // Attendance methods
        public async Task<List<AttendanceDTO>?> GetAllAttendancesAsync()
            => await _httpClient.GetFromJsonAsync<List<AttendanceDTO>>("api/attendance");

        // Attendance methods by course
        public async Task<List<AttendanceDTO>?> GetAttendancesByCourseIDAsync(int? courseID)
            => await _httpClient.GetFromJsonAsync<List<AttendanceDTO>>($"api/attendance/course/{courseID}");

        public async Task<bool> UpdateAttendanceAsync(int AttendanceID, AttendanceDTO attendance)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/attendance/{attendance.AttendanceID}", attendance);
            return response.IsSuccessStatusCode;
        }


        public async Task<int> GetAttendanceIdBySessionAndUtdIdAsync(int sessionId, string utdId)
        {
            var response = await _httpClient.GetAsync($"api/attendance/session/{sessionId}/student/{utdId}/id");
            if (response.IsSuccessStatusCode)
            {
                // Read the response content as integer
                var content = await response.Content.ReadAsStringAsync();
                if (int.TryParse(content, out int attendanceId))
                {
                    return attendanceId;
                }
            }
            return 0;
        }

        //Session Question
        public async Task<SessionQuestionDTO?> CreateSessionQuestionAsync(SessionQuestionDTO sessionQuestion)
        {
            var response = await _httpClient.PostAsJsonAsync("api/sessionquestion", sessionQuestion);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<SessionQuestionDTO>();
            }
            return null;
        }
    }
}