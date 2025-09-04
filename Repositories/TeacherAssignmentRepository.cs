

// Repositories/Implementations/TeacherAssignmentRepository.cs
using backend.Data;
using backend.Models;
using backend.Repositories;
using MySql.Data.MySqlClient;
using System.Data;

namespace backend.Repositories
{
    public class TeacherAssignmentRepository : ITeacherAssignmentRepository
    {
        private readonly IDatabaseContext _context;

        public TeacherAssignmentRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public async Task<int> AssignTeacherToCourseAsync(int teacherId, int courseOfferingId, bool isPrimary, int adminId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_teacher_id", teacherId),
                new MySqlParameter("p_course_offering_id", courseOfferingId),
                new MySqlParameter("p_is_primary", isPrimary),
                new MySqlParameter("p_admin_id", adminId),
                new MySqlParameter("p_result_message", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Output }
            };

            await _context.ExecuteNonQueryAsync("sp_AssignTeacherToCourse", parameters);

            var resultMessage = parameters[4].Value.ToString();
            if (resultMessage.StartsWith("Error:"))
                throw new Exception(resultMessage);

            // Get the last inserted ID
            var assignment = (await _context.ExecuteQueryAsync(
                "SELECT id FROM teacher_assignment WHERE teacher_id = @teacherId AND course_offering_id = @courseOfferingId ORDER BY id DESC LIMIT 1",
                new MySqlParameter("@teacherId", teacherId),
                new MySqlParameter("@courseOfferingId", courseOfferingId))).Rows[0];

            return Convert.ToInt32(assignment["id"]);
        }

        public async Task<bool> UpdateTeacherAssignmentAsync(int assignmentId, bool isPrimary, int adminId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_assignment_id", assignmentId),
                new MySqlParameter("p_is_primary", isPrimary),
                new MySqlParameter("p_admin_id", adminId),
                new MySqlParameter("p_result_message", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Output }
            };

            await _context.ExecuteNonQueryAsync("sp_UpdateTeacherAssignment", parameters);

            var resultMessage = parameters[3].Value.ToString();
            return !resultMessage.StartsWith("Error:");
        }

        public async Task<bool> RemoveTeacherAssignmentAsync(int assignmentId, int adminId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_assignment_id", assignmentId),
                new MySqlParameter("p_admin_id", adminId),
                new MySqlParameter("p_result_message", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Output }
            };

            await _context.ExecuteNonQueryAsync("sp_RemoveTeacherAssignment", parameters);

            var resultMessage = parameters[2].Value.ToString();
            return !resultMessage.StartsWith("Error:");
        }

        public async Task<IEnumerable<TeacherAssignmentDetail>> GetAllAssignmentsAsync(int adminId, bool? filterIsActive)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_admin_id", adminId),
                new MySqlParameter("p_filter_is_active", filterIsActive ?? (object)DBNull.Value)
            };

            var result = await _context.ExecuteQueryAsync("sp_GetAllTeacherAssignments", parameters);

            return result.AsEnumerable().Select(row => new TeacherAssignmentDetail
            {
                Id = Convert.ToInt32(row["assignment_id"]),
                TeacherId = Convert.ToInt32(row["teacher_id"]),
                TeacherName = row["teacher_name"].ToString(),
                CourseOfferingId = Convert.ToInt32(row["course_offering_id"]),
                CourseCode = row["course_code"].ToString(),
                CourseTitle = row["course_title"].ToString(),
                SemesterName = row["semester_name"].ToString(),
                SemesterStart = Convert.ToDateTime(row["semester_start"]),
                SemesterEnd = Convert.ToDateTime(row["semester_end"]),
                IsPrimary = Convert.ToBoolean(row["is_primary"]),
                CreatedAt = Convert.ToDateTime(row["created_at"]),
                ModifiedAt = Convert.ToDateTime(row["modified_at"]),
                IsActive = Convert.ToBoolean(row["is_active"])
            }).ToList();
        }

        public async Task<IEnumerable<TeacherAssignmentDetail>> GetTeacherAssignmentsAsync(int userId, bool includeInactive)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_user_id", userId),
                new MySqlParameter("p_include_inactive", includeInactive)
            };

            var result = await _context.ExecuteQueryAsync("sp_GetTeacherAssignments", parameters);

            if (result.Rows.Count > 0 && result.Rows[0]["error_message"] != DBNull.Value)
                throw new Exception(result.Rows[0]["error_message"].ToString());

            return result.AsEnumerable().Select(row => new TeacherAssignmentDetail
            {
                Id = Convert.ToInt32(row["assignment_id"]),
                CourseOfferingId = Convert.ToInt32(row["course_offering_id"]),
                CourseCode = row["course_code"].ToString(),
                CourseTitle = row["course_title"].ToString(),
                SemesterName = row["semester_name"].ToString(),
                SemesterStart = Convert.ToDateTime(row["semester_start"]),
                SemesterEnd = Convert.ToDateTime(row["semester_end"]),
                IsPrimary = Convert.ToBoolean(row["is_primary"]),
                CreatedAt = Convert.ToDateTime(row["created_at"]),
                ModifiedAt = Convert.ToDateTime(row["modified_at"]),
                IsActive = Convert.ToBoolean(row["is_active"])
            }).ToList();
        }

        public async Task<TeacherAssignment> GetByIdAsync(int id)
        {
            var result = await _context.ExecuteQueryAsync(
                "SELECT * FROM teacher_assignment WHERE id = @id AND is_deleted = FALSE",
                new MySqlParameter("@id", id));

            if (result.Rows.Count == 0)
                return null;

            var row = result.Rows[0];
            return new TeacherAssignment
            {
                Id = Convert.ToInt32(row["id"]),
                TeacherId = Convert.ToInt32(row["teacher_id"]),
                CourseOfferingId = Convert.ToInt32(row["course_offering_id"]),
                IsPrimary = Convert.ToBoolean(row["is_primary"]),
                CreatedAt = Convert.ToDateTime(row["created_at"]),
                CreatedBy = row["created_by"] == DBNull.Value ? null : Convert.ToInt32(row["created_by"]),
                ModifiedAt = Convert.ToDateTime(row["modified_at"]),
                ModifiedBy = row["modified_by"] == DBNull.Value ? null : Convert.ToInt32(row["modified_by"]),
                IsActive = Convert.ToBoolean(row["is_active"])
            };
        }
    }
}