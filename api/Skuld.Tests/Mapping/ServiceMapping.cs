using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skuld.Shared.Infrastructure.Configuration.Options;
using Skuld.Shared.Services;

namespace Skuld.Tests.Mapping
{
    [TestClass]
    public class ServiceMapping : BaseTest
    {
        [TestMethod]
        public void TestMappingUserService()
        {
            var service = new UserService(null, Options.Create(new JwtOptions()));

            AssertConfigIsValid(service.Mapper);
        }
    }
}
