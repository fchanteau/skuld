using System.Collections.Generic;

#nullable disable

namespace Skuld.Data.Entities
{
	public partial class User : IEntity
	{
		public User ()
		{
			RefreshTokens = new HashSet<RefreshToken> ();
			Passwords = new HashSet<Password> ();
		}

		public long UserId { get; set; }

		public string Email { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public int Role { get; set; }

		public ICollection<RefreshToken> RefreshTokens { get; set; }
		public ICollection<Password> Passwords { get; set; }
	}
}
