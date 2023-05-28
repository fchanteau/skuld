using System.ComponentModel.DataAnnotations;

namespace Skuld.WebApi.Dto.Users
{
	public class RefreshTokenPayload
	{
		[Required]
		public string? RefreshToken { get; set; }
	}
}
