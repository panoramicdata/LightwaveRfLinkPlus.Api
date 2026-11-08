using Microsoft.Extensions.Logging;
using System.IO;
using Xunit;

namespace LightwaveRfLinkPlus.Api.Test
{
	[Trait("Category", "Integration")]
	public class ApiTest
	{
		public LightwaveRfLinkPlusClient Client { get; }

		protected ApiTest(ITestOutputHelper testOutputHelper)
		{
			// Load config

			var options = new LightwaveRfLinkPlusClientOptions
			{
				Repository = new AppSettingsCredentialsRepository(new FileInfo("../../../appsettings.json")),
				LoggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(builder => builder.SetMinimumLevel(LogLevel.Debug).AddProvider(new XunitLoggerProvider(testOutputHelper)))
			};

			Client = new LightwaveRfLinkPlusClient(options);
		}
	}
}