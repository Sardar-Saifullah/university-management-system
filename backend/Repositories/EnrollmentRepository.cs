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

            return Convert.ToInt32(parameters[4].Value);
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

            return Convert.ToInt32(parameters[4].Value);
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

            return Convert.ToInt32(parameters[3].Value);
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

            return Convert.ToInt32(parameters[4].Value);
        }

        public async Task<IEnumerable<Enrollment>> GetPendingEnrollments()
        {
            var result = await _context.ExecuteQueryAsync("sp_GetPendingEnrollments");
            return MapToEnrollments(result);
        }

        public async Task<int> RequestCourseEnrollment(int studentId, int courseOfferingId, int createdBy)
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

            return Convert.ToInt32(parameters[3].Value);
        }

        public async Task<IEnumerable<CourseSemesterOffering>> GetAvailableCoursesForStudent(int studentId)
        {
            var parameters = new[] { new MySqlParameter("p_student_id", studentId) };
            var result = await _context.ExecuteQueryAsync("sp_GetAvailableCoursesForStudent", parameters);
            return MapToCourseOfferings(result);
        }

        public async Task<IEnumerable<Enrollment>> GetEnrolledCoursesForStudent(int studentId)
        {
            var parameters = new[] { new MySqlParameter("p_student_id", studentId) };
            var result = await _context.ExecuteQueryAsync("sp_GetEnrolledCoursesForStudent", parameters);
            return MapToEnrollments(result);
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

            return Convert.ToInt32(parameters[3].Value);
        }

        public async Task<IEnumerable<Enrollment>> GetEnrollmentHistoryForStudent(int studentId)
        {
            var parameters = new[] { new MySqlParameter("p_student_id", studentId) };
            var result = await _context.ExecuteQueryAsync("sp_GetEnrollmentHistoryForStudent", parameters);
            return MapToEnrollments(result);
        }

        public async Task<CreditLimitPolicy> GetCreditHourStatus(int studentId)
        {
            var parameters = new[] { new MySqlParameter("p_student_id", studentId) };
            var result = await _context.ExecuteQueryAsync("sp_GetCreditHourStatus", parameters);

            if (result.Rows.Count == 0) return null;

            return new CreditLimitPolicy
            {
                MaxCredits = Convert.ToInt32(result.Rows[0]["max_allowed_credits"])
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

            return Convert.ToInt32(parameters[4].Value);
        }

        public async Task<IEnumerable<CourseSemesterOffering>> GetAssignedCoursesForTeacher(int teacherId)
        {
            var parameters = new[] { new MySqlParameter("p_teacher_id", teacherId) };
            var result = await _context.ExecuteQueryAsync("sp_GetAssignedCoursesForTeacher", parameters);
            return MapToCourseOfferings(result);
        }

        public async Task<IEnumerable<Enrollment>> GetEnrolledStudentsForCourse(int teacherId, int courseOfferingId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_teacher_id", teacherId),
                new MySqlParameter("p_course_offering_id", courseOfferingId)
            };

            var result = await _context.ExecuteQueryAsync("sp_GetEnrolledStudentsForCourse", parameters);
            return MapToEnrollments(result);
        }

        public async Task<CourseSemesterOffering> GetCourseDetailsForTeacher(int teacherId, int courseOfferingId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_teacher_id", teacherId),
                new MySqlParameter("p_course_offering_id", courseOfferingId)
            };

            var result = await _context.ExecuteQueryAsync("sp_GetCourseDetailsForTeacher", parameters);

            if (result.Rows.Count == 0) return null;

            return MapToCourseOffering(result.Rows[0]);
        }

        public async Task<StudentProfile> GetStudentAcademicHistory(int teacherId, int studentId)
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

        public async Task<IEnumerable<Enrollment>> GetEnrollmentStatusHistory(int enrollmentId)
        {
            var parameters = new[] { new MySqlParameter("p_enrollment_id", enrollmentId) };
            var result = await _context.ExecuteQueryAsync("sp_GetEnrollmentStatusHistory", parameters);
            return MapToEnrollments(result);
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

            return Convert.ToInt32(parameters[6].Value);
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
                PrerequisitesMet = Convert.ToBoolean(parameters[2].Value),
                MissingPrerequisites = parameters[3].Value?.ToString() ?? string.Empty
            };
        }

        public async Task<CourseLoadDto> GetStudentCurrentCourseLoad(int studentId)
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
                    Id = Convert.ToInt32(row["id"]),
                    StudentId = Convert.ToInt32(row["student_id"]),
                    CourseOfferingId = Convert.ToInt32(row["course_offering_id"]),
                    Status = row["status"].ToString(),
                    EnrollmentDate = Convert.ToDateTime(row["enrollment_date"]),
                    ApprovalDate = row["approval_date"] == DBNull.Value ? null : Convert.ToDateTime(row["approval_date"]),
                    RejectionReason = row["rejection_reason"]?.ToString(),
                    WithdrawalDate = row["withdrawal_date"] == DBNull.Value ? null : Convert.ToDateTime(row["withdrawal_date"]),
                    DropDate = row["drop_date"] == DBNull.Value ? null : Convert.ToDateTime(row["drop_date"])
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
                    Id = Convert.ToInt32(row["course_offering_id"]),
                    CourseId = Convert.ToInt32(row["course_id"]),
                    MaxCapacity = row["max_capacity"] == DBNull.Value ? null : Convert.ToInt32(row["max_capacity"]),
                    CurrentEnrollment = Convert.ToInt32(row["current_enrollment"]),
                    EnrollmentStart = row["enrollment_start"] == DBNull.Value ? null : Convert.ToDateTime(row["enrollment_start"]),
                    EnrollmentEnd = row["enrollment_end"] == DBNull.Value ? null : Convert.ToDateTime(row["enrollment_end"])
                });
            }

            return offerings;
        }

        private CourseSemesterOffering MapToCourseOffering(DataRow row)
        {
            return new CourseSemesterOffering
            {
                Id = Convert.ToInt32(row["course_offering_id"]),
                CourseId = Convert.ToInt32(row["course_id"]),
                MaxCapacity = row["max_capacity"] == DBNull.Value ? null : Convert.ToInt32(row["max_capacity"]),
                CurrentEnrollment = Convert.ToInt32(row["current_enrollment"])
            };
        }

        private StudentProfile MapToStudentProfile(DataRow row)
        {
            return new StudentProfile
            {
                Id = Convert.ToInt32(row["id"]),
                RegistrationNo = row["reg_no"].ToString(),
                Cgpa = Convert.ToDecimal(row["cgpa"]),
                CurrentCreditHours = Convert.ToDecimal(row["current_credit_hours"]),
                CompletedCreditHours = Convert.ToDecimal(row["completed_credit_hours"]),
                AttemptedCreditHours = Convert.ToDecimal(row["attempted_credit_hours"])
            };
        }
    }
}