// ProgramService.cs
using backend.Dtos;
using backend.Models;
using backend.Repositories;

namespace backend.Services
{
    public class AcademicProgramService : IAcademicProgramService
    {
        private readonly IAcademicProgramRepository _programRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public AcademicProgramService(IAcademicProgramRepository programRepository, IDepartmentRepository departmentRepository)
        {
            _programRepository = programRepository;
            _departmentRepository = departmentRepository;
        }

        public async Task<ProgramResponseDto> CreateProgram(ProgramCreateDto dto, int userId)
        {
            var program = new AcademicProgram
            {
                DepartmentId = dto.DepartmentId,
                Name = dto.Name,
                Code = dto.Code,
                DurationSemesters = dto.DurationSemesters,
                CreditHoursRequired = dto.CreditHoursRequired,
                Description = dto.Description,
                CreatedBy = userId
            };

            var createdProgram = await _programRepository.CreateAsync(program);
            return MapToDto(createdProgram);
        }

        public async Task<ProgramResponseDto> GetProgram(int id)
        {
            var program = await _programRepository.GetByIdAsync(id);
            if (program == null) return null;

            return MapToDto(program);
        }

        public async Task<IEnumerable<ProgramResponseDto>> GetAllPrograms()
        {
            var programs = await _programRepository.GetAllAsync();
            return programs.Select(MapToDto);
        }

        public async Task<ProgramResponseDto> UpdateProgram(ProgramUpdateDto dto, int userId)
        {
            var existingProgram = await _programRepository.GetByIdAsync(dto.Id);
            if (existingProgram == null) return null;

            var program = new AcademicProgram
            {
                Id = dto.Id,
                DepartmentId = dto.DepartmentId,
                Name = dto.Name,
                Code = dto.Code,
                DurationSemesters = dto.DurationSemesters,
                CreditHoursRequired = dto.CreditHoursRequired,
                Description = dto.Description,
                ModifiedBy = userId
            };

            var success = await _programRepository.UpdateAsync(program);
            if (!success) return null;

            var updatedProgram = await _programRepository.GetByIdAsync(dto.Id);
            return MapToDto(updatedProgram);
        }

        public async Task<bool> DeleteProgram(int id, int userId)
        {
            return await _programRepository.DeleteAsync(id, userId);
        }

        private ProgramResponseDto MapToDto(AcademicProgram program)
        {
            return new ProgramResponseDto
            {
                Id = program.Id,
                DepartmentId = program.DepartmentId,
                Name = program.Name,
                Code = program.Code,
                DurationSemesters = program.DurationSemesters,
                CreditHoursRequired = program.CreditHoursRequired,
                Description = program.Description,
                DepartmentName = program.Department?.Name,
                DepartmentCode = program.Department?.Code,
                CreatedAt = program.CreatedAt,
                ModifiedAt = program.ModifiedAt
            };
        }
    }
}