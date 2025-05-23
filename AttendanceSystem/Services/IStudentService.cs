using AttendanceSystem.Models;
namespace AttendanceSystem.Services;
/*Diego Cabanas:*/
public interface IStudentService {
  Task<IEnumerable<Student>> GetAllStudentsAsync();
  Task<Student?> GetStudentByUTDIdAsync(String id);
  Task<Student?> GetStudentByUsernameAsync(String username);
  Task<bool> AddStudentAsync(Student student);
  Task<bool> UpdateStudentAsync(Student student);
  Task<bool> DeleteStudentByUTDIdAsync(String id);
}