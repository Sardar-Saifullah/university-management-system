using backend.Dtos;
using backend.Repositories;

namespace backend.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;

        public PermissionService(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<bool> CheckPermission(string profileName, string activityName)
        {
            var permission = await _permissionRepository.CheckPermission(profileName, activityName);
            return permission != null && permission.CanRead;
        }

        public async Task<List<PermissionResponse>> GetProfilePermissions(string profileName)
        {
            return await _permissionRepository.GetProfilePermissions(profileName);
        }
    }
}