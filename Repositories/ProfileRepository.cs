// Repositories/ProfileRepository.cs
using backend.Data;
using backend.Dtos;
using MySql.Data.MySqlClient;

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

            var affectedRows = await _context.ExecuteNonQueryAsync("sp_AdminUpdateProfile", parameters);
            return affectedRows > 0;
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

            var affectedRows = await _context.ExecuteNonQueryAsync("sp_TeacherUpdateProfile", parameters);
            return affectedRows > 0;
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

            var affectedRows = await _context.ExecuteNonQueryAsync("sp_TeacherUpdateQualifications", parameters);
            return affectedRows > 0;
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

            var affectedRows = await _context.ExecuteNonQueryAsync("sp_StudentUpdateProfile", parameters);
            return affectedRows > 0;
        }
    }
}