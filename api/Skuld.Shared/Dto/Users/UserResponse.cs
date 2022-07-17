using Skuld.Shared.Dto.Enum;

namespace Skuld.Shared.Dto.Users
{
	public class UserResponse
	{
		public long UserId { get; set; }

		public string Email { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public Role Role { get; set; }
	}
}
