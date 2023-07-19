using System.ComponentModel.DataAnnotations;

namespace Skuld.WebApi.Features.Auth.Dto
{
	public class AddUserPayload
	{
		[Required]
		[MaxLength (255)]
		[EmailAddress]
		public string? Email { get; set; }

		[Required]
		public string? Password { get; set; }

		[Required]
		[MaxLength (255)]
		public string? FirstName { get; set; }

		[Required]
		[MaxLength (255)]
		public string? LastName { get; set; }
	}
}
