using System.ComponentModel.DataAnnotations;

namespace Skuld.WebApi.Features.Users.Models
{
    public class RefreshTokenPostModel
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
