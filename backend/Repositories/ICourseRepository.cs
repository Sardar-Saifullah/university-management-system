using backend.Dtos;
using backend.Models;
using System.Data;

namespace backend.Repositories
{
    public interface ICourseRepository
    {
        Task<int> CreateCourse(Course course);
        Task<Course> GetCourseById(int id);
        Task<IEnumerable<Course>> GetAllCourses();
        Task<bool> UpdateCourse(Course course);
        Task<bool> DeleteCourse(int id, int userId);
        Task<CourseListResponseDto> GetFilteredCourses(int page, int pageSize, int? departmentId, int? programId, int? levelId, bool? isElective, string searchTerm);
        Task<int> BulkUploadCourses(DataTable courses, int userId);
        Task<IEnumerable<CoursePrerequisite>> GetPrerequisitesForCourse(int courseId);
        Task<bool> AddPrerequisite(CoursePrerequisite prerequisite);
        Task<bool> RemovePrerequisite(int prerequisiteId, int userId);
    }
}