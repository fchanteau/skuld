using System;
using System.Text;

namespace Skuld.WebApi.Helpers
{
	public interface IPasswordProvider
	{
		string GenerateCipherPassword (string? password);
	}

	public class PasswordProvider : IPasswordProvider
	{
		public string GenerateCipherPassword (string? password)
		{
			if (password is null)
			{
				throw new ArgumentNullException (nameof (password));
			}
			return Convert.ToBase64String (Encoding.ASCII.GetBytes (password));
		}
	}
}
