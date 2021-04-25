namespace Skuld.WebApi.Features.Users.Models
{
    public class TokenResponseModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }

        public TokenResponseModel(string token, string refreshToken)
        {
            Token = token;
            RefreshToken = refreshToken;
        }
    }
}
