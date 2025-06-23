using backend.Data;
using backend.Dtos;
using backend.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace backend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDatabaseContext _context;

        public UserRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public async Task<DataTable> RegisterStudent(StudentRegistrationDto dto, int createdBy)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_name", dto.Name),
                new MySqlParameter("p_password_hash", dto.Password),
                new MySqlParameter("p_email", dto.Email),
                new MySqlParameter("p_reg_no", dto.RegNo),
                new MySqlParameter("p_enroll_year", dto.EnrollYear),
                new MySqlParameter("p_dep_id", dto.DepartmentId),
                new MySqlParameter("p_program_id", dto.ProgramId),
                new MySqlParameter("p_level_id", dto.LevelId),
                new MySqlParameter("p_created_by", createdBy)
            };

            return await _context.ExecuteQueryAsync("sp_RegisterStudent", parameters);
        }

        public async Task<DataTable> RegisterTeacher(TeacherRegistrationDto dto, int createdBy)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_name", dto.Name),
                new MySqlParameter("p_password_hash", dto.Password),
                new MySqlParameter("p_email", dto.Email),
                new MySqlParameter("p_dep_id", dto.DepartmentId),
                new MySqlParameter("p_designation", dto.Designation),
                new MySqlParameter("p_hire_date", dto.HireDate),
                new MySqlParameter("p_qualification", dto.Qualification ?? string.Empty),
                new MySqlParameter("p_specialization", dto.Specialization ?? string.Empty),
                new MySqlParameter("p_created_by", createdBy)
            };

            return await _context.ExecuteQueryAsync("sp_RegisterTeacher", parameters);
        }

        public async Task<DataTable> RegisterAdmin(AdminRegistrationDto dto, int createdBy)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_name", dto.Name),
                new MySqlParameter("p_password_hash", dto.Password),
                new MySqlParameter("p_email", dto.Email),
                new MySqlParameter("p_level", dto.Level),
                new MySqlParameter("p_hire_date", dto.HireDate),
                new MySqlParameter("p_department_id", dto.DepartmentId ?? (object)DBNull.Value),
                new MySqlParameter("p_created_by", createdBy)
            };

            return await _context.ExecuteQueryAsync("sp_RegisterAdmin", parameters);
        }

        public async Task<DataTable> GetUserProfile(int userId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_user_id", userId)
            };

            return await _context.ExecuteQueryAsync("sp_ViewUserProfile", parameters);
        }



        public async Task<StudentDetailsResponse> AdminGetStudentByRegNo(int adminId, string regNo)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_admin_id", adminId),
                new MySqlParameter("p_reg_no", regNo)
            };

            var result = await _context.ExecuteQueryAsync("sp_AdminGetStudentByRegNo", parameters);

            if (result.Rows.Count == 0)
                throw new Exception("Student not found");

            var row = result.Rows[0];
            return new StudentDetailsResponse
            {
                Id = Convert.ToInt32(row["id"]),
                Name = row["name"].ToString(),
                Email = row["email"].ToString(),
                Contact = row["contact"].ToString(),
                ProfilePicUrl = row["profile_pic_url"]?.ToString(),
                RegNo = row["reg_no"].ToString(),
                EnrollYear = Convert.ToInt32(row["enroll_year"]),
                CurrentSemester = Convert.ToInt32(row["current_semester"]),
                DepartmentName = row["department_name"].ToString(),
                ProgramName = row["program_name"].ToString(),
                LevelName = row["level_name"].ToString(),
                AcademicStatus = row["academic_status"].ToString(),
                Cgpa = Convert.ToDecimal(row["cgpa"]),
                TotalSemestersStudied = Convert.ToInt32(row["total_semesters_studied"]),
                IsActive = Convert.ToBoolean(row["is_active"]),
                CreatedAt = Convert.ToDateTime(row["created_at"])
            };
        }

        public async Task<List<TeacherDetailsResponse>> AdminGetTeacherByDetails(int adminId, string name, int depId, string designation)
        {
            var parameters = new[]
            {
        new MySqlParameter("p_admin_id", adminId),
        new MySqlParameter("p_name", name),
        new MySqlParameter("p_dep_id", depId),
        new MySqlParameter("p_designation", designation)
    };

            var dataTable = await _context.ExecuteQueryAsync("sp_AdminGetTeacherByDetails", parameters);

            var teachers = new List<TeacherDetailsResponse>();
            foreach (DataRow row in dataTable.Rows)
            {
                teachers.Add(new TeacherDetailsResponse
                {
                    Id = Convert.ToInt32(row["id"]),
                    Name = row["name"].ToString(),
                    Email = row["email"].ToString(),
                    Contact = row["contact"].ToString(),
                    ProfilePicUrl = row["profile_pic_url"]?.ToString(),
                    Designation = row["designation"].ToString(),
                    HireDate = Convert.ToDateTime(row["hire_date"]),
                    Qualification = row["qualification"].ToString(),
                    Specialization = row["specialization"].ToString(),
                    ExperienceYears = Convert.ToInt32(row["experience_years"]),
                    DepartmentName = row["department_name"].ToString(),
                    DepartmentCode = row["department_code"].ToString(),
                    IsActive = Convert.ToBoolean(row["is_active"]),
                    CreatedAt = Convert.ToDateTime(row["created_at"])
                });
            }

            return teachers;
        }

        public async Task<int> AdminUpdateStudent(AdminUpdateStudentModel model)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_admin_id", model.AdminId),
                new MySqlParameter("p_user_id", model.UserId),
                new MySqlParameter("p_name", model.Name ?? (object)DBNull.Value),
                new MySqlParameter("p_email", model.Email ?? (object)DBNull.Value),
                new MySqlParameter("p_reg_no", model.RegNo ?? (object)DBNull.Value),
                new MySqlParameter("p_contact", model.Contact ?? (object)DBNull.Value),
                new MySqlParameter("p_enroll_year", model.EnrollYear ?? (object)DBNull.Value),
                new MySqlParameter("p_current_semester", model.CurrentSemester ?? (object)DBNull.Value),
                new MySqlParameter("p_academic_status", model.AcademicStatus ?? (object)DBNull.Value),
                new MySqlParameter("p_cgpa", model.Cgpa ?? (object)DBNull.Value)
                //new MySqlParameter("p_profile_pic_url", model.ProfilePicUrl ?? (object)DBNull.Value)
            };

            var result = await _context.ExecuteQueryAsync("sp_AdminUpdateStudent", parameters);

            if (result.Rows.Count > 0 && result.Columns.Contains("affected_rows"))
            {
                return Convert.ToInt32(result.Rows[0]["affected_rows"]);
            }
            return 0;
        }

        public async Task<int> AdminUpdateTeacher(AdminUpdateTeacherModel model)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_admin_id", model.AdminId),
                new MySqlParameter("p_user_id", model.UserId),
                new MySqlParameter("p_name", model.Name ?? (object)DBNull.Value),
                new MySqlParameter("p_email", model.Email ?? (object)DBNull.Value),
                new MySqlParameter("p_contact", model.Contact ?? (object)DBNull.Value),
                new MySqlParameter("p_designation", model.Designation ?? (object)DBNull.Value),
                new MySqlParameter("p_qualification", model.Qualification ?? (object)DBNull.Value),
                new MySqlParameter("p_specialization", model.Specialization ?? (object)DBNull.Value),
                new MySqlParameter("p_experience_years", model.ExperienceYears ?? (object)DBNull.Value),
                new MySqlParameter("p_profile_pic_url", model.ProfilePicUrl ?? (object)DBNull.Value)
            };
            // Use ExecuteScalarAsync since we're returning a single value
            var result = await _context.ExecuteScalarAsync<int>("sp_AdminUpdateTeacher", parameters);
            return result;
        }

        public async Task<(bool Success, string Message)> AdminSoftDeleteUser(int adminId, int userId)
        {
            var parameters = new[]
            {
        new MySqlParameter("p_admin_id", adminId),
        new MySqlParameter("p_user_id", userId),
        new MySqlParameter("p_message", MySqlDbType.VarChar) {
            Direction = ParameterDirection.Output,
            Size = 255
        }
    };

            var affectedRows = await _context.ExecuteNonQueryAsync("sp_AdminSoftDeleteUser", parameters);
            var message = parameters[2].Value?.ToString() ?? "User deleted successfully";

            return (affectedRows > 0, message);
        }

        public async Task<int> AdminCountActiveUsers(int adminId, string profileName)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_admin_id", adminId),
                new MySqlParameter("p_profile_name", profileName)
            };

            var result = await _context.ExecuteQueryAsync("sp_AdminCountActiveUsers", parameters);
            return Convert.ToInt32(result.Rows[0]["active_count"]);
        }

        public async Task<List<UserResponse>> AdminGetAllUsersByProfile(int adminId, string profileName, int limit, int offset)
        {
            var parameters = new[]
            {
        new MySqlParameter("p_admin_id", adminId),
        new MySqlParameter("p_profile_name", profileName),
        new MySqlParameter("p_limit", limit),
        new MySqlParameter("p_offset", offset)
    };

            var dataTable = await _context.ExecuteQueryAsync("sp_GetAllUsersByProfile", parameters);

            var users = new List<UserResponse>();
            foreach (DataRow row in dataTable.Rows)
            {
                users.Add(new UserResponse
                {
                    Id = Convert.ToInt32(row["id"]),
                    Name = row["name"].ToString(),
                    Email = row["email"].ToString(),
                    Profile = row["profile_name"].ToString(),
                    AdditionalInfo = row["additional_info"]?.ToString() ?? string.Empty
                });
            }

            return users;
        }





    }
}