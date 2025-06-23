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
        public string ActivityName { get; }

        public PermissionRequiredAttribute(string activityName)
        {
            ActivityName = activityName;
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

            // Get the requested URL path
            var path = context.HttpContext.Request.Path.Value?.ToLower();
            if (string.IsNullOrEmpty(path))
            {
                context.Result = new ForbidResult();
                return;
            }

            // Check permissions
            var parameters = new[]
            {
                new MySqlParameter("p_profile_name", profile),
                new MySqlParameter("p_activity_name", permissionAttribute.ActivityName)
            };

            var result = await _context.ExecuteQueryAsync("sp_CheckPermission", parameters);

            if (result.Rows.Count == 0)
            {
                context.Result = new ForbidResult();
                return;
            }

            await next();
        }
    }
}