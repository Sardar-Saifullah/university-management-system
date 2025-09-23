using backend.Dtos;
using backend.Models;
using backend.Repositories;
using backend.Utilities;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

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
            return await GetCourse(id, userId);
        }

        public async Task<CourseResponseDto> GetCourse(int id, int userId)
        {
            var course = await _courseRepository.GetCourseById(id, userId);
            if (course == null) return null;

            return new CourseResponseDto
            {
                Id = course.Id,
                Code = course.Code,
                Title = course.Title,
                Description = course.Description,
                CreditHours = course.CreditHours,
                LevelId = course.LevelId,
                LevelName = course.LevelName,
                ProgramId = course.ProgramId,
                ProgramName = course.ProgramName,
                DepartmentId = course.DepartmentId,
                DepartmentName = course.DepartmentName,
                DepartmentCode = course.DepartmentCode,
                IsElective = course.IsElective,
                IsActive = course.IsActive,
                CreatedAt = course.CreatedAt,
                ModifiedAt = course.ModifiedAt,
                ActiveOfferingsCount = course.ActiveOfferingsCount
            };
        }

        public async Task<CourseListResponseDto> GetCourses(int userId, int page, int pageSize, int? departmentId, int? programId, int? levelId, bool? isElective, string searchTerm, bool onlyActive)
        {
            return await _courseRepository.GetFilteredCourses(
                userId, page, pageSize, departmentId, programId, levelId, isElective, searchTerm, onlyActive);
        }

        public async Task<CourseResponseDto> UpdateCourse(int id, CourseUpdateDto dto, int userId)
        {
            try
            {
                var existingCourse = await _courseRepository.GetCourseById(id, userId);
                if (existingCourse == null) return null;

                // Only update provided fields
                if (!string.IsNullOrEmpty(dto.Title)) existingCourse.Title = dto.Title;
                if (dto.Description != null) existingCourse.Description = dto.Description;
                if (dto.CreditHours > 0) existingCourse.CreditHours = dto.CreditHours;
                if (dto.LevelId > 0) existingCourse.LevelId = dto.LevelId;
                if (dto.ProgramId.HasValue) existingCourse.ProgramId = dto.ProgramId;
                if (dto.DepartmentId > 0) existingCourse.DepartmentId = dto.DepartmentId;

                // For boolean values in the Course model (which are non-nullable), we can assign directly
                // since the DTO properties are required and will always have values
                existingCourse.IsElective = dto.IsElective;
                existingCourse.IsActive = dto.IsActive;

                existingCourse.ModifiedBy = userId;

                var success = await _courseRepository.UpdateCourse(existingCourse);
                if (!success) return null;

                return await GetCourse(id, userId);
            }
            catch (ApplicationException ex)
            {
                // Log the error and rethrow or handle appropriately
                throw new ApplicationException($"Failed to update course: {ex.Message}");
            }
        }

        public async Task<bool> DeleteCourse(int id, int userId)
        {
            try { 
            return await _courseRepository.DeleteCourse(id, userId);}
            catch (ApplicationException ex)
    {
                // Log the error and rethrow or handle appropriately
                throw new ApplicationException($"Failed to delete course: {ex.Message}");
            }
        }

        public async Task<BulkUploadResultDto> BulkUploadCourses(IFormFile file, int userId)
        {
            var jsonDoc = await _jsonFileProcessor.ProcessJsonFile(file);
            var jsonArray = jsonDoc.RootElement;

            // Convert to JSON string for stored procedure
            var jsonData = jsonArray.ToString();

            return await _courseRepository.BulkUploadCourses(jsonData, userId);
        }

        public async Task<CourseListResponseDto> SearchCoursesLightweight(int userId, string searchTerm, int? departmentId, int? levelId, bool? isElective, int page, int pageSize)
        {
            return await _courseRepository.SearchCoursesLightweight(
                userId, searchTerm, departmentId, levelId, isElective, page, pageSize);
        }
    }
}