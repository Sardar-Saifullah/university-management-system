using backend.Dtos;

namespace backend.Services
{
    public interface ISemesterService
    {
        Task<GetSemesterDto> Create(CreateSemesterDto dto, int userId);
        Task<GetSemesterDto?> GetById(int id);
        Task<IEnumerable<GetSemesterDto>> GetAll();
        Task<GetSemesterDto?> GetCurrent();
        Task<GetSemesterDto> Update(int id, UpdateSemesterDto dto, int userId);
        Task<bool> Delete(int id, int userId);
    }
}
