using System.Security.Claims;

namespace Nexly.Api.MiddleWares
{
    public class MaintenanceModeMiddleware
    {
        private readonly RequestDelegate _next;

        public MaintenanceModeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IApplicationState applicationState)
        {
            var requestPath = context.Request.Path.Value;

            var authEndpoints = new List<string> { "/api/account/login", "/api/account/currentUser" };

            if (authEndpoints.Any(endpoint => requestPath.StartsWith(endpoint, StringComparison.OrdinalIgnoreCase)) ||
                (applicationState.IsMaintenanceModeEnabled && IsAdminUser(context)))
            {
                await _next(context);
            }
            else if (applicationState.IsMaintenanceModeEnabled)
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                await context.Response.WriteAsync("The site is currently down for maintenance.");
            }
            else
            {
                await _next(context);
            }
        }

        private bool IsAdminUser(HttpContext context)
        {
            var claimsIdentity = context.User.Identity as ClaimsIdentity;
            bool isAdmin = false;

            if (claimsIdentity != null)
            {
                //foreach(var claim in claimsIdentity.Claims)
                //{
                //    Console.WriteLine($"Type: {claim.Type} - value: {claim.Value}");
                //}

                isAdmin = claimsIdentity.Claims.Any(claim => claim.Type == ClaimTypes.Role && claim.Value.Contains("Admin"));
            }

            //Console.WriteLine($"Is Admin: {isAdmin}");

            //return context.User.IsInRole("Admin");
            return isAdmin;
        }
    }

    public interface IApplicationState
    {
        bool IsMaintenanceModeEnabled { get; set; }
    }

    public class ApplicationState : IApplicationState
    {
        public bool IsMaintenanceModeEnabled { get; set; }
    }
}
