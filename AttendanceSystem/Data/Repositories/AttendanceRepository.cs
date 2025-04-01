using AttendanceSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.Data.Repositories;
public class AttendanceRepository(AppDbContext context) : IAttendanceRepository {
    private readonly AppDbContext _context = context; // EF Core DbContext

    public async Task<IEnumerable<Attendance>> GetAllAsync()
    {
        return await _context.Attendances.ToListAsync(); // get all attendance recs
    }

    public async Task<Attendance?> GetByIdAsync(int id)
    {
        return await _context.Attendances.FindAsync(id); // get a specific rec by ID
    }

    public async Task<IEnumerable<Attendance>> GetPresentAttendancesAsync()
    {
        return await _context.Attendances
            .Where(a => a.AttendanceType == AttendanceType.Present) // get whose present
            .ToListAsync();
    }

    public async Task AddAsync(Attendance attendance)
    {
        await _context.Attendances.AddAsync(attendance); // add a new rec to the db
        await _context.SaveChangesAsync();               // save changes
    }

    public async Task UpdateAsync(Attendance attendance)
    {
        _context.Attendances.Update(attendance);         // update existing rec
        await _context.SaveChangesAsync();               // save changes
    }

    public async Task DeleteAsync(Attendance attendance)
    {
        _context.Attendances.Remove(attendance);         // delete rec
        await _context.SaveChangesAsync();               // save changes
    }
}