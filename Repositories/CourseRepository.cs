using backend.Data;
using backend.Dtos;
using backend.Models;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System.Data;

namespace backend.Repositories
{
   

    public class CourseRepository : ICourseRepository
    {
        private readonly IDatabaseContext _context;

        public CourseRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public async Task<int> CreateCourse(Course course)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_user_id", course.CreatedBy),
                new MySqlParameter("p_code", course.Code),
                new MySqlParameter("p_title", course.Title),
                new MySqlParameter("p_description", course.Description ?? (object)DBNull.Value),
                new MySqlParameter("p_credit_hours", course.CreditHours),
                new MySqlParameter("p_level_id", course.LevelId),
                new MySqlParameter("p_program_id", course.ProgramId ?? (object)DBNull.Value),
                new MySqlParameter("p_dep_id", course.DepartmentId),
                new MySqlParameter("p_is_elective", course.IsElective),
                new MySqlParameter("p_course_id", MySqlDbType.Int32) { Direction = ParameterDirection.Output }
            };

            await _context.ExecuteNonQueryAsync("sp_create_course", parameters);

            return Convert.ToInt32(parameters[9].Value);
        }

        public async Task<Course> GetCourseById(int id, int userId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_user_id", userId),
                new MySqlParameter("p_course_id", id)
            };

            var result = await _context.ExecuteQueryAsync("sp_get_course_by_id", parameters);

            if (result.Rows.Count == 0) return null;

            var row = result.Rows[0];
            return new Course
            {
                Id = Convert.ToInt32(row["id"]),
                Code = row["code"].ToString(),
                Title = row["title"].ToString(),
                Description = row["description"].ToString(),
                CreditHours = Convert.ToDecimal(row["credit_hours"]),
                LevelId = Convert.ToInt32(row["level_id"]),
                LevelName = row["level_name"].ToString(),
                ProgramId = row["program_id"] != DBNull.Value ? Convert.ToInt32(row["program_id"]) : (int?)null,
                ProgramName = row["program_name"]?.ToString(),
                DepartmentId = Convert.ToInt32(row["dep_id"]),
                DepartmentName = row["department_name"].ToString(),
                DepartmentCode = row["department_code"].ToString(),
                IsElective = Convert.ToBoolean(row["is_elective"]),
                IsActive = Convert.ToBoolean(row["is_active"]),
                CreatedAt = Convert.ToDateTime(row["created_at"]),
                ModifiedAt = Convert.ToDateTime(row["modified_at"]),
                ActiveOfferingsCount = Convert.ToInt32(row["active_offerings_count"])
            };
        }

        public async Task<bool> UpdateCourse(Course course)
        {
            var parameters = new[]
            {
        new MySqlParameter("p_user_id", course.ModifiedBy),
        new MySqlParameter("p_course_id", course.Id),
        new MySqlParameter("p_title", course.Title),
        new MySqlParameter("p_description", course.Description ?? (object)DBNull.Value),
        new MySqlParameter("p_credit_hours", course.CreditHours),
        new MySqlParameter("p_level_id", course.LevelId),
        new MySqlParameter("p_program_id", course.ProgramId ?? (object)DBNull.Value),
        new MySqlParameter("p_dep_id", course.DepartmentId),
        new MySqlParameter("p_is_elective", course.IsElective),
        new MySqlParameter("p_is_active", course.IsActive)
    };

            // Execute query and get the result set instead of just affected rows
            var result = await _context.ExecuteQueryAsync("sp_update_course", parameters);

            // Check if we got a result and if affected_rows > 0
            if (result.Rows.Count > 0)
            {
                var affectedRows = Convert.ToInt32(result.Rows[0]["affected_rows"]);
                return affectedRows > 0;
            }

            return false;
        }

       public async Task<bool> DeleteCourse(int id, int userId)
{
    var parameters = new[]
    {
        new MySqlParameter("p_user_id", userId),
        new MySqlParameter("p_course_id", id)
    };

    // Execute query and get the result set instead of just affected rows
    var result = await _context.ExecuteQueryAsync("sp_delete_course", parameters);
    
    // Check if we got a result and if affected_rows > 0
    if (result.Rows.Count > 0)
    {
        var affectedRows = Convert.ToInt32(result.Rows[0]["affected_rows"]);
        return affectedRows > 0;
    }
    
    return false;
}
        public async Task<CourseListResponseDto> GetFilteredCourses(int userId, int page, int pageSize, int? departmentId, int? programId, int? levelId, bool? isElective, string searchTerm, bool onlyActive)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_user_id", userId),
                new MySqlParameter("p_page", page),
                new MySqlParameter("p_page_size", pageSize),
                new MySqlParameter("p_department_id", departmentId ?? (object)DBNull.Value),
                new MySqlParameter("p_program_id", programId ?? (object)DBNull.Value),
                new MySqlParameter("p_level_id", levelId ?? (object)DBNull.Value),
                new MySqlParameter("p_is_elective", isElective ?? (object)DBNull.Value),
                new MySqlParameter("p_search_term", searchTerm ?? (object)DBNull.Value),
                new MySqlParameter("p_only_active", onlyActive)
            };

            var result = await _context.ExecuteQueryAsync("sp_get_all_courses", parameters);

            var courses = new List<CourseResponseDto>();
            int totalCount = 0;

            foreach (DataRow row in result.Rows)
            {
                courses.Add(new CourseResponseDto
                {
                    Id = Convert.ToInt32(row["id"]),
                    Code = row["code"].ToString(),
                    Title = row["title"].ToString(),
                    Description = row["description"].ToString(),
                    CreditHours = Convert.ToDecimal(row["credit_hours"]),
                    LevelId = Convert.ToInt32(row["level_id"]),
                    LevelName = row["level_name"].ToString(),
                    ProgramId = row["program_id"] != DBNull.Value ? Convert.ToInt32(row["program_id"]) : (int?)null,
                    ProgramName = row["program_name"]?.ToString(),
                    DepartmentId = Convert.ToInt32(row["dep_id"]),
                    DepartmentName = row["department_name"].ToString(),
                    DepartmentCode = row["department_code"].ToString(),
                    IsElective = Convert.ToBoolean(row["is_elective"]),
                    IsActive = Convert.ToBoolean(row["is_active"]),
                    CreatedAt = Convert.ToDateTime(row["created_at"]),
                    ModifiedAt = Convert.ToDateTime(row["modified_at"]),
                    ActiveOfferingsCount = Convert.ToInt32(row["active_offerings_count"])
                });

                totalCount = Convert.ToInt32(row["total_count"]);
            }

            return new CourseListResponseDto
            {
                Courses = courses,
                TotalCount = totalCount
            };
        }

        public async Task<BulkUploadResultDto> BulkUploadCourses(string jsonData, int userId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_user_id", userId),
                new MySqlParameter("p_json_data", jsonData),
                new MySqlParameter("p_success_count", MySqlDbType.Int32) { Direction = ParameterDirection.Output },
                new MySqlParameter("p_error_count", MySqlDbType.Int32) { Direction = ParameterDirection.Output },
                new MySqlParameter("p_error_messages", MySqlDbType.JSON) { Direction = ParameterDirection.Output }
            };

            await _context.ExecuteNonQueryAsync("sp_bulk_upload_courses", parameters);

            var successCount = Convert.ToInt32(parameters[2].Value);
            var errorCount = Convert.ToInt32(parameters[3].Value);
            var errorMessagesJson = parameters[4].Value?.ToString();

            var errors = new List<BulkUploadErrorDto>();
            if (!string.IsNullOrEmpty(errorMessagesJson))
            {
                var errorArray = Newtonsoft.Json.Linq.JArray.Parse(errorMessagesJson);
                foreach (var error in errorArray)
                {
                    errors.Add(new BulkUploadErrorDto
                    {
                        Index = error["index"]?.Value<int>() ?? 0,
                        Code = error["code"]?.Value<string>() ?? "unknown",
                        Message = error["message"]?.Value<string>() ?? "Unknown error"
                    });
                }
            }

            return new BulkUploadResultDto
            {
                SuccessCount = successCount,
                ErrorCount = errorCount,
                Errors = errors
            };
        }

        public async Task<CourseListResponseDto> SearchCoursesLightweight(int userId, string searchTerm, int? departmentId, int? levelId, bool? isElective, int page, int pageSize)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_user_id", userId),
                new MySqlParameter("p_search_term", searchTerm ?? (object)DBNull.Value),
                new MySqlParameter("p_department_id", departmentId ?? (object)DBNull.Value),
                new MySqlParameter("p_level_id", levelId ?? (object)DBNull.Value),
                new MySqlParameter("p_is_elective", isElective ?? (object)DBNull.Value),
                new MySqlParameter("p_page", page),
                new MySqlParameter("p_page_size", pageSize)
            };

            var result = await _context.ExecuteQueryAsync("sp_search_courses_lightweight", parameters);

            var courses = new List<CourseResponseDto>();
            int totalCount = 0;

            foreach (DataRow row in result.Rows)
            {
                courses.Add(new CourseResponseDto
                {
                    Id = Convert.ToInt32(row["id"]),
                    Code = row["code"].ToString(),
                    Title = row["title"].ToString(),
                    CreditHours = Convert.ToDecimal(row["credit_hours"]),
                    LevelId = Convert.ToInt32(row["level_id"]),
                    LevelName = row["level_name"].ToString(),
                    DepartmentId = Convert.ToInt32(row["dep_id"]),
                    DepartmentName = row["department_name"].ToString(),
                    DepartmentCode = row["department_code"].ToString(),
                    IsElective = Convert.ToBoolean(row["is_elective"]),
                    IsActive = Convert.ToBoolean(row["is_active"])
                });

                totalCount = Convert.ToInt32(row["total_count"]);
            }

            return new CourseListResponseDto
            {
                Courses = courses,
                TotalCount = totalCount
            };
        }
        public async Task<Course> GetById(int courseId)
        {
            // Use a default user ID (like admin ID 1) since this is an internal call
            var parameters = new[]
            {
        new MySqlParameter("p_user_id", 1), // Default admin user
        new MySqlParameter("p_course_id", courseId)
    };

            var result = await _context.ExecuteQueryAsync("sp_get_course_by_id", parameters);

            if (result.Rows.Count == 0) return null;

            var row = result.Rows[0];
            return new Course
            {
                Id = Convert.ToInt32(row["id"]),
                Code = row["code"].ToString(),
                Title = row["title"].ToString(),
                Description = row["description"].ToString(),
                CreditHours = Convert.ToDecimal(row["credit_hours"]),
                LevelId = Convert.ToInt32(row["level_id"]),
                LevelName = row["level_name"].ToString(),
                ProgramId = row["program_id"] != DBNull.Value ? Convert.ToInt32(row["program_id"]) : (int?)null,
                ProgramName = row["program_name"]?.ToString(),
                DepartmentId = Convert.ToInt32(row["dep_id"]),
                DepartmentName = row["department_name"].ToString(),
                DepartmentCode = row["department_code"].ToString(),
                IsElective = Convert.ToBoolean(row["is_elective"]),
                IsActive = Convert.ToBoolean(row["is_active"]),
                CreatedAt = Convert.ToDateTime(row["created_at"]),
                ModifiedAt = Convert.ToDateTime(row["modified_at"]),
                ActiveOfferingsCount = Convert.ToInt32(row["active_offerings_count"])
            };
        }
    }
}