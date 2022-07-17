using System.ComponentModel.DataAnnotations;

namespace Skuld.Shared.Dto.Users
{
	public class RefreshTokenPayload
	{
		[Required]
		public string RefreshToken { get; set; }
	}
}
