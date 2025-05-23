using AttendanceSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
// Dinagaran Senthilkumar
// Interface defining CRUD operations for QuizResponse.
namespace AttendanceSystem.Data.Repositories
{
   
    public interface IQuizResponseRepository
    {
        Task<IEnumerable<QuizResponse>> GetAllResponsesAsync(); // Retrieve all quiz responses.
        Task<QuizResponse?> GetResponseByIdAsync(int responseId); // Retrieve a quiz response by ID.
        Task AddResponseAsync(QuizResponse response); // Add a new quiz response.
        Task UpdateResponseAsync(QuizResponse response); // Update an existing quiz response.
        Task DeleteResponseAsync(QuizResponse response); // Delete a quiz response.
    }
}
