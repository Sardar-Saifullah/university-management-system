using backend.Dtos;
using backend.Models;
using backend.Repositories;
using System.Text.Json;

namespace backend.Services
{
  

    public class CourseOfferingService : ICourseOfferingService
    {
        private readonly ICourseOfferingRepository _repository;
        private readonly ICourseRepository _courseRepository;
        private readonly ISemesterRepository _semesterRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IAcademicProgramRepository _programRepository;
        private readonly ILevelRepository _levelRepository;

        public CourseOfferingService(
            ICourseOfferingRepository repository,
            ICourseRepository courseRepository,
            ISemesterRepository semesterRepository,
            IDepartmentRepository departmentRepository,
            IAcademicProgramRepository programRepository,
            ILevelRepository levelRepository)
        {
            _repository = repository;
            _courseRepository = courseRepository;
            _semesterRepository = semesterRepository;
            _departmentRepository = departmentRepository;
            _programRepository = programRepository;
            _levelRepository = levelRepository;
        }

        public async Task<CourseOfferingGetDto> Create(CourseOfferingCreateDto dto, int userId)
        {
            var offering = new CourseSemesterOffering
            {
                CourseId = dto.CourseId,
                SemesterId = dto.SemesterId,
                MaxCapacity = dto.MaxCapacity,
                EnrollmentStart = dto.EnrollmentStart,
                EnrollmentEnd = dto.EnrollmentEnd,
                CreatedBy = userId
            };

            var created = await _repository.Create(offering, userId);
            return await MapToDto(created);
        }

        public async Task<CourseOfferingGetDto> Update(int id, CourseOfferingUpdateDto dto, int userId)
        {
            var existing = await _repository.GetById(id);
            if (existing == null) return null;

            if (dto.MaxCapacity.HasValue) existing.MaxCapacity = dto.MaxCapacity.Value;
            if (dto.EnrollmentStart.HasValue) existing.EnrollmentStart = dto.EnrollmentStart.Value;
            if (dto.EnrollmentEnd.HasValue) existing.EnrollmentEnd = dto.EnrollmentEnd.Value;
            if (dto.IsActive.HasValue) existing.IsActive = dto.IsActive.Value;

            var updated = await _repository.Update(id, existing, userId);
            return await MapToDto(updated);
        }

        public async Task<bool> Delete(int id, int userId)
        {
            return await _repository.Delete(id, userId);
        }

        public async Task<CourseOfferingGetDto> GetById(int id)
        {
            var offering = await _repository.GetById(id);
            return await MapToDto(offering);
        }

        public async Task<IEnumerable<CourseOfferingGetDto>> GetAll()
        {
            var offerings = await _repository.GetAll();
            var dtos = new List<CourseOfferingGetDto>();

            foreach (var offering in offerings)
            {
                dtos.Add(await MapToDto(offering));
            }

            return dtos;
        }

        public async Task<BulkUploadResultDto> BulkUpload(JsonDocument jsonData, int userId)
        {
            return await _repository.BulkUpload(jsonData, userId);
        }

        private async Task<CourseOfferingGetDto> MapToDto(CourseSemesterOffering offering)
        {
            if (offering == null) return null;

            var course = await _courseRepository.GetById(offering.CourseId);
            var semester = await _semesterRepository.GetById(offering.SemesterId);

            if (course == null || semester == null) return null;

            string programName = null;
            if (course.ProgramId.HasValue)
            {
                var program = await _programRepository.GetByIdAsync(course.ProgramId.Value);
                programName = program?.Name;
            }

            var department = await _departmentRepository.GetById(course.DepartmentId);
            var level = await _levelRepository.GetById(course.LevelId);

            return new CourseOfferingGetDto
            {
                Id = offering.Id,
                CourseId = offering.CourseId,
                CourseCode = course.Code,
                CourseTitle = course.Title,
                SemesterId = offering.SemesterId,
                SemesterName = semester.Name,
                SemesterStartDate = semester.StartDate,
                SemesterEndDate = semester.EndDate,
                MaxCapacity = offering.MaxCapacity,
                CurrentEnrollment = offering.CurrentEnrollment,
                EnrollmentStart = offering.EnrollmentStart,
                EnrollmentEnd = offering.EnrollmentEnd,
                IsActive = offering.IsActive,
                DepartmentId = course.DepartmentId,
                DepartmentName = department?.Name,
                ProgramId = course.ProgramId,
                ProgramName = programName,
                LevelId = course.LevelId,
                LevelName = level?.Name,
                CreditHours = course.CreditHours
            };
        }
    }
}