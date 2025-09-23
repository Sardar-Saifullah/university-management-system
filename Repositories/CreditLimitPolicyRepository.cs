// Repositories/CreditLimitPolicyRepository.cs
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
        new MySqlParameter("p_description", policy.Description),
        new MySqlParameter("p_applies_to", policy.AppliesTo),
        new MySqlParameter("p_applies_to_id", policy.AppliesToId ?? (object)DBNull.Value),
        new MySqlParameter("p_max_credits", policy.MaxCredits),
        new MySqlParameter("p_min_credits", policy.MinCredits),
        new MySqlParameter("p_created_by", createdBy),
        // Add the OUT parameter
        new MySqlParameter("p_policy_id", MySqlDbType.Int32)
        {
            Direction = ParameterDirection.Output
        }
    };

            await _context.ExecuteNonQueryAsync("sp_create_credit_limit_policy", parameters);

            // Get the output parameter value
            var policyIdParam = parameters.FirstOrDefault(p => p.ParameterName == "p_policy_id");
            return Convert.ToInt32(policyIdParam?.Value ?? 0);
        }

        public async Task<bool> UpdateAsync(CreditLimitPolicy policy, int modifiedBy)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_policy_id", policy.Id),
                new MySqlParameter("p_name", policy.Name),
                new MySqlParameter("p_description", policy.Description),
                new MySqlParameter("p_applies_to", policy.AppliesTo),
                new MySqlParameter("p_applies_to_id", policy.AppliesToId ?? (object)DBNull.Value),
                new MySqlParameter("p_max_credits", policy.MaxCredits),
                new MySqlParameter("p_min_credits", policy.MinCredits),
                new MySqlParameter("p_modified_by", modifiedBy),
                new MySqlParameter("p_is_active", policy.IsActive)
            };

            await _context.ExecuteNonQueryAsync("sp_update_credit_limit_policy", parameters);
            return true;
        }

        public async Task<IEnumerable<CreditLimitPolicy>> GetAllAsync()
        {
            var dataTable = await _context.ExecuteQueryAsync("sp_get_all_credit_limit_policies");
            var policies = new List<CreditLimitPolicy>();

            foreach (DataRow row in dataTable.Rows)
            {
                policies.Add(MapFromDataRow(row));
            }

            return policies;
        }

        public async Task<CreditLimitPolicy?> GetByIdAsync(int id)
        {
            var parameters = new[] { new MySqlParameter("p_id", id) };
            var dataTable = await _context.ExecuteQueryAsync("sp_get_credit_limit_policy_by_id", parameters);

            if (dataTable.Rows.Count == 0)
                return null;

            return MapFromDataRow(dataTable.Rows[0]);
        }

        public async Task<CreditLimitPolicy?> GetStudentCreditLimitAsync(int studentId)
        {
            var parameters = new[] { new MySqlParameter("p_student_id", studentId) };
            var dataTable = await _context.ExecuteQueryAsync("sp_get_student_credit_limit", parameters);

            if (dataTable.Rows.Count == 0)
                return null;

            return MapFromDataRow(dataTable.Rows[0]);
        }

        public async Task<bool> DeleteAsync(int id, int modifiedBy)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_policy_id", id),
                new MySqlParameter("p_modified_by", modifiedBy)
            };

            await _context.ExecuteNonQueryAsync("sp_delete_credit_limit_policy", parameters);
            return true;
        }

        private CreditLimitPolicy MapFromDataRow(DataRow row)
        {
            return new CreditLimitPolicy
            {
                Id = Convert.ToInt32(row["id"]),
                Name = row["name"].ToString(),
                Description = row["description"].ToString(),
                AppliesTo = row["applies_to"].ToString(),
                AppliesToId = row["applies_to_id"] == DBNull.Value ? null : Convert.ToInt32(row["applies_to_id"]),
                AppliesToName = row["applies_to_name"]?.ToString(),
                MaxCredits = Convert.ToInt32(row["max_credits"]),
                MinCredits = Convert.ToInt32(row["min_credits"]),
                IsActive = Convert.ToBoolean(row["is_active"]),
                CreatedAt = Convert.ToDateTime(row["created_at"]),
                CreatedBy = row["created_by"] == DBNull.Value ? null : Convert.ToInt32(row["created_by"]),
                CreatedByName = row["created_by_name"]?.ToString(),
                ModifiedAt = row["modified_at"] == DBNull.Value ? DateTime.UtcNow : Convert.ToDateTime(row["modified_at"]),
                ModifiedBy = row["modified_by"] == DBNull.Value ? null : Convert.ToInt32(row["modified_by"]),
                ModifiedByName = row["modified_by_name"]?.ToString()
            };
        }
    }
}