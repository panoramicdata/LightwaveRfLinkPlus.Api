using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace LightwaveRfLinkPlus.Api.Test
{
	public class HierarchyTests : ApiTest
	{
		public HierarchyTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
		{
		}

		[Fact]
		public async Task GetHierarchySucceeds()
		{
			// Should not throw an exception
			var hierarchy = await Client
				.Hierarchy
				.GetAsync(CancellationToken.None)
				.ConfigureAwait(false);

			Assert.NotNull(hierarchy);
			Assert.NotNull(hierarchy.Favourites);
			Assert.All(hierarchy.Favourites, favourite =>
			{
				Assert.NotNull(favourite.AutomationSets);
				Assert.All(favourite.AutomationSets, automationSet => Assert.NotNull(automationSet));

				Assert.NotNull(favourite.FeatureSets);
				Assert.All(favourite.FeatureSets, featureSet => Assert.NotNull(featureSet));

				Assert.False(string.IsNullOrWhiteSpace(favourite.Id));

				Assert.False(string.IsNullOrWhiteSpace(favourite.Name));

				Assert.NotNull(favourite.Order);
				Assert.All(favourite.Order, orderItem => Assert.False(string.IsNullOrWhiteSpace(orderItem)));

				Assert.NotNull(favourite.ParentGroups);
				Assert.All(favourite.ParentGroups, parentGroup => Assert.False(string.IsNullOrWhiteSpace(parentGroup)));

				Assert.NotNull(favourite.ScriptSets);
				Assert.All(favourite.ScriptSets, scriptSet => Assert.NotNull(scriptSet));

				Assert.False(string.IsNullOrWhiteSpace(favourite.Type));
			});
		}
	}
}
