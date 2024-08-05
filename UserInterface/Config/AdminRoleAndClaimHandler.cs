using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using UserInterface.Config;

public class AdminRoleAndClaimHandler : AuthorizationHandler<AdminRoleAndClaimRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminRoleAndClaimRequirement requirement)
    {
        // Check if the user is in the "admin" role
        if (context.User.IsInRole("admin"))
        {
            // Check for the required read and write claims
            if (context.User.HasClaim(c => c.Type == "scope" && (c.Value == "all.read" || c.Value == "all.write")))
            {
                // Check for the organization claim
                if (context.User.HasClaim(c => c.Type == "organization_id"))
                {
                    context.Succeed(requirement); // The requirement is fulfilled
                }
            }
        }

        return Task.CompletedTask; // Return Task.CompletedTask to indicate completion
    }
}
