using backend.Data;
using backend.Dtos;
using backend.Repositories;
using System.Data;
using MySql.Data.MySqlClient;
public class PermissionRepository : IPermissionRepository
{
    private readonly IDatabaseContext _context;

    public PermissionRepository(IDatabaseContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PermissionResponse>> GetProfilePermissions(string profileName)
    {
        var parameters = new[]
        {
            new MySqlParameter("p_profile_name", profileName)
        };

        var result = await _context.ExecuteQueryAsync("sp_GetProfilePermissions", parameters);

        var permissions = new List<PermissionResponse>();
        foreach (DataRow row in result.Rows)
        {
            permissions.Add(new PermissionResponse
            {
                ProfileId = Convert.ToInt32(row["profile_id"]),
                ProfileName = row["profile_name"].ToString(),
                ActivityId = Convert.ToInt32(row["activity_id"]),
                ActivityName = row["activity_name"].ToString(),
                ActivityUrl = row["activity_url"].ToString(),
                CanCreate = Convert.ToBoolean(row["can_create"]),
                CanRead = Convert.ToBoolean(row["can_read"]),
                CanUpdate = Convert.ToBoolean(row["can_update"]),
                CanDelete = Convert.ToBoolean(row["can_delete"]),
                CanExport = Convert.ToBoolean(row["can_export"])
            });
        }

        return permissions;
    }
}