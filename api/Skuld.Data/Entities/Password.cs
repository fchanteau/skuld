using System;

namespace Skuld.Data.Entities
{
	public class Password : IEntity
	{
		public long PasswordId { get; set; }
		public string Value { get; set; }
		public DateTime CreatedAt { get; set; }
		public bool IsActive { get; set; }

		public long UserId { get; set; }
		public User User { get; set; }
	}
}
