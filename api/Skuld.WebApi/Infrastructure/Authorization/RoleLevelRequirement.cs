using Microsoft.AspNetCore.Authorization;
using Skuld.WebApi.Features.Auth.Dto;

namespace Skuld.WebApi.Infrastructure.Authorization
{
	public class RoleLevelRequirement : IAuthorizationRequirement
	{
		public string ClaimType { get; set; }

		public int Level { get; set; }

		public RoleLevelRequirement (string claimType, int level)
		{
			ClaimType = claimType;
			Level = level;
		}

		public RoleLevelRequirement (string claimType, Role minimumLevel) : this (claimType, (int)minimumLevel)
		{ }
	}
}
