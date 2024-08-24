using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Skuld.WebApi.Common.Authorization
{
	public class RoleAuthorizationHandler : AuthorizationHandler<RoleLevelRequirement>
	{
		protected override Task HandleRequirementAsync (AuthorizationHandlerContext context, RoleLevelRequirement requirement)
		{
			// get claim value
			var claimValue = context.User.FindFirst (requirement.ClaimType)?.Value;
			// TODO FCU : better handle of TryParse here
			_ = int.TryParse (claimValue, out var permissionLevel);

			if (requirement is MinRoleLevelRequirement minRequirement)
			{
				CheckMinRole (context, minRequirement, permissionLevel);
			}
			else
			{
				CheckExactRole (context, requirement, permissionLevel);
			}

			return Task.CompletedTask;
		}

		private static void CheckExactRole (AuthorizationHandlerContext context, RoleLevelRequirement requirement, int permissionLevel)
		{
			// Check if permission level is strictly equals to the minimum level
			if (permissionLevel == requirement.Level)
			{
				context.Succeed (requirement);
			}
			else
			{
				context.Fail ();
			}
		}

		private static void CheckMinRole (AuthorizationHandlerContext context, MinRoleLevelRequirement minRequirement, int permissionLevel)
		{
			// Check if permission level is at least equals to the minimum level
			if (permissionLevel >= minRequirement.Level)
			{
				context.Succeed (minRequirement);
			}
			else
			{
				context.Fail ();
			}
		}
	}
}
