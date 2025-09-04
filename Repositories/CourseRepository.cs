using backend.Data;
using backend.Dtos;
using backend.Models;
using MySql.Data.MySqlClient;
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
                new MySqlParameter("@p_user_id", course.CreatedBy),
                new MySqlParameter("@p_code", course.Code),
                new MySqlParameter("@p_title", course.Title),
                new MySqlParameter("@p_description", course.Description ?? (object)DBNull.Value),
                new MySqlParameter("@p_credit_hours", course.CreditHours),
                new MySqlParameter("@p_level_id", course.LevelId),
                new MySqlParameter("@p_program_id", course.ProgramId ?? (object)DBNull.Value),
                new MySqlParameter("@p_dep_id", course.DepartmentId),
                new MySqlParameter("@p_is_elective", course.IsElective),
                new MySqlParameter("@p_course_id", MySqlDbType.Int32) { Direction = ParameterDirection.Output }
            };

            await _context.ExecuteNonQueryAsync("sp_create_course", parameters);

            return Convert.ToInt32(parameters[9].Value);
        }

        public async Task<Course> GetCourseById(int id)
        {
            var parameters = new[] { new MySqlParameter("@p_course_id", id) };
            var result = await _context.ExecuteQueryAsync("sp_get_course", parameters);

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
                ProgramId = row["program_id"] != DBNull.Value ? Convert.ToInt32(row["program_id"]) : (int?)null,
                DepartmentId = Convert.ToInt32(row["dep_id"]),
                IsElective = Convert.ToBoolean(row["is_elective"]),
                IsActive = Convert.ToBoolean(row["is_active"]),
                CreatedAt = Convert.ToDateTime(row["created_at"]),
                ModifiedAt = Convert.ToDateTime(row["modified_at"])
            };
        }

        public async Task<bool> UpdateCourse(Course course)
        {
            var parameters = new[]
            {
                new MySqlParameter("@p_user_id", course.ModifiedBy),
                new MySqlParameter("@p_course_id", course.Id),
                new MySqlParameter("@p_title", course.Title),
                new MySqlParameter("@p_description", course.Description ?? (object)DBNull.Value),
                new MySqlParameter("@p_credit_hours", course.CreditHours),
                new MySqlParameter("@p_level_id", course.LevelId),
                new MySqlParameter("@p_program_id", course.ProgramId ?? (object)DBNull.Value),
                new MySqlParameter("@p_dep_id", course.DepartmentId),
                new MySqlParameter("@p_is_elective", course.IsElective),
                new MySqlParameter("@p_is_active", course.IsActive)
            };

            var affectedRows = await _context.ExecuteNonQueryAsync("sp_update_course", parameters);
            return affectedRows > 0;
        }

        public async Task<bool> DeleteCourse(int id, int userId)
        {
            var parameters = new[]
            {
                new MySqlParameter("@p_user_id", userId),
                new MySqlParameter("@p_course_id", id)
            };

            var affectedRows = await _context.ExecuteNonQueryAsync("sp_delete_course", parameters);
            return affectedRows > 0;
        }

        public async Task<CourseListResponseDto> GetFilteredCourses(int page, int pageSize, int? departmentId, int? programId, int? levelId, bool? isElective, string searchTerm)
        {
            var parameters = new[]
            {
                new MySqlParameter("@p_user_id", 1), // Assuming admin access
                new MySqlParameter("@p_page", page),
                new MySqlParameter("@p_page_size", pageSize),
                new MySqlParameter("@p_department_id", departmentId ?? (object)DBNull.Value),
                new MySqlParameter("@p_program_id", programId ?? (object)DBNull.Value),
                new MySqlParameter("@p_level_id", levelId ?? (object)DBNull.Value),
                new MySqlParameter("@p_is_elective", isElective ?? (object)DBNull.Value),
                new MySqlParameter("@p_search_term", searchTerm ?? (object)DBNull.Value)
            };

            var result = await _context.ExecuteQueryAsync("sp_list_courses", parameters);

            var courses = new List<CourseResponseDto>();
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
                    IsElective = Convert.ToBoolean(row["is_elective"]),
                    IsActive = Convert.ToBoolean(row["is_active"]),
                    CreatedAt = Convert.ToDateTime(row["created_at"]),
                    ModifiedAt = Convert.ToDateTime(row["modified_at"])
                });
            }

            var totalCount = result.Rows.Count > 0 ? Convert.ToInt32(result.Rows[0]["total_count"]) : 0;

            return new CourseListResponseDto
            {
                Courses = courses,
                TotalCount = totalCount
            };
        }

        public async Task<int> BulkUploadCourses(DataTable courses, int userId)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(courses);
            var parameters = new[]
            {
                new MySqlParameter("@p_user_id", userId),
                new MySqlParameter("@p_json_data", json),
                new MySqlParameter("@p_success_count", MySqlDbType.Int32) { Direction = ParameterDirection.Output },
                new MySqlParameter("@p_error_count", MySqlDbType.Int32) { Direction = ParameterDirection.Output },
                new MySqlParameter("@p_error_messages", MySqlDbType.JSON) { Direction = ParameterDirection.Output }
            };

            await _context.ExecuteNonQueryAsync("sp_bulk_upload_courses", parameters);

            return Convert.ToInt32(parameters[2].Value);
        }

        public async Task<IEnumerable<CoursePrerequisite>> GetPrerequisitesForCourse(int courseId)
        {
            var parameters = new[] { new MySqlParameter("@p_course_id", courseId) };
            var result = await _context.ExecuteQueryAsync("sp_get_course_prerequisites", parameters);

            var prerequisites = new List<CoursePrerequisite>();
            foreach (DataRow row in result.Rows)
            {
                prerequisites.Add(new CoursePrerequisite
                {
                    Id = Convert.ToInt32(row["id"]),
                    CourseId = Convert.ToInt32(row["course_id"]),
                    PrerequisiteCourseId = Convert.ToInt32(row["prerequisite_course_id"]),
                    IsMandatory = Convert.ToBoolean(row["is_mandatory"]),
                    MinimumGrade = row["minimum_grade"] != DBNull.Value ? Convert.ToDecimal(row["minimum_grade"]) : (decimal?)null,
                    IsActive = Convert.ToBoolean(row["is_active"])
                });
            }

            return prerequisites;
        }

        public async Task<bool> AddPrerequisite(CoursePrerequisite prerequisite)
        {
            var parameters = new[]
            {
                new MySqlParameter("@p_course_id", prerequisite.CourseId),
                new MySqlParameter("@p_prerequisite_course_id", prerequisite.PrerequisiteCourseId),
                new MySqlParameter("@p_is_mandatory", prerequisite.IsMandatory),
                new MySqlParameter("@p_minimum_grade", prerequisite.MinimumGrade ?? (object)DBNull.Value),
                new MySqlParameter("@p_created_by", prerequisite.CreatedBy)
            };

            var affectedRows = await _context.ExecuteNonQueryAsync("sp_add_course_prerequisite", parameters);
            return affectedRows > 0;
        }

        public async Task<bool> RemovePrerequisite(int prerequisiteId, int userId)
        {
            var parameters = new[]
            {
                new MySqlParameter("@p_prerequisite_id", prerequisiteId),
                new MySqlParameter("@p_user_id", userId)
            };

            var affectedRows = await _context.ExecuteNonQueryAsync("sp_remove_course_prerequisite", parameters);
            return affectedRows > 0;
        }

        public async Task<IEnumerable<Course>> GetAllCourses()
        {
            var result = await _context.ExecuteQueryAsync("sp_get_all_courses", Array.Empty<MySqlParameter>());

            var courses = new List<Course>();
            foreach (DataRow row in result.Rows)
            {
                courses.Add(new Course
                {
                    Id = Convert.ToInt32(row["id"]),
                    Code = row["code"].ToString(),
                    Title = row["title"].ToString(),
                    Description = row["description"].ToString(),
                    CreditHours = Convert.ToDecimal(row["credit_hours"]),
                    LevelId = Convert.ToInt32(row["level_id"]),
                    ProgramId = row["program_id"] != DBNull.Value ? Convert.ToInt32(row["program_id"]) : (int?)null,
                    DepartmentId = Convert.ToInt32(row["dep_id"]),
                    IsElective = Convert.ToBoolean(row["is_elective"]),
                    IsActive = Convert.ToBoolean(row["is_active"]),
                    CreatedAt = Convert.ToDateTime(row["created_at"]),
                    ModifiedAt = Convert.ToDateTime(row["modified_at"])
                });
            }

            return courses;
        }
    }
}