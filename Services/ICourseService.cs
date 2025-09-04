using backend.Dtos;
using backend.Models;
using Microsoft.AspNetCore.Http;

namespace backend.Services
{
    public interface ICourseService
    {
        Task<CourseResponseDto> CreateCourse(CourseCreateDto dto, int userId);
        Task<CourseResponseDto> GetCourse(int id);
        Task<CourseListResponseDto> GetCourses(int page, int pageSize, int? departmentId, int? programId, int? levelId, bool? isElective, string searchTerm);
        Task<CourseResponseDto> UpdateCourse(int id, CourseUpdateDto dto, int userId);
        Task<bool> DeleteCourse(int id, int userId);
        Task<BulkUploadResultDto> BulkUploadCourses(IFormFile file, int userId);
        Task<IEnumerable<PrerequisiteDto>> GetCoursePrerequisites(int courseId);
        Task<PrerequisiteDto> AddPrerequisite(PrerequisiteDto dto, int userId);
        Task<bool> RemovePrerequisite(int prerequisiteId, int userId);
    }
}