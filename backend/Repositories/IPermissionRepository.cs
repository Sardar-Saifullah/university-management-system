using backend.Dtos;
using backend.Models;
using System.Threading.Tasks;

namespace backend.Repositories
{
    public interface IPermissionRepository
    {
        Task<ActivityProfileMapping> CheckPermission(string profileName, string activityName);
        Task<List<PermissionResponse>> GetProfilePermissions(string profileName);
    }
}