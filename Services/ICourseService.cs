using backend.Dtos;
using backend.Models;
using Microsoft.AspNetCore.Http;

namespace backend.Services
{
    public interface ICourseService
    {
        Task<CourseResponseDto> CreateCourse(CourseCreateDto dto, int userId);
        Task<CourseResponseDto> GetCourse(int id, int userId);
        Task<CourseListResponseDto> GetCourses(int userId, int page, int pageSize, int? departmentId, int? programId, int? levelId, bool? isElective, string searchTerm, bool onlyActive);
        Task<CourseResponseDto> UpdateCourse(int id, CourseUpdateDto dto, int userId);
        Task<bool> DeleteCourse(int id, int userId);
        Task<BulkUploadResultDto> BulkUploadCourses(IFormFile file, int userId);
        Task<CourseListResponseDto> SearchCoursesLightweight(int userId, string searchTerm, int? departmentId, int? levelId, bool? isElective, int page, int pageSize);
    }

}