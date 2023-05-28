using Skuld.WebApi.Dto.Enum;

namespace Skuld.WebApi.Dto.Users
{
	public class UserResponse
	{
		public long UserId { get; set; }

		public string? Email { get; set; }

		public string? FirstName { get; set; }

		public string? LastName { get; set; }

		public Role Role { get; set; }
	}
}
