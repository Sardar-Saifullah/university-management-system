using backend.Dtos;
using backend.Models;
using System.Data;

namespace backend.Repositories
{
    public interface ICourseRepository
    {
        Task<int> CreateCourse(Course course);
        Task<Course> GetCourseById(int id, int userId);
        Task<bool> UpdateCourse(Course course);
        Task<bool> DeleteCourse(int id, int userId);
        Task<CourseListResponseDto> GetFilteredCourses(int userId, int page, int pageSize, int? departmentId, int? programId, int? levelId, bool? isElective, string searchTerm, bool onlyActive);
        Task<BulkUploadResultDto> BulkUploadCourses(string jsonData, int userId);
        Task<CourseListResponseDto> SearchCoursesLightweight(int userId, string searchTerm, int? departmentId, int? levelId, bool? isElective, int page, int pageSize);
        Task<Course> GetById(int courseId);
    }
}