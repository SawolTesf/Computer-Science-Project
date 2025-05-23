using AttendanceSystem.Models;

using AttendanceSystem.Data.Repositories;

namespace AttendanceSystem.Services
{
    public interface IAttendanceService
    {
        Task<List<Attendance>> GetAllAttendancesAsync(); // all atendances
        Task<Attendance?> GetAttendanceByIdAsync(int id); // get by id
        Task<List<Attendance>> GetPresentAttendancesAsync(); // presnte attendance
        Task CreateAttendanceAsync(Attendance attendance); // create attendacen
        Task UpdateAttendanceAsync(Attendance attendance); // update attendance
        Task DeleteAttendanceAsync(int id); //deete 

        Task<bool> RecordExistsAsync(string ipAddress, DateTime date);

        Task<bool> DateExistsAsync(DateTime date);
        /// Retrieve attendances for a specific course via its sessions
        Task<List<Attendance>> GetByCourseIDAsync(int courseID);

        Task<List<Attendance>> GetAttendanceByUtdIdAsync(string utdId);
        Task<int?> GetAttendanceIdBySessionAndUtdIdAsync(int sessionId, string utdId);
        Task<int?> GetAttendanceIdByIpAndSessionAsync(string ipAddress, int sessionId);

    }
}
