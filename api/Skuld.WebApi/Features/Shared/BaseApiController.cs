using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Skuld.WebApi.Exceptions;
using Skuld.WebApi.Infrastructure.Constants;
using System.Linq;
using System.Threading.Tasks;

namespace Skuld.WebApi.Features.Shared
{
	public abstract class BaseApiController : ControllerBase
	{
		protected BaseApiController () : base ()
		{
		}

		protected long GetUserIdFromToken ()
		{
			var userIdClaim = User.Claims.FirstOrDefault (x => x.Type.Equals (CustomClaimTypes.UserId));

			// TODO FCU : Create a SkuldExceptionType for this below
			if (userIdClaim == null)
				throw new SkuldException (System.Net.HttpStatusCode.InternalServerError, SkuldExceptionType.RefreshTokenInvalid, "");

			return long.TryParse (userIdClaim.Value, out var id) ? id : 0;
		}

		protected Task<string?> GetAccessTokenAsync ()
		{
			return HttpContext.GetTokenAsync ("access_token");
		}
	}
}
