using backend.Data;
using backend.Dtos;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text.Json;

namespace backend.Repositories
{
    public class UserManagementRepository : IUserManagementRepository
    {
        private readonly IDatabaseContext _context;

        public UserManagementRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public async Task<PaginationResponseDto<UserResponseDto>> GetUsersAsync(PaginationRequestDto request, int? profileId = null, bool? isActive = null)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_admin_id", 1),
                new MySqlParameter("p_user_id", DBNull.Value),
                new MySqlParameter("p_profile_id", profileId ?? (object)DBNull.Value),
                new MySqlParameter("p_is_active", isActive ?? (object)DBNull.Value),
                new MySqlParameter("p_search_term", request.SearchTerm ?? (object)DBNull.Value)
            };

            var result = await _context.ExecuteQueryAsync("admin_read_users", parameters);

            var users = result.AsEnumerable().Select(row => new UserResponseDto
            {
                Id = row.Field<int>("id"),
                Name = row.Field<string>("name"),
                Email = row.Field<string>("email"),
                ProfileId = row.Field<int>("profile_id"),
                ProfileName = row.Field<string>("profile_name"),
                Contact = row.Field<string?>("contact"),
                CreatedAt = row.Field<DateTime>("created_at"),
                ModifiedAt = row.Field<DateTime>("modified_at"),
                IsActive = row.Field<bool>("is_active")
            }).ToList();

