// Repositories/PrerequisiteRepository.cs
using backend.Data;
using backend.Dtos;
using backend.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace backend.Repositories
{
    public class PrerequisiteRepository : IPrerequisiteRepository
    {
        private readonly IDatabaseContext _context;

        public PrerequisiteRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public async Task AddPrerequisite(CoursePrerequisite prerequisite, int userId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_user_id", userId),
                new MySqlParameter("p_course_id", prerequisite.CourseId),
                new MySqlParameter("p_prerequisite_course_id", prerequisite.PrerequisiteCourseId),
                new MySqlParameter("p_is_mandatory", prerequisite.IsMandatory),
                new MySqlParameter("p_minimum_grade", prerequisite.MinimumGrade ?? (object)DBNull.Value)
            };

            await _context.ExecuteNonQueryAsync("sp_add_course_prerequisite", parameters);
        }

        public async Task RemovePrerequisite(int prerequisiteId, int userId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_user_id", userId),
                new MySqlParameter("p_prerequisite_id", prerequisiteId)
            };

            await _context.ExecuteNonQueryAsync("sp_remove_course_prerequisite", parameters);
        }

        public async Task<List<PrerequisiteDto>> GetPrerequisites(int courseId, bool includeInactive, int userId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_user_id", userId),
                new MySqlParameter("p_course_id", courseId),
                new MySqlParameter("p_include_inactive", includeInactive)
            };

            var result = await _context.ExecuteQueryAsync("sp_get_course_prerequisites", parameters);

            var prerequisites = new List<PrerequisiteDto>();
            foreach (DataRow row in result.Rows)
            {
                prerequisites.Add(new PrerequisiteDto
                {
                    CourseId = Convert.ToInt32(row["id"]),
                    PrerequisiteCourseId = Convert.ToInt32(row["prerequisite_course_id"]),
                    PrerequisiteCode = row["prerequisite_code"].ToString(),
                    PrerequisiteTitle = row["prerequisite_title"].ToString(),
                    PrerequisiteLevel = row["prerequisite_level"].ToString(),
                    PrerequisiteCredits = Convert.ToDecimal(row["prerequisite_credits"]),
                    IsMandatory = Convert.ToBoolean(row["is_mandatory"]),
                    MinimumGrade = row["minimum_grade"] != DBNull.Value ? Convert.ToDecimal(row["minimum_grade"]) : null,
                    GradeRequirementStatus = row["grade_requirement_status"].ToString(),
                    CreatedBy = row["created_by"].ToString(),
                    ModifiedBy = row["modified_by"] != DBNull.Value ? row["modified_by"].ToString() : null,
                    IsActive = Convert.ToBoolean(row["is_active"])
                });
            }

            return prerequisites;
        }

        public async Task<PrerequisiteValidationResult> ValidatePrerequisites(int studentId, int courseId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_student_id", studentId),
                new MySqlParameter("p_course_id", courseId),
                new MySqlParameter("p_is_eligible", MySqlDbType.Bit) { Direction = ParameterDirection.Output },
                new MySqlParameter("p_missing_prerequisites", MySqlDbType.JSON) { Direction = ParameterDirection.Output }
            };

            await _context.ExecuteNonQueryAsync("sp_validate_prerequisites_for_enrollment", parameters);

            var result = new PrerequisiteValidationResult
            {
                IsEligible = Convert.ToBoolean(parameters[2].Value)
            };

            if (parameters[3].Value != DBNull.Value)
            {
                var json = parameters[3].Value.ToString();
                if (!string.IsNullOrEmpty(json))
                {
                    var missingPrereqs = System.Text.Json.JsonSerializer.Deserialize<List<MissingPrerequisite>>(json);
                    result.MissingPrerequisites = missingPrereqs;
                }
            }

            return result;
        }
    }
}