using System.ComponentModel.DataAnnotations;

namespace Skuld.WebApi.Features.Auth.Dto
{
	public class RefreshTokenPayload
	{
		[Required]
		public string? RefreshToken { get; set; }
	}
}
