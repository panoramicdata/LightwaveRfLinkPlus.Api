using Divergic.Logging.Xunit;
using System.IO;
using Xunit.Abstractions;

namespace LightwaveRfLinkPlus.Api.Test
{
	public class ApiTest
	{
		public LightwaveRfLinkPlusClient Client { get; }

		protected ApiTest(ITestOutputHelper testOutputHelper)
		{
			// Load config

			var options = new LightwaveRfLinkPlusClientOptions
			{
				Repository = new AppSettingsCredentialsRepository(new FileInfo("../../../appsettings.json")),
				LoggerFactory = LogFactory.Create(testOutputHelper)
			};

			Client = new LightwaveRfLinkPlusClient(options);
		}
	}
}