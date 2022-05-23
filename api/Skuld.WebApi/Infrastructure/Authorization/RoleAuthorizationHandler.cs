using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Skuld.WebApi.Infrastructure.Authorization
{
    public class RoleAuthorizationHandler : AuthorizationHandler<RoleLevelRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleLevelRequirement requirement)
        {
            // get claim value
            string claimValue = context.User.FindFirst(requirement.ClaimType)?.Value;
            int.TryParse(claimValue, out int permissionLevel);

            if (requirement is MinRoleLevelRequirement minRequirement)
            {
                this.CheckMinRole(context, minRequirement, permissionLevel);
            }
            else
            {
                this.CheckExactRole(context, requirement, permissionLevel);
            }

            return Task.CompletedTask;
        }

        private void CheckExactRole(AuthorizationHandlerContext context, RoleLevelRequirement requirement, int permissionLevel)
        {
            // Check if permission level is strictly equals to the minimum level
            if (permissionLevel == requirement.Level)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }

        private void CheckMinRole(AuthorizationHandlerContext context, MinRoleLevelRequirement minRequirement, int permissionLevel)
        {
            // Check if permission level is at least equals to the minimum level
            if (permissionLevel >= minRequirement.Level)
            {
                context.Succeed(minRequirement);
            }
            else
            {
                context.Fail();
            }
        }
    }
}
