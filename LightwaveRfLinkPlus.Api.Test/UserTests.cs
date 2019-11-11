using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace LightwaveRfLinkPlus.Api.Test
{
	public class UserTests : ApiTest
	{
		public UserTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
		{
		}

		[Fact]
		public async Task GetUserInfoSucceeds()
		{
			// Should not throw an exception
			var userInfo = await Client
				.Users
				.GetInfoAsync(CancellationToken.None)
				.ConfigureAwait(false);

			Assert.NotNull(userInfo);
			Assert.False(string.IsNullOrWhiteSpace(userInfo.Id));
			Assert.False(string.IsNullOrWhiteSpace(userInfo.Email));
		}
	}
}
