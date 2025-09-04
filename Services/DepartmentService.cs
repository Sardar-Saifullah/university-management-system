

// Services/DepartmentService.cs
using backend.Dtos;
using backend.Models;
using backend.Repositories;

namespace backend.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<DepartmentResponseDto> CreateDepartment(DepartmentCreateDto departmentDto, int userId)
        {
            var department = new Department
            {
                Name = departmentDto.Name,
                Code = departmentDto.Code,
                CreatedBy = userId
            };

            var createdDepartment = await _departmentRepository.Create(department);
            return MapToDto(createdDepartment);
        }

        public async Task<DepartmentResponseDto> GetDepartment(int id)
        {
            var department = await _departmentRepository.GetById(id);
            if (department == null) return null;
            return MapToDto(department);
        }

        public async Task<IEnumerable<DepartmentResponseDto>> GetAllDepartments()
        {
            var departments = await _departmentRepository.GetAll();
            return departments.Select(MapToDto);
        }

        public async Task<DepartmentResponseDto> UpdateDepartment(int id, DepartmentUpdateDto departmentDto, int userId)
        {
            var department = new Department
            {
                Id = id,
                Name = departmentDto.Name,
                Code = departmentDto.Code,
                ModifiedBy = userId
            };

            var updatedDepartment = await _departmentRepository.Update(department);
            return MapToDto(updatedDepartment);
        }

        public async Task<bool> DeleteDepartment(int id, int userId)
        {
            return await _departmentRepository.Delete(id, userId);
        }

        private DepartmentResponseDto MapToDto(Department department)
        {
            return new DepartmentResponseDto
            {
                Id = department.Id,
                Name = department.Name,
                Code = department.Code,
                CreatedAt = department.CreatedAt,
                
                ModifiedAt = department.ModifiedAt
                
             
            };
        }
    }
}