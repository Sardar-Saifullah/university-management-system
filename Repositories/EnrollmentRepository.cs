// Repositories/Implementations/EnrollmentRepository.cs
using backend.Data;
using backend.Dtos;
using backend.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace backend.Repositories
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly IDatabaseContext _context;

        public EnrollmentRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public async Task<int> AdminEnrollStudent(int studentId, int courseOfferingId, int adminId, int createdBy)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_student_id", studentId),
                new MySqlParameter("p_course_offering_id", courseOfferingId),
                new MySqlParameter("p_admin_id", adminId),
                new MySqlParameter("p_created_by", createdBy),
                new MySqlParameter("p_enrollment_id", MySqlDbType.Int32) { Direction = ParameterDirection.Output },
                new MySqlParameter("p_message", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Output }
            };

            await _context.ExecuteNonQueryAsync("sp_AdminEnrollStudent", parameters);

			return parameters[4].Value == DBNull.Value ? 0 : Convert.ToInt32(parameters[4].Value);
        }

        public async Task<int> AdminUnenrollStudent(int studentId, int courseOfferingId, int adminId, int modifiedBy)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_student_id", studentId),
                new MySqlParameter("p_course_offering_id", courseOfferingId),
                new MySqlParameter("p_admin_id", adminId),
                new MySqlParameter("p_modified_by", modifiedBy),
                new MySqlParameter("p_rows_affected", MySqlDbType.Int32) { Direction = ParameterDirection.Output },
                new MySqlParameter("p_message", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Output }
            };

            await _context.ExecuteNonQueryAsync("sp_AdminUnenrollStudent", parameters);

			return parameters[4].Value == DBNull.Value ? 0 : Convert.ToInt32(parameters[4].Value);
        }

        public async Task<int> ApproveEnrollment(int enrollmentId, int adminId, int modifiedBy)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_enrollment_id", enrollmentId),
                new MySqlParameter("p_admin_id", adminId),
                new MySqlParameter("p_modified_by", modifiedBy),
                new MySqlParameter("p_rows_affected", MySqlDbType.Int32) { Direction = ParameterDirection.Output },
                new MySqlParameter("p_message", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Output }
            };

            await _context.ExecuteNonQueryAsync("sp_ApproveEnrollment", parameters);

			return parameters[3].Value == DBNull.Value ? 0 : Convert.ToInt32(parameters[3].Value);
        }

        public async Task<int> RejectEnrollment(int enrollmentId, int adminId, string rejectionReason, int modifiedBy)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_enrollment_id", enrollmentId),
                new MySqlParameter("p_admin_id", adminId),
                new MySqlParameter("p_rejection_reason", rejectionReason),
                new MySqlParameter("p_modified_by", modifiedBy),
                new MySqlParameter("p_rows_affected", MySqlDbType.Int32) { Direction = ParameterDirection.Output },
                new MySqlParameter("p_message", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Output }
            };

            await _context.ExecuteNonQueryAsync("sp_RejectEnrollment", parameters);

			return parameters[4].Value == DBNull.Value ? 0 : Convert.ToInt32(parameters[4].Value);
        }

        public async Task<IEnumerable<Enrollment>> GetPendingEnrollments()
        {
            var result = await _context.ExecuteQueryAsync("sp_GetPendingEnrollments");
            return MapToEnrollments(result);
        }

		public async Task<EnrollmentResponseDto> RequestCourseEnrollment(int studentId, int courseOfferingId, int createdBy)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_student_id", studentId),
                new MySqlParameter("p_course_offering_id", courseOfferingId),
                new MySqlParameter("p_created_by", createdBy),
                new MySqlParameter("p_enrollment_id", MySqlDbType.Int32) { Direction = ParameterDirection.Output },
                new MySqlParameter("p_message", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Output }
            };

            await _context.ExecuteNonQueryAsync("sp_RequestCourseEnrollment", parameters);

			var idObj = parameters[3].Value;
			var msgObj = parameters[4].Value;
			return new EnrollmentResponseDto
			{
				Id = idObj == DBNull.Value ? 0 : Convert.ToInt32(idObj),
				StudentId = studentId,
				CourseOfferingId = courseOfferingId,
				Status = idObj == DBNull.Value ? "error" : "pending",
				EnrollmentDate = DateTime.UtcNow,
				Message = msgObj == DBNull.Value ? string.Empty : msgObj?.ToString() ?? string.Empty
			};
		}

		public async Task<IEnumerable<CourseOfferingDto>> GetAvailableCoursesForStudent(int studentId)
        {
            var parameters = new[] { new MySqlParameter("p_student_id", studentId) };
            var result = await _context.ExecuteQueryAsync("sp_GetAvailableCoursesForStudent", parameters);
			var list = new List<CourseOfferingDto>();
			foreach (DataRow row in result.Rows)
			{
				list.Add(MapToCourseOfferingDto(row));
			}
			return list;
		}

		public async Task<IEnumerable<EnrolledCourseDto>> GetEnrolledCoursesForStudent(int studentId)
        {
            var parameters = new[] { new MySqlParameter("p_student_id", studentId) };
            var result = await _context.ExecuteQueryAsync("sp_GetEnrolledCoursesForStudent", parameters);
			var list = new List<EnrolledCourseDto>();
			foreach (DataRow row in result.Rows)
			{
				list.Add(new EnrolledCourseDto
				{
					EnrollmentId = row.Table.Columns.Contains("enrollment_id") ? Convert.ToInt32(row["enrollment_id"]) : (row.Table.Columns.Contains("id") ? Convert.ToInt32(row["id"]) : 0),
					Status = row.Table.Columns.Contains("status") ? row["status"].ToString() ?? string.Empty : string.Empty,
					EnrollmentDate = row.Table.Columns.Contains("enrollment_date") && row["enrollment_date"] != DBNull.Value ? Convert.ToDateTime(row["enrollment_date"]) : DateTime.MinValue,
					ApprovalDate = row.Table.Columns.Contains("approval_date") && row["approval_date"] != DBNull.Value ? Convert.ToDateTime(row["approval_date"]) : (DateTime?)null,
					RejectionReason = row.Table.Columns.Contains("rejection_reason") && row["rejection_reason"] != DBNull.Value ? row["rejection_reason"].ToString() : null,
					WithdrawalDate = row.Table.Columns.Contains("withdrawal_date") && row["withdrawal_date"] != DBNull.Value ? Convert.ToDateTime(row["withdrawal_date"]) : (DateTime?)null,
					DropDate = row.Table.Columns.Contains("drop_date") && row["drop_date"] != DBNull.Value ? Convert.ToDateTime(row["drop_date"]) : (DateTime?)null,
					CourseId = row.Table.Columns.Contains("course_id") && row["course_id"] != DBNull.Value ? Convert.ToInt32(row["course_id"]) : 0,
					Code = row.Table.Columns.Contains("course_code") && row["course_code"] != DBNull.Value ? row["course_code"].ToString() : (row.Table.Columns.Contains("code") && row["code"] != DBNull.Value ? row["code"].ToString() : string.Empty),
					Title = row.Table.Columns.Contains("course_title") && row["course_title"] != DBNull.Value ? row["course_title"].ToString() : (row.Table.Columns.Contains("title") && row["title"] != DBNull.Value ? row["title"].ToString() : string.Empty),
					Description = row.Table.Columns.Contains("description") ? row["description"].ToString() ?? string.Empty : string.Empty,
					CreditHours = row.Table.Columns.Contains("credit_hours") && row["credit_hours"] != DBNull.Value ? Convert.ToDecimal(row["credit_hours"]) : 0m,
					SemesterName = row.Table.Columns.Contains("semester_name") ? row["semester_name"].ToString() ?? string.Empty : string.Empty,
					AcademicYear = row.Table.Columns.Contains("academic_year") ? row["academic_year"].ToString() ?? string.Empty : string.Empty,
					StartDate = row.Table.Columns.Contains("start_date") && row["start_date"] != DBNull.Value ? Convert.ToDateTime(row["start_date"]) : DateTime.MinValue,
					EndDate = row.Table.Columns.Contains("end_date") && row["end_date"] != DBNull.Value ? Convert.ToDateTime(row["end_date"]) : DateTime.MinValue,
					DepartmentName = row.Table.Columns.Contains("department_name") ? row["department_name"].ToString() ?? string.Empty : string.Empty,
					ProgramName = row.Table.Columns.Contains("program_name") ? row["program_name"].ToString() ?? string.Empty : string.Empty,
					LevelName = row.Table.Columns.Contains("level_name") ? row["level_name"].ToString() ?? string.Empty : string.Empty,
					ApprovedByName = row.Table.Columns.Contains("approved_by_name") ? row["approved_by_name"].ToString() ?? string.Empty : string.Empty
				});
			}
			return list;
        }

        public async Task<int> CancelEnrollmentRequest(int enrollmentId, int studentId, int modifiedBy)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_enrollment_id", enrollmentId),
                new MySqlParameter("p_student_id", studentId),
                new MySqlParameter("p_modified_by", modifiedBy),
                new MySqlParameter("p_rows_affected", MySqlDbType.Int32) { Direction = ParameterDirection.Output },
                new MySqlParameter("p_message", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Output }
            };

            await _context.ExecuteNonQueryAsync("sp_CancelEnrollmentRequest", parameters);

			return parameters[3].Value == DBNull.Value ? 0 : Convert.ToInt32(parameters[3].Value);
        }

        public async Task<IEnumerable<Enrollment>> GetEnrollmentHistoryForStudent(int studentId)
        {
            var parameters = new[] { new MySqlParameter("p_student_id", studentId) };
            var result = await _context.ExecuteQueryAsync("sp_GetEnrollmentHistoryForStudent", parameters);
            return MapToEnrollments(result);
        }

		public async Task<CreditHourStatusDto?> GetCreditHourStatus(int studentId)
        {
            var parameters = new[] { new MySqlParameter("p_student_id", studentId) };
            var result = await _context.ExecuteQueryAsync("sp_GetCreditHourStatus", parameters);

            if (result.Rows.Count == 0) return null;

			var row = result.Rows[0];
			return new CreditHourStatusDto
			{
				CurrentCreditHours = row.Table.Columns.Contains("current_credit_hours") && row["current_credit_hours"] != DBNull.Value ? Convert.ToDecimal(row["current_credit_hours"]) : 0m,
				MaxAllowedCredits = row.Table.Columns.Contains("max_allowed_credits") && row["max_allowed_credits"] != DBNull.Value ? Convert.ToInt32(row["max_allowed_credits"]) : 0,
				RemainingCredits = row.Table.Columns.Contains("remaining_credits") && row["remaining_credits"] != DBNull.Value ? Convert.ToDecimal(row["remaining_credits"]) : 0m,
				OverrideReason = row.Table.Columns.Contains("override_reason") && row["override_reason"] != DBNull.Value ? row["override_reason"].ToString() : null,
				OverrideExpiry = row.Table.Columns.Contains("override_expiry") && row["override_expiry"] != DBNull.Value ? Convert.ToDateTime(row["override_expiry"]) : (DateTime?)null,
				ApprovedBy = row.Table.Columns.Contains("approved_by") && row["approved_by"] != DBNull.Value ? row["approved_by"].ToString() : null,
				HasOverride = row.Table.Columns.Contains("override_reason") && row["override_reason"] != DBNull.Value
            };
        }

        public async Task<int> WithdrawFromCourse(int enrollmentId, int studentId, int modifiedBy, string reason)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_enrollment_id", enrollmentId),
                new MySqlParameter("p_student_id", studentId),
                new MySqlParameter("p_modified_by", modifiedBy),
                new MySqlParameter("p_reason", reason),
                new MySqlParameter("p_rows_affected", MySqlDbType.Int32) { Direction = ParameterDirection.Output },
                new MySqlParameter("p_message", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Output }
            };

            await _context.ExecuteNonQueryAsync("sp_WithdrawFromCourse", parameters);

			return parameters[4].Value == DBNull.Value ? 0 : Convert.ToInt32(parameters[4].Value);
        }

		public async Task<IEnumerable<CourseOfferingDto>> GetAssignedCoursesForTeacher(int teacherId)
        {
            var parameters = new[] { new MySqlParameter("p_teacher_id", teacherId) };
            var result = await _context.ExecuteQueryAsync("sp_GetAssignedCoursesForTeacher", parameters);
			var list = new List<CourseOfferingDto>();
			foreach (DataRow row in result.Rows)
			{
				list.Add(MapToCourseOfferingDto(row));
			}
			return list;
		}

		public async Task<IEnumerable<EnrolledStudentDto>> GetEnrolledStudentsForCourse(int teacherId, int courseOfferingId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_teacher_id", teacherId),
                new MySqlParameter("p_course_offering_id", courseOfferingId)
            };

            var result = await _context.ExecuteQueryAsync("sp_GetEnrolledStudentsForCourse", parameters);
			var list = new List<EnrolledStudentDto>();
			foreach (DataRow row in result.Rows)
			{
				list.Add(new EnrolledStudentDto
				{
					EnrollmentId = row.Table.Columns.Contains("enrollment_id") && row["enrollment_id"] != DBNull.Value ? Convert.ToInt32(row["enrollment_id"]) : 0,
					Status = row.Table.Columns.Contains("status") ? row["status"].ToString() ?? string.Empty : string.Empty,
					EnrollmentDate = row.Table.Columns.Contains("enrollment_date") && row["enrollment_date"] != DBNull.Value ? Convert.ToDateTime(row["enrollment_date"]) : DateTime.MinValue,
					ApprovalDate = row.Table.Columns.Contains("approval_date") && row["approval_date"] != DBNull.Value ? Convert.ToDateTime(row["approval_date"]) : (DateTime?)null,
					StudentId = row.Table.Columns.Contains("student_id") && row["student_id"] != DBNull.Value ? Convert.ToInt32(row["student_id"]) : 0,
					RegNo = row.Table.Columns.Contains("reg_no") ? row["reg_no"].ToString() ?? string.Empty : string.Empty,
					StudentName = row.Table.Columns.Contains("student_name") ? row["student_name"].ToString() ?? string.Empty : string.Empty,
					CurrentSemester = row.Table.Columns.Contains("current_semester") && row["current_semester"] != DBNull.Value ? Convert.ToInt32(row["current_semester"]) : (int?)null,
					Cgpa = row.Table.Columns.Contains("cgpa") && row["cgpa"] != DBNull.Value ? Convert.ToDecimal(row["cgpa"]) : 0m,
					CurrentCreditHours = row.Table.Columns.Contains("current_credit_hours") && row["current_credit_hours"] != DBNull.Value ? Convert.ToDecimal(row["current_credit_hours"]) : 0m,
					DepartmentName = row.Table.Columns.Contains("department_name") ? row["department_name"].ToString() ?? string.Empty : string.Empty,
					ProgramName = row.Table.Columns.Contains("program_name") ? row["program_name"].ToString() ?? string.Empty : string.Empty,
					LevelName = row.Table.Columns.Contains("level_name") ? row["level_name"].ToString() ?? string.Empty : string.Empty
				});
			}
			return list;
		}

		public async Task<CourseOfferingDto?> GetCourseDetailsForTeacher(int teacherId, int courseOfferingId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_teacher_id", teacherId),
                new MySqlParameter("p_course_offering_id", courseOfferingId)
            };

            var result = await _context.ExecuteQueryAsync("sp_GetCourseDetailsForTeacher", parameters);

            if (result.Rows.Count == 0) return null;

			return MapToCourseOfferingDto(result.Rows[0]);
        }

		public async Task<StudentProfile?> GetStudentAcademicHistory(int teacherId, int studentId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_teacher_id", teacherId),
                new MySqlParameter("p_student_id", studentId)
            };

            var result = await _context.ExecuteQueryAsync("sp_GetStudentAcademicHistory", parameters);

            if (result.Rows.Count == 0) return null;

            return MapToStudentProfile(result.Rows[0]);
        }

		public async Task<IEnumerable<EnrollmentStatusHistoryDto>> GetEnrollmentStatusHistory(int enrollmentId)
        {
            var parameters = new[] { new MySqlParameter("p_enrollment_id", enrollmentId) };
            var result = await _context.ExecuteQueryAsync("sp_GetEnrollmentStatusHistory", parameters);

			var list = new List<EnrollmentStatusHistoryDto>();
			foreach (DataRow row in result.Rows)
			{
				list.Add(new EnrollmentStatusHistoryDto
				{
					Id = Convert.ToInt32(row["id"]),
					OldStatus = row.Table.Columns.Contains("old_status") ? row["old_status"].ToString() ?? string.Empty : string.Empty,
					NewStatus = row.Table.Columns.Contains("new_status") ? row["new_status"].ToString() ?? string.Empty : string.Empty,
					ChangeDate = row.Table.Columns.Contains("change_date") && row["change_date"] != DBNull.Value ? Convert.ToDateTime(row["change_date"]) : DateTime.MinValue,
					Reason = row.Table.Columns.Contains("reason") && row["reason"] != DBNull.Value ? row["reason"].ToString() ?? string.Empty : string.Empty,
					ChangedBy = row.Table.Columns.Contains("changed_by") && row["changed_by"] != DBNull.Value ? row["changed_by"].ToString() ?? string.Empty : string.Empty
				});
			}
			return list;
        }

        public async Task<int> OverrideCreditLimit(int studentId, int policyId, int newMaxCredits, string reason, int approvedBy, DateTime? expiresAt)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_student_id", studentId),
                new MySqlParameter("p_policy_id", policyId),
                new MySqlParameter("p_new_max_credits", newMaxCredits),
                new MySqlParameter("p_reason", reason),
                new MySqlParameter("p_approved_by", approvedBy),
                new MySqlParameter("p_expires_at", expiresAt),
                new MySqlParameter("p_override_id", MySqlDbType.Int32) { Direction = ParameterDirection.Output },
                new MySqlParameter("p_message", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Output }
            };

            await _context.ExecuteNonQueryAsync("sp_OverrideCreditLimit", parameters);

			return parameters[6].Value == DBNull.Value ? 0 : Convert.ToInt32(parameters[6].Value);
        }

        public async Task<PrerequisiteCheckDto> CheckCoursePrerequisites(int studentId, int courseId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_student_id", studentId),
                new MySqlParameter("p_course_id", courseId),
                new MySqlParameter("p_prerequisites_met", MySqlDbType.Bit) { Direction = ParameterDirection.Output },
                new MySqlParameter("p_missing_prerequisites", MySqlDbType.Text) { Direction = ParameterDirection.Output }
            };

            await _context.ExecuteNonQueryAsync("sp_CheckCoursePrerequisites", parameters);

            return new PrerequisiteCheckDto
            {
				PrerequisitesMet = parameters[2].Value != DBNull.Value && Convert.ToBoolean(parameters[2].Value),
				MissingPrerequisites = parameters[3].Value == DBNull.Value ? string.Empty : parameters[3].Value?.ToString() ?? string.Empty
            };
        }

		public async Task<CourseLoadDto?> GetStudentCurrentCourseLoad(int studentId)
        {
            var parameters = new[] { new MySqlParameter("p_student_id", studentId) };
            var result = await _context.ExecuteQueryAsync("sp_GetStudentCurrentCourseLoad", parameters);

            if (result.Rows.Count == 0) return null;

            return new CourseLoadDto
            {
                StudentId = Convert.ToInt32(result.Rows[0]["student_id"]),
                RegNo = result.Rows[0]["reg_no"].ToString(),
                StudentName = result.Rows[0]["student_name"].ToString(),
                CurrentCreditHours = Convert.ToDecimal(result.Rows[0]["current_credit_hours"]),
                CreditLimit = Convert.ToInt32(result.Rows[0]["credit_limit"]),
                RemainingCredits = Convert.ToDecimal(result.Rows[0]["remaining_credits"]),
                CurrentCourses = result.Rows[0]["current_courses"].ToString()
            };
        }

        // Helper methods to map DataTable to models
        private IEnumerable<Enrollment> MapToEnrollments(DataTable dataTable)
        {
            var enrollments = new List<Enrollment>();

            foreach (DataRow row in dataTable.Rows)
            {
                enrollments.Add(new Enrollment
                {
					Id = row.Table.Columns.Contains("id") ? Convert.ToInt32(row["id"]) : 0,
					StudentId = row.Table.Columns.Contains("student_id") ? Convert.ToInt32(row["student_id"]) : 0,
					CourseOfferingId = row.Table.Columns.Contains("course_offering_id") ? Convert.ToInt32(row["course_offering_id"]) : 0,
					Status = row.Table.Columns.Contains("status") ? row["status"].ToString() ?? string.Empty : string.Empty,
					EnrollmentDate = row.Table.Columns.Contains("enrollment_date") && row["enrollment_date"] != DBNull.Value ? Convert.ToDateTime(row["enrollment_date"]) : DateTime.MinValue,
					ApprovalDate = row.Table.Columns.Contains("approval_date") && row["approval_date"] != DBNull.Value ? Convert.ToDateTime(row["approval_date"]) : (DateTime?)null,
					RejectionReason = row.Table.Columns.Contains("rejection_reason") && row["rejection_reason"] != DBNull.Value ? row["rejection_reason"].ToString() : null,
					WithdrawalDate = row.Table.Columns.Contains("withdrawal_date") && row["withdrawal_date"] != DBNull.Value ? Convert.ToDateTime(row["withdrawal_date"]) : (DateTime?)null,
					DropDate = row.Table.Columns.Contains("drop_date") && row["drop_date"] != DBNull.Value ? Convert.ToDateTime(row["drop_date"]) : (DateTime?)null
                });
            }

            return enrollments;
        }

        private IEnumerable<CourseSemesterOffering> MapToCourseOfferings(DataTable dataTable)
        {
            var offerings = new List<CourseSemesterOffering>();

            foreach (DataRow row in dataTable.Rows)
            {
                offerings.Add(new CourseSemesterOffering
                {
					Id = row.Table.Columns.Contains("course_offering_id") ? Convert.ToInt32(row["course_offering_id"]) : (row.Table.Columns.Contains("id") ? Convert.ToInt32(row["id"]) : 0),
					CourseId = row.Table.Columns.Contains("course_id") ? Convert.ToInt32(row["course_id"]) : 0,
					MaxCapacity = row.Table.Columns.Contains("max_capacity") && row["max_capacity"] != DBNull.Value ? Convert.ToInt32(row["max_capacity"]) : (int?)null,
					CurrentEnrollment = row.Table.Columns.Contains("current_enrollment") && row["current_enrollment"] != DBNull.Value ? Convert.ToInt32(row["current_enrollment"]) : 0,
					EnrollmentStart = row.Table.Columns.Contains("enrollment_start") && row["enrollment_start"] != DBNull.Value ? Convert.ToDateTime(row["enrollment_start"]) : (DateTime?)null,
					EnrollmentEnd = row.Table.Columns.Contains("enrollment_end") && row["enrollment_end"] != DBNull.Value ? Convert.ToDateTime(row["enrollment_end"]) : (DateTime?)null
                });
            }

            return offerings;
        }

		private CourseOfferingDto MapToCourseOfferingDto(DataRow row)
		{
			return new CourseOfferingDto
			{
				Id = row.Table.Columns.Contains("course_offering_id") ? Convert.ToInt32(row["course_offering_id"]) : (row.Table.Columns.Contains("id") ? ConvertToInt(row["id"]) : 0),
				Code = GetString(row, "code", "course_code"),
				Title = GetString(row, "title", "course_title"),
				Description = GetString(row, "description"),
				CreditHours = GetDecimal(row, "credit_hours"),
				IsElective = GetBool(row, "is_elective"),
				MaxCapacity = GetNullableInt(row, "max_capacity"),
				CurrentEnrollment = GetInt(row, "current_enrollment"),
				AvailableSeats = GetInt(row, "available_seats"),
				SemesterName = GetString(row, "semester_name"),
				AcademicYear = GetString(row, "academic_year"),
				StartDate = GetDateTime(row, "start_date"),
				EndDate = GetDateTime(row, "end_date"),
				DepartmentName = GetString(row, "department_name"),
				ProgramName = GetString(row, "program_name"),
				LevelName = GetString(row, "level_name"),
				PrerequisiteCourses = GetString(row, "prerequisite_courses")
			};
		}

		private static int ConvertToInt(object value) => value == DBNull.Value ? 0 : Convert.ToInt32(value);
		private static string GetString(DataRow row, params string[] names)
		{
			foreach (var name in names)
				if (row.Table.Columns.Contains(name) && row[name] != DBNull.Value)
					return row[name].ToString() ?? string.Empty;
			return string.Empty;
		}
		private static int GetInt(DataRow row, string name) => row.Table.Columns.Contains(name) && row[name] != DBNull.Value ? Convert.ToInt32(row[name]) : 0;
		private static int? GetNullableInt(DataRow row, string name) => row.Table.Columns.Contains(name) && row[name] != DBNull.Value ? Convert.ToInt32(row[name]) : (int?)null;
		private static decimal GetDecimal(DataRow row, string name) => row.Table.Columns.Contains(name) && row[name] != DBNull.Value ? Convert.ToDecimal(row[name]) : 0m;
		private static bool GetBool(DataRow row, string name) => row.Table.Columns.Contains(name) && row[name] != DBNull.Value && (row[name] is bool ? (bool)row[name] : Convert.ToInt32(row[name]) == 1);
		private static DateTime GetDateTime(DataRow row, string name) => row.Table.Columns.Contains(name) && row[name] != DBNull.Value ? Convert.ToDateTime(row[name]) : DateTime.MinValue;

        private CourseSemesterOffering MapToCourseOffering(DataRow row)
        {
            return new CourseSemesterOffering
            {
				Id = row.Table.Columns.Contains("course_offering_id") ? Convert.ToInt32(row["course_offering_id"]) : (row.Table.Columns.Contains("id") ? Convert.ToInt32(row["id"]) : 0),
				CourseId = row.Table.Columns.Contains("course_id") ? Convert.ToInt32(row["course_id"]) : 0,
				MaxCapacity = row.Table.Columns.Contains("max_capacity") && row["max_capacity"] != DBNull.Value ? Convert.ToInt32(row["max_capacity"]) : (int?)null,
				CurrentEnrollment = row.Table.Columns.Contains("current_enrollment") && row["current_enrollment"] != DBNull.Value ? Convert.ToInt32(row["current_enrollment"]) : 0
            };
        }

        private StudentProfile MapToStudentProfile(DataRow row)
        {
            return new StudentProfile
            {
				Id = row.Table.Columns.Contains("id") ? Convert.ToInt32(row["id"]) : 0,
				UserId = row.Table.Columns.Contains("user_id") && row["user_id"] != DBNull.Value ? Convert.ToInt32(row["user_id"]) : 0,
				RegistrationNo = row.Table.Columns.Contains("reg_no") ? row["reg_no"].ToString() ?? string.Empty : string.Empty,
				EnrollYear = row.Table.Columns.Contains("enroll_year") && row["enroll_year"] != DBNull.Value ? Convert.ToInt32(row["enroll_year"]) : 0,
				CurrentSemester = row.Table.Columns.Contains("current_semester") && row["current_semester"] != DBNull.Value ? Convert.ToInt32(row["current_semester"]) : (int?)null,
				DepartmentId = row.Table.Columns.Contains("department_id") && row["department_id"] != DBNull.Value ? Convert.ToInt32(row["department_id"]) : 0,
				DepartmentName = row.Table.Columns.Contains("department_name") ? row["department_name"].ToString() ?? string.Empty : string.Empty,
				ProgramId = row.Table.Columns.Contains("program_id") && row["program_id"] != DBNull.Value ? Convert.ToInt32(row["program_id"]) : 0,
				ProgramName = row.Table.Columns.Contains("program_name") ? row["program_name"].ToString() ?? string.Empty : string.Empty,
				LevelId = row.Table.Columns.Contains("level_id") && row["level_id"] != DBNull.Value ? Convert.ToInt32(row["level_id"]) : 0,
				LevelName = row.Table.Columns.Contains("level_name") ? row["level_name"].ToString() ?? string.Empty : string.Empty,
				AcademicStatus = row.Table.Columns.Contains("academic_status") ? row["academic_status"].ToString() ?? string.Empty : string.Empty,
				Cgpa = row.Table.Columns.Contains("cgpa") && row["cgpa"] != DBNull.Value ? Convert.ToDecimal(row["cgpa"]) : 0m,
				CurrentCreditHours = row.Table.Columns.Contains("current_credit_hours") && row["current_credit_hours"] != DBNull.Value ? Convert.ToDecimal(row["current_credit_hours"]) : 0m,
				CompletedCreditHours = row.Table.Columns.Contains("completed_credit_hours") && row["completed_credit_hours"] != DBNull.Value ? Convert.ToDecimal(row["completed_credit_hours"]) : 0m,
				AttemptedCreditHours = row.Table.Columns.Contains("attempted_credit_hours") && row["attempted_credit_hours"] != DBNull.Value ? Convert.ToDecimal(row["attempted_credit_hours"]) : 0m
			};
		}
    }
}