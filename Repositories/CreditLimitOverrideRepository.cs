// Repositories/Implementations/CreditLimitOverrideRepository.cs
using backend.Data;
using backend.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace backend.Repositories
{
    public class CreditLimitOverrideRepository : ICreditLimitOverrideRepository
    {
        private readonly IDatabaseContext _context;

        public CreditLimitOverrideRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(CreditLimitOverride overrideEntity, int approvedByUserId)
        {
            var parameters = new[]
            {
        new MySqlParameter("p_policy_id", overrideEntity.PolicyId),
        new MySqlParameter("p_student_id", overrideEntity.StudentId),
        new MySqlParameter("p_new_max_credits", overrideEntity.NewMaxCredits),
        new MySqlParameter("p_reason", overrideEntity.Reason),
        new MySqlParameter("p_approved_by", approvedByUserId),
        new MySqlParameter("p_expires_at", overrideEntity.ExpiresAt ?? (object)DBNull.Value),
        new MySqlParameter("p_created_by", approvedByUserId),  // Add this
        new MySqlParameter("p_modified_by", approvedByUserId), // Add this
        new MySqlParameter("p_override_id", MySqlDbType.Int32) { Direction = ParameterDirection.Output }
    };

            await _context.ExecuteNonQueryAsync("sp_create_credit_limit_override", parameters);

            return Convert.ToInt32(parameters[8].Value); // Update index to 8
        }


       
        public async Task<bool> UpdateAsync(CreditLimitOverride overrideEntity, int modifiedBy)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_override_id", overrideEntity.Id),
                new MySqlParameter("p_new_max_credits", overrideEntity.NewMaxCredits),
                new MySqlParameter("p_reason", overrideEntity.Reason),
                new MySqlParameter("p_expires_at", overrideEntity.ExpiresAt ?? (object)DBNull.Value),
                new MySqlParameter("p_is_active", overrideEntity.IsActive),
                new MySqlParameter("p_modified_by", modifiedBy)
            };

            var result = await _context.ExecuteNonQueryAsync("sp_update_credit_limit_override", parameters);
            return result > 0;
        }

        public async Task<IEnumerable<CreditLimitOverride>> GetByStudentIdAsync(int studentId)
        {
            var parameters = new[] { new MySqlParameter("p_student_id", studentId) };
            var result = await _context.ExecuteQueryAsync("sp_get_student_overrides", parameters);

            var overrides = new List<CreditLimitOverride>();
            foreach (DataRow row in result.Rows)
            {
                overrides.Add(MapToOverride(row));
            }

            return overrides;
        }

        public async Task<IEnumerable<CreditLimitOverride>> GetAllAsync(bool? activeOnly = true, int? departmentId = null, int? programId = null)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_filter_active", activeOnly ?? (object)DBNull.Value),
                new MySqlParameter("p_department_id", departmentId ?? (object)DBNull.Value),
                new MySqlParameter("p_program_id", programId ?? (object)DBNull.Value)
            };

            var result = await _context.ExecuteQueryAsync("sp_get_all_credit_limit_overrides", parameters);

            var overrides = new List<CreditLimitOverride>();
            foreach (DataRow row in result.Rows)
            {
                overrides.Add(MapToOverride(row));
            }

            return overrides;
        }

        public async Task<CreditLimitOverride?> GetByIdAsync(int id)
        {
            var parameters = new[] { new MySqlParameter("p_id", id) };
            var result = await _context.ExecuteQueryAsync("sp_get_credit_limit_override_by_id", parameters);

            if (result.Rows.Count == 0)
                return null;

            var row = result.Rows[0];
            return new CreditLimitOverride
            {
                Id = Convert.ToInt32(row["id"]),
                PolicyId = Convert.ToInt32(row["policy_id"]),
                StudentId = Convert.ToInt32(row["student_id"]),
                NewMaxCredits = Convert.ToInt32(row["new_max_credits"]),
                Reason = row["reason"]?.ToString() ?? string.Empty,
                ApprovedBy = Convert.ToInt32(row["approved_by"]),
                ExpiresAt = row["expires_at"] == DBNull.Value ? null : Convert.ToDateTime(row["expires_at"]),
                CreatedAt = Convert.ToDateTime(row["created_at"]),
                CreatedBy = row["created_by"] == DBNull.Value ? null : Convert.ToInt32(row["created_by"]),
                ModifiedAt = row["modified_at"] == DBNull.Value ? DateTime.UtcNow : Convert.ToDateTime(row["modified_at"]),
                ModifiedBy = row["modified_by"] == DBNull.Value ? null : Convert.ToInt32(row["modified_by"]),
                IsActive = Convert.ToBoolean(row["is_active"]),
                IsDeleted = Convert.ToBoolean(row["is_deleted"]),

                // Use safe access with null checks for joined columns
                PolicyName = row.Table.Columns.Contains("policy_name") ? row["policy_name"]?.ToString() : null,
                StudentName = row.Table.Columns.Contains("student_name") ? row["student_name"]?.ToString() : null,
                StudentRegNo = row.Table.Columns.Contains("student_reg_no") ? row["student_reg_no"]?.ToString() : null,
                ApprovedByName = row.Table.Columns.Contains("approved_by_name") ? row["approved_by_name"]?.ToString() : null,
                Status = row.Table.Columns.Contains("status") ? row["status"]?.ToString() : null
            };
        }
        public async Task<bool> DeleteAsync(int id, int modifiedBy)
        {
            try
            {
                var parameters = new[]
                {
            new MySqlParameter("p_override_id", id),
            new MySqlParameter("p_modified_by", modifiedBy)
        };

                var result = await _context.ExecuteNonQueryAsync("sp_delete_credit_limit_override", parameters);
                return result > 0;
            }
            catch (MySqlException ex) when (ex.Number == 1644) // Custom error code for unauthorized
            {
                throw new UnauthorizedAccessException("Only admin users can delete credit limit overrides");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete credit limit override: {ex.Message}");
            }
        }

        public async Task<CreditLimitOverride?> GetActiveOverrideForStudentAsync(int studentId)
        {
            var overrides = await GetByStudentIdAsync(studentId);
            return overrides.FirstOrDefault(o => o.IsActive &&
                (o.ExpiresAt == null || o.ExpiresAt >= DateTime.Today));
        }

        private CreditLimitOverride MapToOverride(DataRow row)
        {
            return new CreditLimitOverride
            {
                Id = Convert.ToInt32(row["id"]),
                PolicyId = Convert.ToInt32(row["policy_id"]),
                StudentId = Convert.ToInt32(row["student_id"]),
                NewMaxCredits = Convert.ToInt32(row["new_max_credits"]),
                Reason = row["reason"].ToString() ?? string.Empty,
                ApprovedBy = Convert.ToInt32(row["approved_by"]),
                ExpiresAt = row["expires_at"] as DateTime?,
                IsActive = Convert.ToBoolean(row["is_active"]),
                CreatedAt = Convert.ToDateTime(row["created_at"]),

                // Use safe access with null checks
                PolicyName = row.Table.Columns.Contains("policy_name") ? row["policy_name"] as string : null,
                StudentName = row.Table.Columns.Contains("student_name") ? row["student_name"] as string : null,
                StudentRegNo = row.Table.Columns.Contains("student_reg_no") ? row["student_reg_no"] as string : null,
                ApprovedByName = row.Table.Columns.Contains("approved_by_name") ? row["approved_by_name"] as string : null,
                Status = row.Table.Columns.Contains("status") ? row["status"] as string : null
            };
        }
    }
    
}