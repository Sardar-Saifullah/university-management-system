// Repositories/ProfileRepository.cs
using backend.Data;
using backend.Dtos;
using MySql.Data.MySqlClient;
using System.Data;

namespace backend.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly IDatabaseContext _context;

        public ProfileRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public async Task<AdminProfileDto> GetAdminProfile(int adminId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_admin_id", adminId)
            };

            var result = await _context.ExecuteQueryAsync("sp_AdminViewProfile", parameters);

            if (result.Rows.Count == 0)
                return null;

            var row = result.Rows[0];
            return new AdminProfileDto
            {
                Id = Convert.ToInt32(row["id"]),
                Name = row["name"].ToString(),
                Email = row["email"].ToString(),
                Contact = row["contact"]?.ToString(),
                ProfilePicUrl = row["profile_pic_url"]?.ToString(),
                ProfilePicType = row["profile_pic_type"]?.ToString(),
                Level = row["level"].ToString(),
                HireDate = Convert.ToDateTime(row["hire_date"]),
                DepartmentName = row["department_name"]?.ToString(),
                CreatedAt = Convert.ToDateTime(row["created_at"]),
                ModifiedAt = Convert.ToDateTime(row["modified_at"])
            };
        }

        public async Task<bool> UpdateAdminProfile(int adminId, AdminOwnProfileUpdateDto updateDto)
        {
            var parameters = new[]
            {
        new MySqlParameter("p_admin_id", adminId),
        new MySqlParameter("p_name", updateDto.Name ?? (object)DBNull.Value),
        new MySqlParameter("p_email", updateDto.Email ?? (object)DBNull.Value),
        new MySqlParameter("p_contact", updateDto.Contact ?? (object)DBNull.Value)
    };

            try
            {
                // Use ExecuteScalarAsync to get the success indicator from the SELECT statement
                var result = await _context.ExecuteScalarAsync<int>("sp_AdminUpdateProfile", parameters);
                return result == 1;
            }
            catch (Exception ex)
            {
                // Log the error for debugging
                Console.WriteLine($"Error updating admin profile: {ex.Message}");
                return false;
            }
        }


        public async Task<bool> IsUserAdmin(int userId)
        {
            var parameters = new[]
            {
        new MySqlParameter("p_user_id", userId),
        new MySqlParameter("p_is_admin", MySqlDbType.Bit) { Direction = ParameterDirection.Output }
    };

            await _context.ExecuteNonQueryAsync("sp_is_user_admin", parameters);
            return Convert.ToBoolean(parameters[1].Value);
        }
        public async Task<TeacherProfileDto> GetTeacherProfile(int teacherId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_teacher_id", teacherId)
            };

            var result = await _context.ExecuteQueryAsync("sp_TeacherViewProfile", parameters);

            if (result.Rows.Count == 0)
                return null;

            var row = result.Rows[0];
            return new TeacherProfileDto
            {
                Id = Convert.ToInt32(row["id"]),
                Name = row["name"].ToString(),
                Email = row["email"].ToString(),
                Contact = row["contact"]?.ToString(),
                ProfilePicUrl = row["profile_pic_url"]?.ToString(),
                ProfilePicType = row["profile_pic_type"]?.ToString(),
                Designation = row["designation"].ToString(),
                HireDate = Convert.ToDateTime(row["hire_date"]),
                Qualification = row["qualification"]?.ToString(),
                Specialization = row["specialization"]?.ToString(),
                ExperienceYears = Convert.ToInt32(row["experience_years"]),
                DepartmentName = row["department_name"].ToString(),
                DepartmentCode = row["department_code"].ToString(),
                CreatedAt = Convert.ToDateTime(row["created_at"]),
                ModifiedAt = Convert.ToDateTime(row["modified_at"])
            };
        }

        public async Task<bool> UpdateTeacherProfile(int teacherId, TeacherOwnProfileUpdateDto updateDto)
        {
            var parameters = new[]
            {
        new MySqlParameter("p_teacher_id", teacherId),
        new MySqlParameter("p_name", updateDto.Name ?? (object)DBNull.Value),
        new MySqlParameter("p_email", updateDto.Email ?? (object)DBNull.Value),
        new MySqlParameter("p_contact", updateDto.Contact ?? (object)DBNull.Value)
    };

            try
            {
                var result = await _context.ExecuteScalarAsync<int>("sp_TeacherUpdateProfile", parameters);
                return result == 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating teacher profile: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateTeacherQualifications(int teacherId, TeacherQualificationUpdateDto updateDto)
        {
            var parameters = new[]
            {
        new MySqlParameter("p_teacher_id", teacherId),
        new MySqlParameter("p_qualification", updateDto.Qualification ?? (object)DBNull.Value),
        new MySqlParameter("p_specialization", updateDto.Specialization ?? (object)DBNull.Value),
        new MySqlParameter("p_experience_years", updateDto.ExperienceYears)
    };

            try
            {
                var result = await _context.ExecuteScalarAsync<int>("sp_TeacherUpdateQualifications", parameters);
                return result == 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating teacher qualifications: {ex.Message}");
                return false;
            }
        }

        public async Task<StudentProfileDto> GetStudentProfile(int studentId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_student_id", studentId)
            };

            var result = await _context.ExecuteQueryAsync("sp_StudentViewProfile", parameters);

            if (result.Rows.Count == 0)
                return null;

            var row = result.Rows[0];
            return new StudentProfileDto
            {
                Id = Convert.ToInt32(row["id"]),
                Name = row["name"].ToString(),
                Email = row["email"].ToString(),
                Contact = row["contact"]?.ToString(),
                ProfilePicUrl = row["profile_pic_url"]?.ToString(),
                ProfilePicType = row["profile_pic_type"]?.ToString(),
                RegNo = row["reg_no"].ToString(),
                EnrollYear = Convert.ToInt32(row["enroll_year"]),
                CurrentSemester = row["current_semester"] != DBNull.Value ? Convert.ToInt32(row["current_semester"]) : null,
                DepartmentName = row["department_name"].ToString(),
                ProgramName = row["program_name"].ToString(),
                AcademicStatus = row["academic_status"].ToString(),
                Cgpa = Convert.ToDecimal(row["cgpa"]),
                CurrentCreditHours = Convert.ToDecimal(row["current_credit_hours"]),
                CompletedCreditHours = Convert.ToDecimal(row["completed_credit_hours"]),
                AttemptedCreditHours = Convert.ToDecimal(row["attempted_credit_hours"]),
                LevelName = row["level_name"].ToString(),
                CurrentSemesterName = row["current_semester_name"]?.ToString(),
                CreatedAt = Convert.ToDateTime(row["created_at"]),
                ModifiedAt = Convert.ToDateTime(row["modified_at"])
            };
        }

        public async Task<bool> UpdateStudentProfile(int studentId, StudentOwnProfileUpdateDto updateDto)
        {
            var parameters = new[]
            {
        new MySqlParameter("p_student_id", studentId),
        new MySqlParameter("p_name", updateDto.Name ?? (object)DBNull.Value),
        new MySqlParameter("p_email", updateDto.Email ?? (object)DBNull.Value),
        new MySqlParameter("p_contact", updateDto.Contact ?? (object)DBNull.Value)
    };

            try
            {
                var result = await _context.ExecuteScalarAsync<int>("sp_StudentUpdateProfile", parameters);
                return result == 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating student profile: {ex.Message}");
                return false;
            }
        }

        public async Task<int?> GetStudentIdByUserId(int userId)
        {
            // Direct query is fine here; this is a simple lookup
            var sql = "SELECT id FROM student_profile WHERE user_id = @userId AND is_deleted = FALSE LIMIT 1";
            var parameters = new[] { new MySqlParameter("@userId", userId) };
            var table = await _context.ExecuteQueryAsync(sql, parameters);
            if (table.Rows.Count == 0) return null;
            return Convert.ToInt32(table.Rows[0]["id"]);
        }

        public async Task<int?> GetTeacherIdByUserId(int userId)
        {
            var sql = "SELECT id FROM teacher_profile WHERE user_id = @userId AND is_deleted = FALSE LIMIT 1";
            var parameters = new[] { new MySqlParameter("@userId", userId) };
            var table = await _context.ExecuteQueryAsync(sql, parameters);
            if (table.Rows.Count == 0) return null;
            return Convert.ToInt32(table.Rows[0]["id"]);
        }
    }
}