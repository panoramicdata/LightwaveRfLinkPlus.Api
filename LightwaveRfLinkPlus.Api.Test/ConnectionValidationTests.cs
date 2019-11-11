using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace LightwaveRfLinkPlus.Api.Test
{
	public class ConnectionValidationTests : ApiTest
	{
		public ConnectionValidationTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
		{
		}

		[Fact]
		public async Task ValidateConnectionSucceeds()
		{
			// Should not throw an exception
			await Client
				.ValidateConnectionAsync(CancellationToken.None)
				.ConfigureAwait(false);
		}
	}
}
