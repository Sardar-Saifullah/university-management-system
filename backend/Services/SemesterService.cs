
// Services/Implementations/SemesterService.cs
using backend.Dtos;
using backend.Models;
using backend.Repositories;
using backend.Services;

namespace backend.Services
{
    public class SemesterService : ISemesterService
    {
        private readonly ISemesterRepository _repository;

        public SemesterService(ISemesterRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetSemesterDto> Create(CreateSemesterDto dto, int userId)
        {
            var semester = new Semester
            {
                Name = dto.Name,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                IsCurrent = dto.IsCurrent,
                RegistrationStart = dto.RegistrationStart,
                RegistrationEnd = dto.RegistrationEnd,
                AcademicYear = dto.AcademicYear
            };

            var created = await _repository.Create(semester, userId);
            return MapToDto(created);
        }

        public async Task<GetSemesterDto?> GetById(int id)
        {
            var semester = await _repository.GetById(id);
            return semester != null ? MapToDto(semester) : null;
        }

        public async Task<IEnumerable<GetSemesterDto>> GetAll()
        {
            var semesters = await _repository.GetAll();
            return semesters.Select(MapToDto);
        }

        public async Task<GetSemesterDto?> GetCurrent()
        {
            var semester = await _repository.GetCurrent();
            return semester != null ? MapToDto(semester) : null;
        }

        public async Task<GetSemesterDto> Update(int id, UpdateSemesterDto dto, int userId)
        {
            var existing = await _repository.GetById(id);
            if (existing == null)
            {
                throw new KeyNotFoundException("Semester not found");
            }

            existing.Name = dto.Name;
            existing.StartDate = dto.StartDate;
            existing.EndDate = dto.EndDate;
            existing.IsCurrent = dto.IsCurrent;
            existing.RegistrationStart = dto.RegistrationStart;
            existing.RegistrationEnd = dto.RegistrationEnd;
            existing.AcademicYear = dto.AcademicYear;

            var updated = await _repository.Update(existing, userId);
            return MapToDto(updated);
        }

        public async Task<bool> Delete(int id, int userId)
        {
            return await _repository.Delete(id, userId);
        }

        private GetSemesterDto MapToDto(Semester semester)
        {
            return new GetSemesterDto
            {
                Id = semester.Id,
                Name = semester.Name,
                StartDate = semester.StartDate,
                EndDate = semester.EndDate,
                IsCurrent = semester.IsCurrent,
                RegistrationStart = semester.RegistrationStart,
                RegistrationEnd = semester.RegistrationEnd,
                AcademicYear = semester.AcademicYear,
                CreatedAt = semester.CreatedAt,
                ModifiedAt = semester.ModifiedAt
            };
        }
    }
}