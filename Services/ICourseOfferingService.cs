using backend.Dtos;
using System.Text.Json;

namespace backend.Services
{
    public interface ICourseOfferingService
    {
        Task<CourseOfferingGetDto> Create(CourseOfferingCreateDto dto, int userId);
        Task<CourseOfferingGetDto> Update(int id, CourseOfferingUpdateDto dto, int userId);
        Task<bool> Delete(int id, int userId);
        Task<CourseOfferingGetDto> GetById(int id);
        Task<IEnumerable<CourseOfferingGetDto>> GetAll();
        Task<BulkUploadResultDto> BulkUpload(JsonDocument jsonData, int userId);
    }
}