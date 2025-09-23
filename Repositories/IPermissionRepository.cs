using backend.Dtos;
using backend.Models;

namespace backend.Repositories
{
    public interface IPermissionRepository
    {
        Task<ActivityProfileMapping?> CheckPermission(string profileName, string activityName);
        Task<List<PermissionResponse>> GetProfilePermissions(string profileName); // Add <PermissionResponse>
    }
}