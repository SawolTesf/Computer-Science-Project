using AttendanceSystem.Models;
using AttendanceSystem.Data.Repositories; // Assumes ICourseRepository exists
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using AttendanceSystem.Models.DTOs;
//Dinagaran Senthilkumar
// This is basically the CourseService.cs file that implements the ICourseService interface.
namespace AttendanceSystem.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository; // Repository dependency

        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<List<CourseDTO>> GetAllCoursesAsync()
        {
            var courses = await _courseRepository.GetAllCoursesAsync();
            var coursesDTOs = new List<CourseDTO>();
            // convert courses to DTOs
            foreach (var course in courses)
            {
                var courseDto = new CourseDTO
                {
                    CourseNumber = course.CourseNumber,
                    CourseName = course.CourseName,
                    Section = course.Section,
                    ProfessorID = course.ProfessorID,
                    CourseID = course.CourseID
                };
                coursesDTOs.Add(courseDto);
            }
            return new List<CourseDTO>(coursesDTOs);
        }

        public async Task<CourseDTO?> GetCourseByIDAsync(int courseID)
        {
            Course? course = await _courseRepository.GetCourseByIDAsync(courseID);
            if (course == null)
            {
                return null;
            }
            // convert course to DTO
            var courseDto = new CourseDTO
            {
                CourseNumber = course.CourseNumber,
                CourseName = course.CourseName,
                Section = course.Section,
                ProfessorID = course.ProfessorID,
                CourseID = course.CourseID
            };
            return courseDto;
        }
        public async Task<List<CourseDTO>> GetCoursesByProfessorAsync(string professorID)
        {
            var courses = await _courseRepository.GetAllCoursesAsync();
            var coursesDTOs = new List<CourseDTO>();
            // convert courses to DTOs
            foreach (var course in courses)
            {
                var courseDto = new CourseDTO
                {
                    CourseNumber = course.CourseNumber,
                    CourseName = course.CourseName,
                    Section = course.Section,
                    ProfessorID = course.ProfessorID,
                    CourseID = course.CourseID
                };
                coursesDTOs.Add(courseDto);
            }
            return new List<CourseDTO>(coursesDTOs);
        }

        public async Task CreateCourseAsync(Course course)
        {
            Debug.WriteLine("checking repository");
            await _courseRepository.AddCourseAsync(course);
        }

        public async Task UpdateCourseAsync(Course course)
        {
            await _courseRepository.UpdateCourseAsync(course);
        }

        public async Task DeleteCourseAsync(int courseID)
        {
            var course = await _courseRepository.GetCourseByIDAsync(courseID);
            if (course != null)
            {
                await _courseRepository.DeleteCourseAsync(course);
            }
        }
    }
}
