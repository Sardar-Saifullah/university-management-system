using backend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MySql.Data.MySqlClient;
using System.Data;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace backend.Filters
{
    public class SessionManagementFilter : IAsyncActionFilter
    {
        private readonly IDatabaseContext _context;

        public SessionManagementFilter(IDatabaseContext context)
        {
            _context = context;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Skip session check for login/register endpoints
            if (context.ActionDescriptor.EndpointMetadata.Any(em => em is Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute))
            {
                await next();
                return;
            }

            var jti = context.HttpContext.User.FindFirstValue(JwtRegisteredClaimNames.Jti);
            var userId = context.HttpContext.User.FindFirstValue("id");

            if (string.IsNullOrEmpty(jti) || string.IsNullOrEmpty(userId))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // Check session validity
            var revokeCheckParams = new[]
             {
                new MySqlParameter("p_jti", jti)
            };

            var revokeResult = await _context.ExecuteQueryAsync("sp_CheckSessionRevoked", revokeCheckParams);

            if (revokeResult.Rows.Count > 0 && Convert.ToBoolean(revokeResult.Rows[0]["is_revoked"]))
            {
                context.Result = new UnauthorizedObjectResult(new { message = "Session has been revoked" });
                return;
            }

            await next();
        }
    }
}