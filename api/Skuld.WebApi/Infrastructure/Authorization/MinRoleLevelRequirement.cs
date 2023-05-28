using Skuld.WebApi.Dto.Enum;

namespace Skuld.WebApi.Infrastructure.Authorization
{
	public class MinRoleLevelRequirement : RoleLevelRequirement
    {
        public MinRoleLevelRequirement(string claimType, int level) : base(claimType, level)
        {
        }

        public MinRoleLevelRequirement(string claimType, Role minLevel) : base(claimType, minLevel)
        {
        }
    }
}
