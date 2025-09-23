using backend.Data;
using backend.Dtos;
using backend.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace backend.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly IDatabaseContext _context;

        public PermissionRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public async Task<ActivityProfileMapping?> CheckPermission(string profileName, string activityName)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_profile_name", profileName),
                new MySqlParameter("p_activity_name", activityName)
            };

            var result = await _context.ExecuteQueryAsync("sp_CheckPermission", parameters);

            return result.Rows.Count == 0 ? null : MapToActivityProfileMapping(result.Rows[0]);
        }

        public async Task<List<PermissionResponse>> GetProfilePermissions(string profileName)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_profile_name", profileName)
            };

            var result = await _context.ExecuteQueryAsync("sp_GetProfilePermissions", parameters);
            var permissions = new List<PermissionResponse>();

            foreach (DataRow row in result.Rows)
            {
                permissions.Add(MapToPermissionResponse(row));
            }

            return permissions;
        }

        private PermissionResponse MapToPermissionResponse(DataRow row)
        {
            return new PermissionResponse
            {
                ProfileId = Convert.ToInt32(row["profile_id"]),
                ProfileName = row["profile_name"].ToString(),
                ActivityId = Convert.ToInt32(row["activity_id"]),
                ActivityName = row["activity_name"].ToString(),
                ControllerName = row["controller_name"].ToString(),
                ActionType = row["action_type"].ToString(),
                ResourceName = row["resource_name"].ToString(),
                ActivityUrl = row["activity_url"].ToString(),
                CanCreate = Convert.ToBoolean(row["can_create"]),
                CanRead = Convert.ToBoolean(row["can_read"]),
                CanUpdate = Convert.ToBoolean(row["can_update"]),
                CanDelete = Convert.ToBoolean(row["can_delete"]),
                CanExport = Convert.ToBoolean(row["can_export"]),
                IsActive = Convert.ToBoolean(row["mapping_active"])
            };
        }

        private ActivityProfileMapping MapToActivityProfileMapping(DataRow row)
        {
            return new ActivityProfileMapping
            {
                Id = Convert.ToInt32(row["id"]),
                ProfileId = Convert.ToInt32(row["profile_id"]),
                ActivityId = Convert.ToInt32(row["activity_id"]),
                CanCreate = Convert.ToBoolean(row["can_create"]),
                CanRead = Convert.ToBoolean(row["can_read"]),
                CanUpdate = Convert.ToBoolean(row["can_update"]),
                CanDelete = Convert.ToBoolean(row["can_delete"]),
                CanExport = Convert.ToBoolean(row["can_export"])
            };
        }
    }
}