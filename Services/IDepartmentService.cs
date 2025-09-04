using backend.Dtos;
namespace backend.Services
{
    public interface IDepartmentService
    {
        Task<DepartmentResponseDto> CreateDepartment(DepartmentCreateDto departmentDto, int userId);
        Task<DepartmentResponseDto> GetDepartment(int id);
        Task<IEnumerable<DepartmentResponseDto>> GetAllDepartments();
        Task<DepartmentResponseDto> UpdateDepartment(int id, DepartmentUpdateDto departmentDto, int userId);
        Task<bool> DeleteDepartment(int id, int userId);
    }
}
