using Skuld.WebApi.Features.Users.Dto;

namespace Skuld.WebApi.Common.Authorization
{
	public class MinRoleLevelRequirement : RoleLevelRequirement
	{
		public MinRoleLevelRequirement (string claimType, int level) : base (claimType, level)
		{
		}

		public MinRoleLevelRequirement (string claimType, Role minLevel) : base (claimType, minLevel)
		{
		}
	}
}
