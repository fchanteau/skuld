namespace Skuld.WebApi.Infrastructure.Configuration.Options
{
	public class JwtOptions
	{
		public static string SectionName = "Skuld:JWT";

		public string? Issuer { get; set; }
		public string? Audience { get; set; }
		public string? SecretKey { get; set; }
	}
}
