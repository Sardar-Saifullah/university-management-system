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
                new MySqlParameter("p_override_id", MySqlDbType.Int32) { Direction = ParameterDirection.Output }
            };

            await _context.ExecuteNonQueryAsync("sp_create_credit_limit_override", parameters);

            return Convert.ToInt32(parameters[6].Value);
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

            var result = await _context.ExecuteQueryAsync("sp_get_all_overrides", parameters);

            var overrides = new List<CreditLimitOverride>();
            foreach (DataRow row in result.Rows)
            {
                overrides.Add(MapToOverride(row));
            }

            return overrides;
        }

        public async Task<CreditLimitOverride?> GetByIdAsync(int id)
        {
            // We'll get all and filter by ID since we don't have a specific stored procedure
            var allOverrides = await GetAllAsync(null);
            return allOverrides.FirstOrDefault(o => o.Id == id);
        }

        public async Task<bool> DeleteAsync(int id, int modifiedBy)
        {
            var overrideEntity = await GetByIdAsync(id);
            if (overrideEntity == null) return false;

            overrideEntity.IsActive = false;
            overrideEntity.IsDeleted = true;

            return await UpdateAsync(overrideEntity, modifiedBy);
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
                CreatedBy = row["created_by"] as int?,
                ModifiedAt = Convert.ToDateTime(row["modified_at"]),
                ModifiedBy = row["modified_by"] as int?,

                // Additional properties from join
                PolicyName = row["policy_name"] as string,
                StudentName = row["student_name"] as string,
                StudentRegNo = row["reg_no"] as string,
                ApprovedByName = row["approved_by_name"] as string,
                Status = row["status"] as string
            };
        }
    }
}