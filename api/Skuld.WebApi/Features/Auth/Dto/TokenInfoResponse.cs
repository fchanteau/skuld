namespace Skuld.WebApi.Features.Auth.Dto
{
	public class TokenInfoResponse
	{
		public string? Token { get; set; }
		public string? RefreshToken { get; set; }
	}
}
