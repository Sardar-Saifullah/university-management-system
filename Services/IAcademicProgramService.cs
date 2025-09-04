using backend.Dtos;

namespace backend.Services
{
    public interface IAcademicProgramService
    {
        Task<ProgramResponseDto> CreateProgram(ProgramCreateDto dto, int userId);
        Task<ProgramResponseDto> GetProgram(int id);
        Task<IEnumerable<ProgramResponseDto>> GetAllPrograms();
        Task<ProgramResponseDto> UpdateProgram(ProgramUpdateDto dto, int userId);
        Task<bool> DeleteProgram(int id, int userId);
    }
}
