using backend.Data;
using backend.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace backend.Repositories
{
    public class RegistrationRepository : IRegistrationRepository
    {
        private readonly IDatabaseContext _context;

        public RegistrationRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public async Task<StudentProfile> RegisterStudent(User user, StudentProfile profile, int createdBy)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_name", user.Name),
                new MySqlParameter("p_password_hash", user.PasswordHash),
                new MySqlParameter("p_email", user.Email),
                 new MySqlParameter("p_contact", user.Contact ?? (object)DBNull.Value), // Add this line
                new MySqlParameter("p_reg_no", profile.RegistrationNo),
                new MySqlParameter("p_enroll_year", profile.EnrollYear),
                new MySqlParameter("p_dep_id", profile.DepartmentId),
                new MySqlParameter("p_program_id", profile.ProgramId),
                new MySqlParameter("p_level_id", profile.LevelId),
                new MySqlParameter("p_created_by", createdBy)
            };

            var result = await _context.ExecuteQueryAsync("sp_RegisterStudent", parameters);
            if (result.Rows.Count == 0) return null;

            return MapToStudentProfile(result.Rows[0]);
        }

        public async Task<TeacherProfile> RegisterTeacher(User user, TeacherProfile profile, int createdBy)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_name", user.Name),
                new MySqlParameter("p_password_hash", user.PasswordHash),
                new MySqlParameter("p_email", user.Email),
                new MySqlParameter("p_contact", user.Contact ?? (object)DBNull.Value), // Add this line
                new MySqlParameter("p_dep_id", profile.DepartmentId),
                new MySqlParameter("p_designation", profile.Designation),
                new MySqlParameter("p_hire_date", profile.HireDate),
                new MySqlParameter("p_qualification", profile.Qualification),
                new MySqlParameter("p_specialization", profile.Specialization),
                new MySqlParameter("p_experience_years", profile.ExperienceYears),
                new MySqlParameter("p_created_by", createdBy)
            };

            var result = await _context.ExecuteQueryAsync("sp_RegisterTeacher", parameters);
            if (result.Rows.Count == 0) return null;

            return MapToTeacherProfile(result.Rows[0]);
        }

        public async Task<AdminProfile> RegisterAdmin(User user, AdminProfile profile, int createdBy)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_name", user.Name),
                new MySqlParameter("p_password_hash", user.PasswordHash),
                new MySqlParameter("p_email", user.Email),
                 new MySqlParameter("p_contact", user.Contact ?? (object)DBNull.Value), // Add this line
                new MySqlParameter("p_level", profile.Level),
                new MySqlParameter("p_hire_date", profile.HireDate),
                new MySqlParameter("p_department_id", profile.DepartmentId ?? (object)DBNull.Value),
                new MySqlParameter("p_created_by", createdBy)
            };

            var result = await _context.ExecuteQueryAsync("sp_RegisterAdmin", parameters);
            if (result.Rows.Count == 0) return null;

            return MapToAdminProfile(result.Rows[0]);
        }

        private StudentProfile MapToStudentProfile(DataRow row)
        {
            return new StudentProfile
            {
                UserId = Convert.ToInt32(row["id"]),
                RegistrationNo = row["reg_no"].ToString(),
                DepartmentName = row["department_name"].ToString(),
                ProgramName = row["program_name"].ToString(),
                LevelName = row["level_name"].ToString(),
            };
        }

        private TeacherProfile MapToTeacherProfile(DataRow row)
        {
            return new TeacherProfile
            {
                UserId = Convert.ToInt32(row["id"]),
                Designation = row["designation"].ToString() ?? string.Empty,
                DepartmentId = Convert.ToInt32(row["dep_id"]),
                DepartmentName = row["department_name"].ToString() ?? string.Empty,
                ExperienceYears = Convert.ToInt32(row["experience_years"]),
            };
        }

        private AdminProfile MapToAdminProfile(DataRow row)
        {
            return new AdminProfile
            {
                UserId = Convert.ToInt32(row["id"]),
                Level = row["level"].ToString(),
                DepartmentName = row["department_name"] != DBNull.Value ? row["department_name"].ToString() : null
            };
        }
    }
}