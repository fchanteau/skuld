﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Skuld.WebApi.Common.Authorization;
using Skuld.WebApi.Common.Constants;
using Skuld.WebApi.Features.Users.Dto;

namespace Skuld.WebApi.Common.Configuration
{
	public static class AuthorizationConfiguration
	{
		public static IServiceCollection AddCustomAuthorization (this IServiceCollection services)
		{
			services.AddAuthorization (ConfigureAuthorizationPolicies);

			services.AddSingleton<IAuthorizationHandler, RoleAuthorizationHandler> ();

			return services;
		}

		private static void ConfigureAuthorizationPolicies (AuthorizationOptions options)
		{
			var userRequirement = new MinRoleLevelRequirement (CustomClaimTypes.UserRole, Role.User);
			var adminRequirement = new MinRoleLevelRequirement (CustomClaimTypes.UserRole, Role.Admin);

			var authorizedUserPolicy = new AuthorizationPolicyBuilder ()
				.AddRequirements (userRequirement)
				.Build ();

			var administratorPolicy = new AuthorizationPolicyBuilder ()
				.AddRequirements (adminRequirement)
				.Build ();

			options.AddPolicy (CustomPolicies.AuthorizedUsersOnly, authorizedUserPolicy);
			options.AddPolicy (CustomPolicies.AdministratorOnly, administratorPolicy);
		}
	}
}
