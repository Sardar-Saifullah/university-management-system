using backend.Dtos;

public interface IPermissionService
{
 
    Task<IEnumerable<PermissionResponse>> GetProfilePermissions(string profileName);
}