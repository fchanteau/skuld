using System.ComponentModel.DataAnnotations;

namespace Skuld.WebApi.Features.Auth.Dto
{
	public class LoginPayload
	{
		[Required]
		[EmailAddress]
		public string? Email { get; set; }

		[Required]
		public string? Password { get; set; }
	}
}
