using AttendanceSystem.Models;
using AttendanceSystem.Data.Repositories;

namespace AttendanceSystem.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository; // repository instance

        public AttendanceService(IAttendanceRepository attendanceRepository)
        {
            _attendanceRepository = attendanceRepository; // dependency injection
        }

        public async Task<List<Attendance>> GetAllAttendancesAsync()
        {
            var result = await _attendanceRepository.GetAllAsync(); // get recs from repository
            return new List<Attendance>(result);                    // convert to list and return
        }

        public async Task<Attendance?> GetAttendanceByIdAsync(int id)
        {
            return await _attendanceRepository.GetByIdAsync(id);   // get rec by ID
        }

        public async Task<List<Attendance>> GetPresentAttendancesAsync()
        {
            var result = await _attendanceRepository.GetPresentAttendancesAsync(); // get recs where type is Present
            return new List<Attendance>(result);                    // convert to list and return
        }

        public async Task CreateAttendanceAsync(Attendance attendance)
        {
            await _attendanceRepository.AddAsync(attendance);       // add a new rec via repository
        }

        public async Task UpdateAttendanceAsync(Attendance attendance)
        {
            await _attendanceRepository.UpdateAsync(attendance);    // update an existing rec via repository
        }

        public async Task DeleteAttendanceAsync(int id)
        {
            var attendance = await _attendanceRepository.GetByIdAsync(id); // get rec to delete
            if (attendance != null)
            {
                await _attendanceRepository.DeleteAsync(attendance);       // delete rec if found
            }
        }
    }
}