using System;

#nullable disable

namespace Skuld.Data.Entities
{
	public partial class RefreshToken : IEntity
	{
		public long RefreshTokenId { get; set; }
		public string Value { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime ExpiredAt { get; set; }

		public long UserId { get; set; }
		public User User { get; set; }
	}
}
