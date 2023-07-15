using System;

namespace Skuld.WebApi.Infrastructure.Configuration.Options
{
	public class CORSOptions
	{
		public static string SectionName = "Skuld:CORS";

		public string[] AllowedUrls { get; set; } = Array.Empty<string> ();
	}
}
