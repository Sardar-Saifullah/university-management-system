// Repositories/Implementations/CourseHistoryRepository.cs
using backend.Data;
using backend.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace backend.Repositories
{
    public class CourseHistoryRepository : ICourseHistoryRepository
    {
        private readonly IDatabaseContext _context;

        public CourseHistoryRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public async Task<int> AddCourseHistoryAsync(StudentCourseHistory courseHistory)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_student_id", courseHistory.StudentId),
                new MySqlParameter("p_course_id", courseHistory.CourseId),
                new MySqlParameter("p_semester_id", courseHistory.SemesterId),
                new MySqlParameter("p_enrollment_id", courseHistory.EnrollmentId ?? (object)DBNull.Value),
                new MySqlParameter("p_grade", courseHistory.Grade ?? (object)DBNull.Value),
                new MySqlParameter("p_letter_grade", courseHistory.LetterGrade ?? (object)DBNull.Value),
                new MySqlParameter("p_credits_earned", courseHistory.CreditsEarned),
                new MySqlParameter("p_term_gpa", courseHistory.TermGpa ?? (object)DBNull.Value),
                new MySqlParameter("p_status", courseHistory.Status),
                new MySqlParameter("p_is_retake", courseHistory.IsRetake),
                new MySqlParameter("p_original_attempt_id", courseHistory.OriginalAttemptId ?? (object)DBNull.Value),
                new MySqlParameter("p_created_by", courseHistory.CreatedBy),
                new MySqlParameter("p_result_message", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Output }
            };

            await _context.ExecuteNonQueryAsync("sp_admin_add_course_history", parameters);

            var resultMessage = parameters[12].Value.ToString();
            if (resultMessage?.Contains("Error") == true)
            {
                throw new Exception(resultMessage);
            }

            // Get the last inserted ID
            var result = await _context.ExecuteScalarAsync<int>("SELECT LAST_INSERT_ID()");
            return result;
        }

        public async Task<bool> UpdateCourseHistoryAsync(int historyId, StudentCourseHistory courseHistory)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_history_id", historyId),
                new MySqlParameter("p_grade", courseHistory.Grade ?? (object)DBNull.Value),
                new MySqlParameter("p_letter_grade", courseHistory.LetterGrade ?? (object)DBNull.Value),
                new MySqlParameter("p_credits_earned", courseHistory.CreditsEarned),
                new MySqlParameter("p_term_gpa", courseHistory.TermGpa ?? (object)DBNull.Value),
                new MySqlParameter("p_status", courseHistory.Status),
                new MySqlParameter("p_is_retake", courseHistory.IsRetake),
                new MySqlParameter("p_original_attempt_id", courseHistory.OriginalAttemptId ?? (object)DBNull.Value),
                new MySqlParameter("p_modified_by", courseHistory.ModifiedBy),
                new MySqlParameter("p_result_message", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Output }
            };

            await _context.ExecuteNonQueryAsync("sp_admin_update_course_history", parameters);

            var resultMessage = parameters[9].Value.ToString();
            if (resultMessage?.Contains("Error") == true)
            {
                throw new Exception(resultMessage);
            }

            return true;
        }

        public async Task<bool> DeleteCourseHistoryAsync(int historyId, int modifiedBy)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_history_id", historyId),
                new MySqlParameter("p_modified_by", modifiedBy),
                new MySqlParameter("p_result_message", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Output }
            };

            await _context.ExecuteNonQueryAsync("sp_admin_delete_course_history", parameters);

            var resultMessage = parameters[2].Value.ToString();
            if (resultMessage?.Contains("Error") == true)
            {
                throw new Exception(resultMessage);
            }

            return true;
        }

        public async Task<StudentCourseHistory?> GetCourseHistoryByIdAsync(int id)
        {
            var parameters = new[] { new MySqlParameter("p_id", id) };
            var result = await _context.ExecuteQueryAsync("sp_get_course_history_by_id", parameters);

            if (result.Rows.Count == 0) return null;

            return MapToStudentCourseHistory(result.Rows[0]);
        }

        public async Task<IEnumerable<StudentCourseHistory>> GetCourseHistoryByStudentIdAsync(int studentId)
        {
            var parameters = new[] { new MySqlParameter("p_student_id", studentId) };
            var result = await _context.ExecuteQueryAsync("sp_student_get_course_history", parameters);

            return result.Rows.Cast<DataRow>().Select(MapToStudentCourseHistory);
        }

        public async Task<IEnumerable<StudentCourseHistory>> GetCourseHistoryByFiltersAsync(int? studentId, int? courseId, int? semesterId,
            string? status, bool? isRetake, string? academicYear, int? limit, int? offset)
        {
            var parameters = new List<MySqlParameter>();

            if (studentId.HasValue) parameters.Add(new MySqlParameter("p_student_id", studentId.Value));
            if (courseId.HasValue) parameters.Add(new MySqlParameter("p_course_id", courseId.Value));
            if (semesterId.HasValue) parameters.Add(new MySqlParameter("p_semester_id", semesterId.Value));
            if (!string.IsNullOrEmpty(status)) parameters.Add(new MySqlParameter("p_status", status));
            if (isRetake.HasValue) parameters.Add(new MySqlParameter("p_is_retake", isRetake.Value));
            if (!string.IsNullOrEmpty(academicYear)) parameters.Add(new MySqlParameter("p_academic_year", academicYear));
            if (limit.HasValue) parameters.Add(new MySqlParameter("p_limit", limit.Value));
            if (offset.HasValue) parameters.Add(new MySqlParameter("p_offset", offset.Value));

            var result = await _context.ExecuteQueryAsync("sp_admin_get_course_history", parameters.ToArray());

            return result.Rows.Cast<DataRow>().Select(MapToStudentCourseHistory);
        }

        public async Task<GpaCalculationResult> CalculateStudentGpaAsync(int studentId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_student_id", studentId),
                new MySqlParameter("p_cgpa", MySqlDbType.Decimal) { Direction = ParameterDirection.Output },
                new MySqlParameter("p_total_credits_earned", MySqlDbType.Decimal) { Direction = ParameterDirection.Output },
                new MySqlParameter("p_total_credits_attempted", MySqlDbType.Decimal) { Direction = ParameterDirection.Output },
                new MySqlParameter("p_total_grade_points", MySqlDbType.Decimal) { Direction = ParameterDirection.Output },
                new MySqlParameter("p_result_message", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Output }
            };

            await _context.ExecuteNonQueryAsync("sp_calculate_student_gpa", parameters);

            return new GpaCalculationResult
            {
                Cgpa = Convert.ToDecimal(parameters[1].Value),
                TotalCreditsEarned = Convert.ToDecimal(parameters[2].Value),
                TotalCreditsAttempted = Convert.ToDecimal(parameters[3].Value),
                TotalGradePoints = Convert.ToDecimal(parameters[4].Value),
                Message = parameters[5].Value?.ToString() ?? string.Empty
            };
        }

        public async Task<StudentAcademicSummary> GetStudentAcademicSummaryAsync(int studentId)
        {
            var parameters = new[] { new MySqlParameter("p_student_id", studentId) };
            var result = await _context.ExecuteQueryAsync("sp_get_student_academic_summary", parameters);

            if (result.Rows.Count == 0) throw new Exception("Student academic summary not found");

            return MapToStudentAcademicSummary(result.Rows[0]);
        }

        public async Task<TermGpaResult> CalculateTermGpaAsync(int studentId, int semesterId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_student_id", studentId),
                new MySqlParameter("p_semester_id", semesterId),
                new MySqlParameter("p_term_gpa", MySqlDbType.Decimal) { Direction = ParameterDirection.Output },
                new MySqlParameter("p_result_message", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Output }
            };

            await _context.ExecuteNonQueryAsync("sp_calculate_term_gpa", parameters);

            return new TermGpaResult
            {
                TermGpa = parameters[2].Value == DBNull.Value ? null : Convert.ToDecimal(parameters[2].Value),
                Message = parameters[3].Value?.ToString() ?? string.Empty
            };
        }

        public async Task<int> CalculateAllTermGpaForSemesterAsync(int semesterId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_semester_id", semesterId),
                new MySqlParameter("p_students_processed", MySqlDbType.Int32) { Direction = ParameterDirection.Output },
                new MySqlParameter("p_result_message", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Output }
            };

            await _context.ExecuteNonQueryAsync("sp_calculate_all_term_gpa_for_semester", parameters);

            return Convert.ToInt32(parameters[1].Value);
        }

        private StudentCourseHistory MapToStudentCourseHistory(DataRow row)
        {
            return new StudentCourseHistory
            {
                Id = Convert.ToInt32(row["id"]),
                StudentId = Convert.ToInt32(row["student_id"]),
                CourseId = Convert.ToInt32(row["course_id"]),
                SemesterId = Convert.ToInt32(row["semester_id"]),
                EnrollmentId = row["enrollment_id"] == DBNull.Value ? null : Convert.ToInt32(row["enrollment_id"]),
                Grade = row["grade"] == DBNull.Value ? null : Convert.ToDecimal(row["grade"]),
                LetterGrade = row["letter_grade"] == DBNull.Value ? null : Convert.ToString(row["letter_grade"]),
                CreditsEarned = Convert.ToDecimal(row["credits_earned"]),
                TermGpa = row["term_gpa"] == DBNull.Value ? null : Convert.ToDecimal(row["term_gpa"]),
                Status = Convert.ToString(row["status"])!,
                IsRetake = Convert.ToBoolean(row["is_retake"]),
                OriginalAttemptId = row["original_attempt_id"] == DBNull.Value ? null : Convert.ToInt32(row["original_attempt_id"]),
                CreatedAt = Convert.ToDateTime(row["created_at"]),
                ModifiedAt = Convert.ToDateTime(row["modified_at"]),
                IsActive = Convert.ToBoolean(row["is_active"]),
                IsDeleted = Convert.ToBoolean(row["is_deleted"])
            };
        }

        private StudentAcademicSummary MapToStudentAcademicSummary(DataRow row)
        {
            return new StudentAcademicSummary
            {
                StudentId = Convert.ToInt32(row["student_id"]),
                RegNo = Convert.ToString(row["reg_no"])!,
                Name = Convert.ToString(row["name"])!,
                EnrollYear = Convert.ToInt32(row["enroll_year"]),
                CurrentSemester = row["current_semester"] == DBNull.Value ? null : Convert.ToInt32(row["current_semester"]),
                TotalSemestersStudied = Convert.ToInt32(row["total_semesters_studied"]),
                DepartmentName = Convert.ToString(row["department_name"])!,
                DepartmentCode = Convert.ToString(row["department_code"])!,
                ProgramName = Convert.ToString(row["program_name"])!,
                ProgramCode = Convert.ToString(row["program_code"])!,
                DurationSemesters = Convert.ToInt32(row["duration_semesters"]),
                CreditHoursRequired = Convert.ToInt32(row["credit_hours_required"]),
                LevelName = Convert.ToString(row["level_name"])!,
                AcademicStatus = Convert.ToString(row["academic_status"])!,
                Cgpa = Convert.ToDecimal(row["cgpa"]),
                CurrentCreditHours = Convert.ToDecimal(row["current_credit_hours"]),
                CompletedCreditHours = Convert.ToDecimal(row["completed_credit_hours"]),
                AttemptedCreditHours = Convert.ToDecimal(row["attempted_credit_hours"]),
                CompletionPercentage = Convert.ToDecimal(row["completion_percentage"]),
                TotalCoursesTaken = Convert.ToInt32(row["total_courses_taken"]),
                CoursesCompleted = Convert.ToInt32(row["courses_completed"]),
                CoursesFailed = Convert.ToInt32(row["courses_failed"]),
                CoursesRetaken = Convert.ToInt32(row["courses_retaken"]),
                CreatedAt = Convert.ToDateTime(row["created_at"]),
                ModifiedAt = Convert.ToDateTime(row["modified_at"])
            };
        }
    }
}