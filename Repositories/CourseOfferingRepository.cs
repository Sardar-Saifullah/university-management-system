
// Repositories/Implementations/CourseOfferingRepository.cs
using backend.Models;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text.Json;
using backend.Dtos;
using backend.Data;

namespace backend.Repositories
{
    public class CourseOfferingRepository : ICourseOfferingRepository
    {
        private readonly IDatabaseContext _context;

        public CourseOfferingRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public async Task<CourseSemesterOffering> Create(CourseSemesterOffering offering, int userId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_user_id", userId),
                new MySqlParameter("p_course_id", offering.CourseId),
                new MySqlParameter("p_semester_id", offering.SemesterId),
                new MySqlParameter("p_max_capacity", offering.MaxCapacity),
                new MySqlParameter("p_enrollment_start", offering.EnrollmentStart),
                new MySqlParameter("p_enrollment_end", offering.EnrollmentEnd),
                new MySqlParameter("p_offering_id", MySqlDbType.Int32) { Direction = ParameterDirection.Output }
            };

            await _context.ExecuteNonQueryAsync("sp_create_course_offering", parameters);

            offering.Id = Convert.ToInt32(parameters[6].Value);
            return offering;
        }

        public async Task<CourseSemesterOffering> Update(int id, CourseSemesterOffering offering, int userId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_user_id", userId),
                new MySqlParameter("p_offering_id", id),
                new MySqlParameter("p_max_capacity", offering.MaxCapacity),
                new MySqlParameter("p_enrollment_start", offering.EnrollmentStart),
                new MySqlParameter("p_enrollment_end", offering.EnrollmentEnd),
                new MySqlParameter("p_is_active", offering.IsActive)
            };

            await _context.ExecuteNonQueryAsync("sp_update_course_offering", parameters);
            return await GetById(id);
        }

        public async Task<bool> Delete(int id, int userId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_user_id", userId),
                new MySqlParameter("p_offering_id", id)
            };

            await _context.ExecuteNonQueryAsync("sp_delete_course_offering", parameters);
            return true;
        }

        public async Task<CourseSemesterOffering> GetById(int id)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_user_id", 1), // TODO: Replace with actual user
                new MySqlParameter("p_offering_id", id)
            };

            var result = await _context.ExecuteQueryAsync("sp_get_course_offering", parameters);

            if (result.Rows.Count == 0) return null;

            var row = result.Rows[0];
            return new CourseSemesterOffering
            {
                Id = Convert.ToInt32(row["id"]),
                CourseId = Convert.ToInt32(row["course_id"]),
                SemesterId = Convert.ToInt32(row["semester_id"]),
                MaxCapacity = Convert.ToInt32(row["max_capacity"]),
                CurrentEnrollment = Convert.ToInt32(row["current_enrollment"]),
                EnrollmentStart = row["enrollment_start"] != DBNull.Value ? Convert.ToDateTime(row["enrollment_start"]) : null,
                EnrollmentEnd = row["enrollment_end"] != DBNull.Value ? Convert.ToDateTime(row["enrollment_end"]) : null,
                IsActive = Convert.ToBoolean(row["is_active"])
            };
        }

        public async Task<IEnumerable<CourseSemesterOffering>> GetAll()
        {
            var parameters = new[]
            {
                new MySqlParameter("p_user_id", 1), // TODO: Replace with actual user
                new MySqlParameter("p_page", 1),
                new MySqlParameter("p_page_size", 100),
                new MySqlParameter("p_semester_id", null),
                new MySqlParameter("p_department_id", null),
                new MySqlParameter("p_program_id", null),
                new MySqlParameter("p_level_id", null),
                new MySqlParameter("p_is_active", null),
                new MySqlParameter("p_search_term", null)
            };

            var result = await _context.ExecuteQueryAsync("sp_list_course_offerings", parameters);

            var offerings = new List<CourseSemesterOffering>();
            foreach (DataRow row in result.Rows)
            {
                offerings.Add(new CourseSemesterOffering
                {
                    Id = Convert.ToInt32(row["id"]),
                    CourseId = Convert.ToInt32(row["course_id"]),
                    SemesterId = Convert.ToInt32(row["semester_id"]),
                    MaxCapacity = Convert.ToInt32(row["max_capacity"]),
                    CurrentEnrollment = Convert.ToInt32(row["current_enrollment"]),
                    EnrollmentStart = row["enrollment_start"] != DBNull.Value ? Convert.ToDateTime(row["enrollment_start"]) : null,
                    EnrollmentEnd = row["enrollment_end"] != DBNull.Value ? Convert.ToDateTime(row["enrollment_end"]) : null,
                    IsActive = Convert.ToBoolean(row["is_active"])
                });
            }

            return offerings;
        }

        public async Task<BulkUploadResultDto> BulkUpload(JsonDocument jsonData, int userId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_user_id", userId),
                new MySqlParameter("p_json_data", jsonData.RootElement.ToString()),
                new MySqlParameter("p_success_count", MySqlDbType.Int32) { Direction = ParameterDirection.Output },
                new MySqlParameter("p_error_count", MySqlDbType.Int32) { Direction = ParameterDirection.Output },
                new MySqlParameter("p_error_messages", MySqlDbType.JSON) { Direction = ParameterDirection.Output }
            };

            await _context.ExecuteNonQueryAsync("sp_bulk_upload_course_offerings", parameters);

            var result = new BulkUploadResultDto
            {
                SuccessCount = Convert.ToInt32(parameters[2].Value),
                ErrorCount = Convert.ToInt32(parameters[3].Value),
                Errors = JsonSerializer.Deserialize<List<BulkUploadErrorDto>>(parameters[4].Value?.ToString() ?? "[]")
            };

            return result;
        }
    }
}