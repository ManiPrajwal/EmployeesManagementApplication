using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FirstWebApp.Security
{
    public class CanEditOnlyOtherAdminRolesAndClaimsHandler : AuthorizationHandler<ManageAdminRolesandClaimsRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageAdminRolesandClaimsRequirement requirement)
        {
            var authFleContext = context.Resource as AuthorizationFilterContext;
            if (authFleContext == null)
            {
                return Task.CompletedTask;
            }
            string loggedInAdminId =
                context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            string adminIdBeingEdited = authFleContext.HttpContext.Request.Query["userId"];
            //var sample = context.User.IsInRole("Admin") /*|| context.User.IsInRole("Super Admin")*/;
            if (context.User.IsInRole("Admin") && 
                context.User.HasClaim(claim => claim.Type == "EditRole" && claim.Value == "true") &&
                adminIdBeingEdited.ToLower() != loggedInAdminId.ToLower())
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
