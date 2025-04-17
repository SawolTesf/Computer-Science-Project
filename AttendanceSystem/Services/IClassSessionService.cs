using AttendanceSystem.Models;

using AttendanceSystem.Data.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AttendanceSystem.Services
{
    public interface IClassSessionService
    {
        Task<List<ClassSession>> GetAllSessionsAsync();            // Get all class sessions
        Task<ClassSession?> GetSessionByIdAsync(int sessionId);       // Get session by ID
        Task CreateSessionAsync(ClassSession session);                // Create a new class session
        Task UpdateSessionAsync(ClassSession session);                // Update an existing session
        Task DeleteSessionAsync(int sessionId);                       // Delete a session by ID
    }
}
