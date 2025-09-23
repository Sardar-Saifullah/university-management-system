using backend.Dtos;

namespace backend.Services
{
    public interface IPermissionService
    {
        Task<bool> CheckPermission(string profileName, string activityName);
        Task<List<PermissionResponse>> GetProfilePermissions(string profileName);
    }
}