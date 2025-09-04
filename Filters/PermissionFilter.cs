using backend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MySql.Data.MySqlClient;
using System.Security.Claims;

namespace backend.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PermissionRequiredAttribute : Attribute
    {
        public string ControllerName { get; }
        public string ActionType { get; }
        public string ResourceName { get; }

        // Constructor for the new structure with controller, action, and resource
        public PermissionRequiredAttribute(string controllerName, string actionType, string resourceName)
        {
            ControllerName = controllerName;
            ActionType = actionType;
            ResourceName = resourceName;
        }

        
    }

    public class PermissionFilter : IAsyncActionFilter
    {
        private readonly IDatabaseContext _context;
       

        public PermissionFilter(IDatabaseContext context)
        {
            _context = context;
            
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Skip permission check for anonymous endpoints
            if (context.ActionDescriptor.EndpointMetadata.Any(em => em is Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute))
            {
                await next();
                return;
            }

            var permissionAttribute = context.ActionDescriptor.EndpointMetadata
                .OfType<PermissionRequiredAttribute>()
                .FirstOrDefault();

            if (permissionAttribute == null)
            {
                await next();
                return;
            }


            var profile = context.HttpContext.User.FindFirstValue("profile");
            if (string.IsNullOrEmpty(profile))
            {
                context.Result = new ForbidResult();
                return;
            }

            try
            {
                var parameters = new[]
                {
                new MySqlParameter("p_profile_name", profile),
                new MySqlParameter("p_controller_name", permissionAttribute.ControllerName),
                new MySqlParameter("p_action_type", permissionAttribute.ActionType),
                new MySqlParameter("p_resource_name", permissionAttribute.ResourceName)
            };

                var result = await _context.ExecuteQueryAsync("sp_CheckPermission", parameters);

                if (result.Rows.Count == 0)
                {
                    context.Result = new ForbidResult();
                    return;
                }

                await next();
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Permission check error: {ex.Message}");
                context.Result = new ForbidResult();
            }
        }
    
    }

    
}

