using System.ComponentModel.DataAnnotations;

namespace Skuld.WebApi.Dto.Users
{
	public class AddUserPayload
	{
		[Required]
		[MaxLength (255)]
		[RegularExpression (@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")]
		public string Email { get; set; }

		[Required]
		public string Password { get; set; }

		[Required]
		[MaxLength (255)]
		public string FirstName { get; set; }

		[Required]
		[MaxLength (255)]
		public string LastName { get; set; }
	}
}