            var pagedData = users
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            return new PaginationResponseDto<UserResponseDto>
            {
                Data = pagedData,
                TotalCount = users.Count,
                CurrentPage = request.Page,
                TotalPages = (int)Math.Ceiling(users.Count / (double)request.PageSize)
            };
        }

        public async Task<UserResponseDto?> GetUserByIdAsync(int userId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_admin_id", 1),
                new MySqlParameter("p_user_id", userId),
                new MySqlParameter("p_profile_id", DBNull.Value),
                new MySqlParameter("p_is_active", DBNull.Value),
                new MySqlParameter("p_search_term", DBNull.Value)
            };

            var result = await _context.ExecuteQueryAsync("admin_read_users", parameters);
            return result.AsEnumerable().Select(row => new UserResponseDto
            {
                Id = row.Field<int>("id"),
                Name = row.Field<string>("name"),
                Email = row.Field<string>("email"),
                ProfileId = row.Field<int>("profile_id"),
                ProfileName = row.Field<string>("profile_name"),
                Contact = row.Field<string?>("contact"),
                CreatedAt = row.Field<DateTime>("created_at"),
                ModifiedAt = row.Field<DateTime>("modified_at"),
                IsActive = row.Field<bool>("is_active")
            }).FirstOrDefault();
        }

        public async Task<int> UpdateUserAsync(int adminId, int userId, UserUpdateDto updateDto)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_admin_id", adminId),
                new MySqlParameter("p_user_id", userId),
                new MySqlParameter("p_name", updateDto.Name ?? (object)DBNull.Value),
                new MySqlParameter("p_email", updateDto.Email ?? (object)DBNull.Value),
                new MySqlParameter("p_contact", updateDto.Contact ?? (object)DBNull.Value),
                new MySqlParameter("p_profile_id", updateDto.ProfileId ?? (object)DBNull.Value),
                new MySqlParameter("p_is_active", updateDto.IsActive ?? (object)DBNull.Value)
            };

            return await _context.ExecuteNonQueryAsync("admin_update_user", parameters);
        }

        public async Task<int> DeleteUserAsync(int adminId, int userId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_admin_id", adminId),
                new MySqlParameter("p_user_id", userId)
            };

            return await _context.ExecuteNonQueryAsync("admin_delete_user", parameters);
        }

        public async Task<PaginationResponseDto<StudentProfileResponseDto>> GetStudentProfilesAsync(PaginationRequestDto request, string? academicStatus = null)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_admin_id", 1),
                new MySqlParameter("p_student_id", DBNull.Value),
                new MySqlParameter("p_user_id", DBNull.Value),
                new MySqlParameter("p_reg_no", DBNull.Value),
                new MySqlParameter("p_academic_status", academicStatus ?? (object)DBNull.Value)
            };

            var result = await _context.ExecuteQueryAsync("admin_read_student_profiles", parameters);

            var students = result.AsEnumerable().Select(row => new StudentProfileResponseDto
            {
                Id = row.Field<int>("id"),
                UserId = row.Field<int>("user_id"),
                UserName = row.Field<string>("name"),
                RegNo = row.Field<string>("reg_no"),
                EnrollYear = row.Field<int>("enroll_year"),
                CurrentSemester = row.Field<int?>("current_semester"),
                DepId = row.Field<int>("dep_id"),
                DepartmentName = row.Field<string>("department_name"),
                ProgramId = row.Field<int>("program_id"),
                ProgramName = row.Field<string>("program_name"),
                LevelId = row.Field<int>("level_id"),
                LevelName = row.Field<string>("level_name"),
                AcademicStatus = row.Field<string>("academic_status"),
                Cgpa = row.Field<decimal>("cgpa"),
                CurrentCreditHours = row.Field<decimal>("current_credit_hours"),
                CompletedCreditHours = row.Field<decimal>("completed_credit_hours"),
                CreatedAt = row.Field<DateTime>("created_at"),
                ModifiedAt = row.Field<DateTime>("modified_at")
            }).ToList();

            var pagedData = students
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            return new PaginationResponseDto<StudentProfileResponseDto>
            {
                Data = pagedData,
                TotalCount = students.Count,
                CurrentPage = request.Page,
                TotalPages = (int)Math.Ceiling(students.Count / (double)request.PageSize)
            };
        }

        public async Task<StudentProfileResponseDto?> GetStudentProfileByIdAsync(int studentId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_admin_id", 1),
                new MySqlParameter("p_student_id", studentId),
                new MySqlParameter("p_user_id", DBNull.Value),
                new MySqlParameter("p_reg_no", DBNull.Value),
                new MySqlParameter("p_academic_status", DBNull.Value)
            };

            var result = await _context.ExecuteQueryAsync("admin_read_student_profiles", parameters);
            return result.AsEnumerable().Select(row => new StudentProfileResponseDto
            {
                Id = row.Field<int>("id"),
                UserId = row.Field<int>("user_id"),
                UserName = row.Field<string>("name"),
                RegNo = row.Field<string>("reg_no"),
                EnrollYear = row.Field<int>("enroll_year"),
                CurrentSemester = row.Field<int?>("current_semester"),
                DepId = row.Field<int>("dep_id"),
                DepartmentName = row.Field<string>("department_name"),
                ProgramId = row.Field<int>("program_id"),
                ProgramName = row.Field<string>("program_name"),
                LevelId = row.Field<int>("level_id"),
                LevelName = row.Field<string>("level_name"),
                AcademicStatus = row.Field<string>("academic_status"),
                Cgpa = row.Field<decimal>("cgpa"),
                CurrentCreditHours = row.Field<decimal>("current_credit_hours"),
                CompletedCreditHours = row.Field<decimal>("completed_credit_hours"),
                CreatedAt = row.Field<DateTime>("created_at"),
                ModifiedAt = row.Field<DateTime>("modified_at")
            }).FirstOrDefault();
        }

        public async Task<int> UpdateStudentProfileAsync(int adminId, int studentId, StudentProfileUpdateDto updateDto)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_admin_id", adminId),
                new MySqlParameter("p_student_id", studentId),
                new MySqlParameter("p_reg_no", updateDto.RegNo ?? (object)DBNull.Value),
                new MySqlParameter("p_current_semester", updateDto.CurrentSemester ?? (object)DBNull.Value),
                new MySqlParameter("p_academic_status", updateDto.AcademicStatus ?? (object)DBNull.Value),
                new MySqlParameter("p_cgpa", updateDto.Cgpa ?? (object)DBNull.Value),
                new MySqlParameter("p_current_credit_hours", updateDto.CurrentCreditHours ?? (object)DBNull.Value),
                new MySqlParameter("p_completed_credit_hours", updateDto.CompletedCreditHours ?? (object)DBNull.Value),
                new MySqlParameter("p_attempted_credit_hours", updateDto.AttemptedCreditHours ?? (object)DBNull.Value)
            };

            return await _context.ExecuteNonQueryAsync("admin_update_student_profile", parameters);
        }

        public async Task<PaginationResponseDto<TeacherProfileResponseDto>> GetTeacherProfilesAsync(PaginationRequestDto request, int? depId = null)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_admin_id", 1),
                new MySqlParameter("p_teacher_id", DBNull.Value),
                new MySqlParameter("p_user_id", DBNull.Value),
                new MySqlParameter("p_dep_id", depId ?? (object)DBNull.Value)
            };

            var result = await _context.ExecuteQueryAsync("admin_read_teacher_profiles", parameters);

            var teachers = result.AsEnumerable().Select(row => new TeacherProfileResponseDto
            {
                Id = row.Field<int>("id"),
                UserId = row.Field<int>("user_id"),
                UserName = row.Field<string>("name"),
                DepId = row.Field<int>("dep_id"),
                DepartmentName = row.Field<string>("department_name"),
                Designation = row.Field<string>("designation"),
                HireDate = row.Field<DateTime>("hire_date"),
                Qualification = row.Field<string?>("qualification"),
                Specialization = row.Field<string?>("specialization"),
                ExperienceYears = row.Field<int>("experience_years"),
                CreatedAt = row.Field<DateTime>("created_at"),
                ModifiedAt = row.Field<DateTime>("modified_at")
            }).ToList();

            var pagedData = teachers
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            return new PaginationResponseDto<TeacherProfileResponseDto>
            {
                Data = pagedData,
                TotalCount = teachers.Count,
                CurrentPage = request.Page,
                TotalPages = (int)Math.Ceiling(teachers.Count / (double)request.PageSize)
            };
        }

        public async Task<TeacherProfileResponseDto?> GetTeacherProfileByIdAsync(int teacherId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_admin_id", 1),
                new MySqlParameter("p_teacher_id", teacherId),
                new MySqlParameter("p_user_id", DBNull.Value),
                new MySqlParameter("p_dep_id", DBNull.Value)
            };

            var result = await _context.ExecuteQueryAsync("admin_read_teacher_profiles", parameters);
            return result.AsEnumerable().Select(row => new TeacherProfileResponseDto
            {
                Id = row.Field<int>("id"),
                UserId = row.Field<int>("user_id"),
                UserName = row.Field<string>("name"),
                DepId = row.Field<int>("dep_id"),
                DepartmentName = row.Field<string>("department_name"),
                Designation = row.Field<string>("designation"),
                HireDate = row.Field<DateTime>("hire_date"),
                Qualification = row.Field<string?>("qualification"),
                Specialization = row.Field<string?>("specialization"),
                ExperienceYears = row.Field<int>("experience_years"),
                CreatedAt = row.Field<DateTime>("created_at"),
                ModifiedAt = row.Field<DateTime>("modified_at")
            }).FirstOrDefault();
        }

        public async Task<int> UpdateTeacherProfileAsync(int adminId, int teacherId, TeacherProfileUpdateDto updateDto)
        {
            var hireDateParam = updateDto.HireDate.HasValue
    ? (object)updateDto.HireDate.Value
    : DBNull.Value;

            var parameters = new[]
            {
        new MySqlParameter("p_admin_id", adminId),
        new MySqlParameter("p_teacher_id", teacherId),
        new MySqlParameter("p_dep_id", updateDto.DepId ?? (object)DBNull.Value),
        new MySqlParameter("p_designation", updateDto.Designation ?? (object)DBNull.Value),
        new MySqlParameter("p_hire_date", hireDateParam),
        new MySqlParameter("p_qualification", updateDto.Qualification ?? (object)DBNull.Value),
        new MySqlParameter("p_specialization", updateDto.Specialization ?? (object)DBNull.Value),
        new MySqlParameter("p_experience_years", updateDto.ExperienceYears ?? (object)DBNull.Value)
    };

            return await _context.ExecuteNonQueryAsync("admin_update_teacher_profile", parameters);
        }

        public async Task<PaginationResponseDto<AdminProfileResponseDto>> GetAdminProfilesAsync(PaginationRequestDto request, string? level = null)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_admin_id", 1),
                new MySqlParameter("p_target_admin_id", DBNull.Value),
                new MySqlParameter("p_user_id", DBNull.Value),
                new MySqlParameter("p_level", level ?? (object)DBNull.Value)
            };

            var result = await _context.ExecuteQueryAsync("admin_read_admin_profiles", parameters);

            var admins = result.AsEnumerable().Select(row => new AdminProfileResponseDto
            {
                Id = row.Field<int>("id"),
                UserId = row.Field<int>("user_id"),
                UserName = row.Field<string>("name"),
                Level = row.Field<string>("level"),
                AccessLevel = string.IsNullOrEmpty(row.Field<string?>("access_level")) ?
                    null : JsonDocument.Parse(row.Field<string>("access_level")),
                HireDate = row.Field<DateTime>("hire_date"),
                DepartmentId = row.Field<int?>("department_id"),
                DepartmentName = row.Field<string?>("department_name"),
                CreatedAt = row.Field<DateTime>("created_at"),
                ModifiedAt = row.Field<DateTime>("modified_at")
            }).ToList();

            var pagedData = admins
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            return new PaginationResponseDto<AdminProfileResponseDto>
            {
                Data = pagedData,
                TotalCount = admins.Count,
                CurrentPage = request.Page,
                TotalPages = (int)Math.Ceiling(admins.Count / (double)request.PageSize)
            };
        }

        public async Task<AdminProfileResponseDto?> GetAdminProfileByIdAsync(int adminProfileId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_admin_id", 1),
                new MySqlParameter("p_target_admin_id", adminProfileId),
                new MySqlParameter("p_user_id", DBNull.Value),
                new MySqlParameter("p_level", DBNull.Value)
            };

            var result = await _context.ExecuteQueryAsync("admin_read_admin_profiles", parameters);
            return result.AsEnumerable().Select(row => new AdminProfileResponseDto
            {
                Id = row.Field<int>("id"),
                UserId = row.Field<int>("user_id"),
                UserName = row.Field<string>("name"),
                Level = row.Field<string>("level"),
                AccessLevel = string.IsNullOrEmpty(row.Field<string?>("access_level")) ?
                    null : JsonDocument.Parse(row.Field<string>("access_level")),
                HireDate = row.Field<DateTime>("hire_date"),
                DepartmentId = row.Field<int?>("department_id"),
                DepartmentName = row.Field<string?>("department_name"),
                CreatedAt = row.Field<DateTime>("created_at"),
                ModifiedAt = row.Field<DateTime>("modified_at")
            }).FirstOrDefault();
        }

        public async Task<int> UpdateAdminProfileAsync(int adminId, int targetAdminId, AdminProfileUpdateDto updateDto)
        {
            // Handle access level conversion
            object accessLevelParam = DBNull.Value;
            if (updateDto.AccessLevel != null)
            {
                accessLevelParam = updateDto.AccessLevel.RootElement.GetRawText();
            }

            // Handle hire date conversion - use DateTime directly, not DateOnly
            object hireDateParam = DBNull.Value;
            if (updateDto.HireDate.HasValue)
            {
                hireDateParam = updateDto.HireDate.Value;
            }

            var parameters = new[]
            {
        new MySqlParameter("p_admin_id", adminId),
        new MySqlParameter("p_target_admin_id", targetAdminId),
        new MySqlParameter("p_level", updateDto.Level ?? (object)DBNull.Value),
        new MySqlParameter("p_access_level", accessLevelParam),
        new MySqlParameter("p_hire_date", hireDateParam),
        new MySqlParameter("p_department_id", updateDto.DepartmentId ?? (object)DBNull.Value)
    };

            return await _context.ExecuteNonQueryAsync("admin_update_admin_profile", parameters);
        }
    }
}