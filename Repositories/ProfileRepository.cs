using backend.Data;
using backend.Dtos;
using backend.Models;
using backend.Utilities;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using System.Data;
using Microsoft.AspNetCore.Http;

namespace backend.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly IDatabaseContext _context;
        private readonly IImageProcessor _imageProcessor;

        public ProfileRepository(IDatabaseContext context, IImageProcessor imageProcessor)
        {
            _context = context;
            _imageProcessor = imageProcessor;
        }

        public async Task<AdminProfileResponse> GetAdminProfileAsync(int adminId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_admin_id", adminId)
            };

            var result = await _context.ExecuteQueryAsync("sp_AdminViewProfile", parameters);

            if (result.Rows.Count == 0)
                throw new KeyNotFoundException("Admin profile not found");

            var row = result.Rows[0];
            return new AdminProfileResponse
            {
                Id = Convert.ToInt32(row["id"]),
                Name = row["name"].ToString(),
                Email = row["email"].ToString(),
                Contact = row["contact"]?.ToString(),
                ProfilePicUrl = row["profile_pic_url"]?.ToString(),
                ProfilePicType = row["profile_picture_type"]?.ToString(),
                Level = row["level"].ToString(),
                HireDate = Convert.ToDateTime(row["hire_date"]),
                DepartmentName = row["department_name"]?.ToString()
            };
        }

        public async Task<bool> UpdateAdminProfileAsync(int adminId, AdminProfileUpdateDto dto, IFormFile? profilePicFile = null)
        {
            string? profilePicUrl = null;
            string? profilePicType = null;

            if (profilePicFile != null)
            {
                var uploadResult = await _imageProcessor.ProcessAndSaveProfileImage(profilePicFile, adminId, "admin");
                profilePicUrl = uploadResult.FilePath;
                profilePicType = uploadResult.ContentType.Split('/')[1];
            }

            // Simplified contact parameter handling to match stored procedure
            object contactParam = string.IsNullOrWhiteSpace(dto.Contact)
                ? (object)DBNull.Value  // Send NULL to SQL for empty/whitespace
                : dto.Contact.Trim();   // Send trimmed value otherwise

            var parameters = new[]
            {
        new MySqlParameter("p_admin_id", adminId),
        new MySqlParameter("p_name", !string.IsNullOrWhiteSpace(dto.Name) ? dto.Name.Trim() : (object)DBNull.Value),
        new MySqlParameter("p_contact", contactParam),
        new MySqlParameter("p_profile_pic_url", profilePicUrl ?? (object)DBNull.Value),
        new MySqlParameter("p_profile_pic_type", profilePicType ?? (object)DBNull.Value)
    };

            var result = await _context.ExecuteQueryAsync("sp_AdminUpdateProfile", parameters);
            return result.Rows.Count > 0 && Convert.ToInt32(result.Rows[0]["success"]) == 1;
        }

        public async Task<bool> DeleteAdminProfilePicAsync(int adminId)
        {
            // First get the current profile pic URL
            var profile = await GetAdminProfileAsync(adminId);
            if (string.IsNullOrEmpty(profile.ProfilePicUrl))
                return false;

            // Delete the file
            await _imageProcessor.DeleteProfileImage(profile.ProfilePicUrl);

            // Clear in database
            var parameters = new[]
            {
                new MySqlParameter("p_admin_id", adminId)
            };

            var result = await _context.ExecuteQueryAsync("sp_AdminDeleteProfilePic", parameters);
            return result.Rows.Count > 0 && Convert.ToInt32(result.Rows[0]["success"]) == 1;
        }

        public async Task<TeacherProfileResponse> GetTeacherProfileAsync(int teacherId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_teacher_id", teacherId)
            };

            var result = await _context.ExecuteQueryAsync("sp_TeacherViewProfile", parameters);

            if (result.Rows.Count == 0)
                throw new KeyNotFoundException("Teacher profile not found");

            var row = result.Rows[0];
            return new TeacherProfileResponse
            {
                Id = Convert.ToInt32(row["id"]),
                Name = row["name"].ToString(),
                Email = row["email"].ToString(),
                Contact = row["contact"]?.ToString(),
                ProfilePicUrl = row["profile_pic_url"]?.ToString(),
                ProfilePicType = row["profile_picture_type"]?.ToString(),
                Designation = row["designation"].ToString(),
                HireDate = Convert.ToDateTime(row["hire_date"]),
                Qualification = row["qualification"]?.ToString(),
                Specialization = row["specialization"]?.ToString(),
                ExperienceYears = Convert.ToInt32(row["experience_years"]),
                DepartmentName = row["department_name"].ToString()
            };
        }

        public async Task<bool> UpdateTeacherProfileAsync(int teacherId, TeacherProfileUpdateDto dto, IFormFile? profilePicFile = null)
        {
            string? profilePicUrl = null;
            string? profilePicType = null;

            if (profilePicFile != null)
            {
                var uploadResult = await _imageProcessor.ProcessAndSaveProfileImage(profilePicFile, teacherId, "teacher");
                profilePicUrl = uploadResult.FilePath;
                profilePicType = uploadResult.ContentType.Split('/')[1]; // "image/jpeg" -> "jpeg"
            }

            var parameters = new[]
            {
                new MySqlParameter("p_teacher_id", teacherId),
                new MySqlParameter("p_contact", dto.Contact ?? (object)DBNull.Value),
                new MySqlParameter("p_profile_pic_url", profilePicUrl ?? (object)DBNull.Value),
                new MySqlParameter("p_profile_pic_type", profilePicType ?? (object)DBNull.Value)
            };

       
            var result = await _context.ExecuteQueryAsync("sp_TeacherUpdateProfile", parameters);
            return result.Rows.Count > 0 && Convert.ToInt32(result.Rows[0]["success"]) == 1;
        }

        public async Task<bool> UpdateTeacherQualificationsAsync(int teacherId, TeacherQualificationsUpdateDto dto)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_teacher_id", teacherId),
                new MySqlParameter("p_qualification", dto.Qualification ?? (object)DBNull.Value),
                new MySqlParameter("p_specialization", dto.Specialization ?? (object)DBNull.Value),
                new MySqlParameter("p_experience_years", dto.ExperienceYears ?? (object)DBNull.Value)
            };

          
            var result = await _context.ExecuteQueryAsync("sp_TeacherUpdateQualifications", parameters);
            return result.Rows.Count > 0 && Convert.ToInt32(result.Rows[0]["success"]) == 1;
        }

        public async Task<bool> DeleteTeacherProfilePicAsync(int teacherId)
        {
            // First get the current profile pic URL
            var profile = await GetTeacherProfileAsync(teacherId);
            if (string.IsNullOrEmpty(profile.ProfilePicUrl))
                return false;

            // Delete the file
            await _imageProcessor.DeleteProfileImage(profile.ProfilePicUrl);

            // Clear in database
            var parameters = new[]
            {
                new MySqlParameter("p_teacher_id", teacherId)
            };

            
            var result = await _context.ExecuteQueryAsync("sp_TeacherDeleteProfilePic", parameters);
            return result.Rows.Count > 0 && Convert.ToInt32(result.Rows[0]["success"]) == 1;
        }

        public async Task<StudentProfileResponse> GetStudentProfileAsync(int studentId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_student_id", studentId)
            };

            var result = await _context.ExecuteQueryAsync("sp_StudentViewProfile", parameters);

            if (result.Rows.Count == 0)
                throw new KeyNotFoundException("Student profile not found");

            var row = result.Rows[0];
            return new StudentProfileResponse
            {
                Id = Convert.ToInt32(row["id"]),
                Name = row["name"].ToString(),
                Email = row["email"].ToString(),
                Contact = row["contact"]?.ToString(),
                ProfilePicUrl = row["profile_pic_url"]?.ToString(),
                ProfilePicType = row["profile_picture_type"]?.ToString(),
                RegNo = row["reg_no"].ToString(),
                EnrollYear = Convert.ToInt32(row["enroll_year"]),
                CurrentSemester = row["current_semester"] != DBNull.Value ? Convert.ToInt32(row["current_semester"]) : null,
                DepartmentName = row["department_name"].ToString(),
                ProgramName = row["program_name"].ToString(),
                LevelName = row["level_name"].ToString(),
                AcademicStatus = row["academic_status"].ToString(),
                Cgpa = Convert.ToDecimal(row["cgpa"])
            };
        }

        public async Task<bool> UpdateStudentProfileAsync(int studentId, StudentProfileUpdateDto dto, IFormFile? profilePicFile = null)
        {
            string? profilePicUrl = null;
            string? profilePicType = null;

            if (profilePicFile != null)
            {
                var uploadResult = await _imageProcessor.ProcessAndSaveProfileImage(profilePicFile, studentId, "student");
                profilePicUrl = uploadResult.FilePath;
                profilePicType = uploadResult.ContentType.Split('/')[1]; // "image/jpeg" -> "jpeg"
            }

            var parameters = new[]
            {
                new MySqlParameter("p_student_id", studentId),
                new MySqlParameter("p_email", dto.Email ?? (object)DBNull.Value),
                new MySqlParameter("p_contact", dto.Contact ?? (object)DBNull.Value),
                new MySqlParameter("p_profile_pic_url", profilePicUrl ?? (object)DBNull.Value),
                new MySqlParameter("p_profile_pic_type", profilePicType ?? (object)DBNull.Value)
            };

            
            var result = await _context.ExecuteQueryAsync("sp_StudentUpdateProfile", parameters);
            return result.Rows.Count > 0 && Convert.ToInt32(result.Rows[0]["success"]) == 1;
        }

        public async Task<bool> DeleteStudentProfilePicAsync(int studentId)
        {
            // First get the current profile pic URL
            var profile = await GetStudentProfileAsync(studentId);
            if (string.IsNullOrEmpty(profile.ProfilePicUrl))
                return false;

            // Delete the file
            await _imageProcessor.DeleteProfileImage(profile.ProfilePicUrl);

            // Clear in database
            var parameters = new[]
            {
                new MySqlParameter("p_student_id", studentId)
            };

           
            var result = await _context.ExecuteQueryAsync("sp_StudentDeleteProfilePic", parameters);
            return result.Rows.Count > 0 && Convert.ToInt32(result.Rows[0]["success"]) == 1;
        }
    }
}