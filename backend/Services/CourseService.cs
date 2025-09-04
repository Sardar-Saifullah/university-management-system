using backend.Dtos;
using backend.Models;
using backend.Repositories;
using backend.Utilities;
using Microsoft.AspNetCore.Http;
using System.Data;

namespace backend.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IJsonFileProcessor _jsonFileProcessor;

        public CourseService(ICourseRepository courseRepository, IJsonFileProcessor jsonFileProcessor)
        {
            _courseRepository = courseRepository;
            _jsonFileProcessor = jsonFileProcessor;
        }

        public async Task<CourseResponseDto> CreateCourse(CourseCreateDto dto, int userId)
        {
            var course = new Course
            {
                Code = dto.Code,
                Title = dto.Title,
                Description = dto.Description,
                CreditHours = dto.CreditHours,
                LevelId = dto.LevelId,
                ProgramId = dto.ProgramId,
                DepartmentId = dto.DepartmentId,
                IsElective = dto.IsElective,
                CreatedBy = userId
            };

            var id = await _courseRepository.CreateCourse(course);
            return await GetCourse(id);
        }

        public async Task<CourseResponseDto> GetCourse(int id)
        {
            var course = await _courseRepository.GetCourseById(id);
            if (course == null) return null;

            return new CourseResponseDto
            {
                Id = course.Id,
                Code = course.Code,
                Title = course.Title,
                Description = course.Description,
                CreditHours = course.CreditHours,
                LevelId = course.LevelId,
                DepartmentId = course.DepartmentId,
                ProgramId = course.ProgramId,
                IsElective = course.IsElective,
                IsActive = course.IsActive,
                CreatedAt = course.CreatedAt,
                ModifiedAt = course.ModifiedAt
            };
        }

        public async Task<CourseListResponseDto> GetCourses(int page, int pageSize, int? departmentId, int? programId, int? levelId, bool? isElective, string searchTerm)
        {
            return await _courseRepository.GetFilteredCourses(
                page, pageSize, departmentId, programId, levelId, isElective, searchTerm);
        }

        public async Task<CourseResponseDto> UpdateCourse(int id, CourseUpdateDto dto, int userId)
        {
            var existingCourse = await _courseRepository.GetCourseById(id);
            if (existingCourse == null) return null;

            existingCourse.Title = dto.Title;
            existingCourse.Description = dto.Description;
            existingCourse.CreditHours = dto.CreditHours;
            existingCourse.LevelId = dto.LevelId;
            existingCourse.ProgramId = dto.ProgramId;
            existingCourse.DepartmentId = dto.DepartmentId;
            existingCourse.IsElective = dto.IsElective;
            existingCourse.IsActive = dto.IsActive;
            existingCourse.ModifiedBy = userId;

            var success = await _courseRepository.UpdateCourse(existingCourse);
            if (!success) return null;

            return await GetCourse(id);
        }

        public async Task<bool> DeleteCourse(int id, int userId)
        {
            return await _courseRepository.DeleteCourse(id, userId);
        }

        public async Task<BulkUploadResultDto> BulkUploadCourses(IFormFile file, int userId)
        {
            var jsonDoc = await _jsonFileProcessor.ProcessJsonFile(file);
            var jsonArray = jsonDoc.RootElement.EnumerateArray();

            var dataTable = new DataTable();
            dataTable.Columns.Add("code", typeof(string));
            dataTable.Columns.Add("title", typeof(string));
            dataTable.Columns.Add("description", typeof(string));
            dataTable.Columns.Add("credit_hours", typeof(decimal));
            dataTable.Columns.Add("level_id", typeof(int));
            dataTable.Columns.Add("program_id", typeof(int));
            dataTable.Columns.Add("dep_id", typeof(int));
            dataTable.Columns.Add("is_elective", typeof(bool));

            foreach (var item in jsonArray)
            {
                dataTable.Rows.Add(
                    item.GetProperty("code").GetString(),
                    item.GetProperty("title").GetString(),
                    item.TryGetProperty("description", out var desc) ? desc.GetString() : null,
                    item.GetProperty("credit_hours").GetDecimal(),
                    item.GetProperty("level_id").GetInt32(),
                    item.TryGetProperty("program_id", out var prog) ? prog.GetInt32() : (object)DBNull.Value,
                    item.GetProperty("dep_id").GetInt32(),
                    item.TryGetProperty("is_elective", out var elective) ? elective.GetBoolean() : false
                );
            }

            var successCount = await _courseRepository.BulkUploadCourses(dataTable, userId);
            return new BulkUploadResultDto
            {
                SuccessCount = successCount,
                ErrorCount = jsonArray.Count() - successCount,
                Errors = new List<BulkUploadErrorDto>()
            };
        }

        public async Task<IEnumerable<PrerequisiteDto>> GetCoursePrerequisites(int courseId)
        {
            var prerequisites = await _courseRepository.GetPrerequisitesForCourse(courseId);
            return prerequisites.Select(p => new PrerequisiteDto
            {
                CourseId = p.CourseId,
                PrerequisiteCourseId = p.PrerequisiteCourseId,
                IsMandatory = p.IsMandatory,
                MinimumGrade = p.MinimumGrade
            });
        }

        public async Task<PrerequisiteDto> AddPrerequisite(PrerequisiteDto dto, int userId)
        {
            var prerequisite = new CoursePrerequisite
            {
                CourseId = dto.CourseId,
                PrerequisiteCourseId = dto.PrerequisiteCourseId,
                IsMandatory = dto.IsMandatory,
                MinimumGrade = dto.MinimumGrade,
                CreatedBy = userId
            };

            var success = await _courseRepository.AddPrerequisite(prerequisite);
            if (!success) return null;

            return dto;
        }

        public async Task<bool> RemovePrerequisite(int prerequisiteId, int userId)
        {
            return await _courseRepository.RemovePrerequisite(prerequisiteId, userId);
        }
    }
}