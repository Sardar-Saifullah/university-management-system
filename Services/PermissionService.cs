using backend.Dtos;
using backend.Repositories;
using System.Data;

public class PermissionService : IPermissionService
{
    private readonly IPermissionRepository _permissionRepository;

    public PermissionService(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public async Task<IEnumerable<PermissionResponse>> GetProfilePermissions(string profileName)
    {
        return await _permissionRepository.GetProfilePermissions(profileName);
    }

    private PermissionResponse MapPermissionResponse(DataRow row)
    {
        return new PermissionResponse
        {
            ProfileId = Convert.ToInt32(row["profile_id"]),
            ProfileName = row["profile_name"].ToString() ?? string.Empty,
            ActivityId = Convert.ToInt32(row["activity_id"]),
            ActivityName = row["activity_name"].ToString() ?? string.Empty,
            ActivityUrl = row["activity_url"].ToString() ?? string.Empty,
            CanCreate = Convert.ToBoolean(row["can_create"]),
            CanRead = Convert.ToBoolean(row["can_read"]),
            CanUpdate = Convert.ToBoolean(row["can_update"]),
            CanDelete = Convert.ToBoolean(row["can_delete"])
        };
    }
}