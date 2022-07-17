using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Skuld.Shared.Infrastructure.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AM = AutoMapper;

namespace Skuld.WebApi.Features.Shared
{
	public abstract class BaseApiController : ControllerBase
	{
		protected AM.IMapper _mapper;

		protected BaseApiController () : base ()
		{
		}

		protected bool Validate<T> (T obj, out ICollection<ValidationResult> results)
		{
			results = new List<ValidationResult> ();

			return Validator.TryValidateObject (obj, new ValidationContext (obj), results, true);
		}

		protected long GetUserIdFromToken ()
		{
			return long.TryParse (this.User.Claims.FirstOrDefault (x => x.Type.Equals (CustomClaimTypes.UserId)).Value, out long id) ? id : 0;
		}

		protected Task<string> GetAccessTokenAsync ()
		{
			return HttpContext.GetTokenAsync ("access_token");
		}
	}
}
