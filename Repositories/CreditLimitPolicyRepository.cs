// Repositories/Implementations/CreditLimitPolicyRepository.cs
using backend.Data;
using backend.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace backend.Repositories
{
    public class CreditLimitPolicyRepository : ICreditLimitPolicyRepository
    {
        private readonly IDatabaseContext _context;

        public CreditLimitPolicyRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(CreditLimitPolicy policy, int createdBy)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_name", policy.Name),
                new MySqlParameter("p_description", policy.Description ?? (object)DBNull.Value),
                new MySqlParameter("p_applies_to", policy.AppliesTo),
                new MySqlParameter("p_applies_to_id", policy.AppliesToId ?? (object)DBNull.Value),
                new MySqlParameter("p_max_credits", policy.MaxCredits),
                new MySqlParameter("p_min_credits", policy.MinCredits),
                new MySqlParameter("p_created_by", createdBy),
                new MySqlParameter("p_policy_id", MySqlDbType.Int32) { Direction = ParameterDirection.Output }
            };

            await _context.ExecuteNonQueryAsync("sp_create_credit_limit_policy", parameters);

            return Convert.ToInt32(parameters[7].Value);
        }

        public async Task<bool> UpdateAsync(CreditLimitPolicy policy, int modifiedBy)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_policy_id", policy.Id),
                new MySqlParameter("p_name", policy.Name),
                new MySqlParameter("p_description", policy.Description ?? (object)DBNull.Value),
                new MySqlParameter("p_applies_to", policy.AppliesTo),
                new MySqlParameter("p_applies_to_id", policy.AppliesToId ?? (object)DBNull.Value),
                new MySqlParameter("p_max_credits", policy.MaxCredits),
                new MySqlParameter("p_min_credits", policy.MinCredits),
                new MySqlParameter("p_modified_by", modifiedBy),
                new MySqlParameter("p_is_active", policy.IsActive)
            };

            var result = await _context.ExecuteNonQueryAsync("sp_update_credit_limit_policy", parameters);
            return result > 0;
        }

        public async Task<IEnumerable<CreditLimitPolicy>> GetAllAsync()
        {
            var result = await _context.ExecuteQueryAsync("sp_get_all_credit_limit_policies");

            var policies = new List<CreditLimitPolicy>();
            foreach (DataRow row in result.Rows)
            {
                policies.Add(MapToPolicy(row));
            }

            return policies;
        }

        public async Task<CreditLimitPolicy?> GetByIdAsync(int id)
        {
            var parameters = new[] { new MySqlParameter("p_id", id) };
            var result = await _context.ExecuteQueryAsync("sp_get_credit_limit_policy_by_id", parameters);

            if (result.Rows.Count == 0) return null;

            return MapToPolicy(result.Rows[0]);
        }

        public async Task<CreditLimitPolicy?> GetStudentCreditLimitAsync(int studentId)
        {
            var parameters = new[] { new MySqlParameter("p_student_id", studentId) };
            var result = await _context.ExecuteQueryAsync("sp_get_student_credit_limit", parameters);

            if (result.Rows.Count == 0) return null;

            return MapToPolicy(result.Rows[0]);
        }

        public async Task<bool> DeleteAsync(int id, int modifiedBy)
        {
            var policy = await GetByIdAsync(id);
            if (policy == null) return false;

            policy.IsActive = false;
            policy.IsDeleted = true;

            return await UpdateAsync(policy, modifiedBy);
        }

        private CreditLimitPolicy MapToPolicy(DataRow row)
        {
            return new CreditLimitPolicy
            {
                Id = Convert.ToInt32(row["id"]),
                Name = row["name"].ToString() ?? string.Empty,
                Description = row["description"] as string,
                AppliesTo = row["applies_to"].ToString() ?? "all",
                AppliesToId = row["applies_to_id"] as int?,
                MaxCredits = Convert.ToInt32(row["max_credits"]),
                MinCredits = Convert.ToInt32(row["min_credits"]),
                IsActive = Convert.ToBoolean(row["is_active"]),
                CreatedAt = Convert.ToDateTime(row["created_at"]),
                CreatedBy = row["created_by"] as int?,
                ModifiedAt = Convert.ToDateTime(row["modified_at"]),
                ModifiedBy = row["modified_by"] as int?
            };
        }
    }
}