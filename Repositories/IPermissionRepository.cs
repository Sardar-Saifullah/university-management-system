using backend.Dtos;
using System.Data;

namespace backend.Repositories
{
    public interface IPermissionRepository
    {
      
        Task<IEnumerable<PermissionResponse>> GetProfilePermissions(string profileName); // Add this line
    }
}