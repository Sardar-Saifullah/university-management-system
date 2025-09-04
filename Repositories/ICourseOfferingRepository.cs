using backend.Dtos;
using backend.Models;
using System.Text.Json;

namespace backend.Repositories
{
    public interface ICourseOfferingRepository
    {
        Task<CourseSemesterOffering> Create(CourseSemesterOffering offering, int userId);
        Task<CourseSemesterOffering> Update(int id, CourseSemesterOffering offering, int userId);
        Task<bool> Delete(int id, int userId);
        Task<CourseSemesterOffering> GetById(int id);
        Task<IEnumerable<CourseSemesterOffering>> GetAll();
        Task<BulkUploadResultDto> BulkUpload(JsonDocument jsonData, int userId);
    }
}