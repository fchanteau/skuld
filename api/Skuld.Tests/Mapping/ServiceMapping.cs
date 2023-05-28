using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skuld.WebApi.Infrastructure.Configuration.Options;
using Skuld.WebApi.Services;

namespace Skuld.Tests.Mapping
{
	[TestClass]
	public class ServiceMapping : BaseTest
	{
		[TestMethod]
		public void TestMappingUserService ()
		{
			var service = new UserService (null, Options.Create (new JwtOptions ()), null);

			AssertConfigIsValid (service.Mapper);
		}
	}
}
