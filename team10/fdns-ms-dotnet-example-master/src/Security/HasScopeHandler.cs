using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Foundation.Sdk.Security;

namespace Foundation.Example.WebUI.Security
{
    /// <summary>
    /// Class for handling scope requirements specific to the foundation services scoping authorization model
    /// </summary>
    public class HasScopeHandler : AuthorizationHandler<HasScopeRequirement>
    {
        private const string SCOPE = "scope";

        private string GetScopeFromRoute(Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext resource)
        {
            var scope = $"fdns.example";
            return scope;
        }

        /// <summary>
        /// Determine if the user's scope claim (if any) matches the URL and HTTP operation they are attempting to carry out
        /// </summary>
        /// <param name="context">Contains authorization information used by Microsoft.AspNetCore.Authorization.IAuthorizationHandler</param>
        /// <param name="requirement">Information about what the requirement for this HTTP operation are</param>
        /// <returns>Task</returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasScopeRequirement requirement)
        {
            // Let's see if the resource is an auth filter. If not, exit
            var resource = (context.Resource as Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext);
            if (resource == null)
            {
                return Task.CompletedTask;
            }

            var scope = $"{GetScopeFromRoute(resource)}.{requirement.Scope}";

            // Just a check to see if the user identity object has a scope claim. If not, something is wrong and exit
            if (!context.User.HasClaim(c => c.Type == SCOPE && c.Issuer == requirement.Issuer))
            {
                return Task.CompletedTask;
            }

            /* Let's figure out all the scopes the user has been authorized to. These came from the OAuth2 token and have been
             * parsed by the ASP.NET Core middleware. We just an array of strings for simplicity's sake.
             */
            var scopes = context.User.FindFirst(c => c.Type == SCOPE && c.Issuer == requirement.Issuer).Value.Split(' ');

            // Succeed if the scope array contains the required scope
            if (scopes.Any(s => s == scope))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}