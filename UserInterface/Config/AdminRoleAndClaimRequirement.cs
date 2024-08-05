using Microsoft.AspNetCore.Authorization;
namespace UserInterface.Config;


public class AdminRoleAndClaimRequirement : IAuthorizationRequirement
{
    public string RequiredClaimType { get; }
    public string RequiredClaimValue { get; }

    public AdminRoleAndClaimRequirement(string requiredClaimType, string requiredClaimValue)
    {
        RequiredClaimType = requiredClaimType;
        RequiredClaimValue = requiredClaimValue;
    }
}
