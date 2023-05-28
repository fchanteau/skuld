using AutoMapper;

namespace Skuld.Tests
{
	public class BaseTest
	{
		protected void AssertConfigIsValid (IMapper mapper)
		{
			mapper.ConfigurationProvider.AssertConfigurationIsValid ();
		}
	}
}
