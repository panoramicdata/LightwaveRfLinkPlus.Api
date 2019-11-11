using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace LightwaveRfLinkPlus.Api.Test
{
	public class StructureTests : ApiTest
	{
		public StructureTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
		{
		}

		[Fact]
		public async Task GetStructuresSucceeds()
		{
			// Should not throw an exception
			var structureList = await Client
				.Structures
				.GetStructureListAsync(CancellationToken.None)
				.ConfigureAwait(false);

			Assert.NotEmpty(structureList.StructureIds);
			Assert.All(structureList.StructureIds, structureId => Assert.False(string.IsNullOrWhiteSpace(structureId)));

			foreach (var structureId in structureList.StructureIds)
			{
				var structure = await Client
					.Structures
					.GetAsync(structureId, CancellationToken.None)
					.ConfigureAwait(false);
				Assert.NotNull(structure);

				// Devices
				Assert.NotNull(structure.Devices);
				Assert.All(structure.Devices, device => Assert.NotNull(device));
				foreach (var device in structure.Devices)
				{
					Assert.False(string.IsNullOrWhiteSpace(device.Category));
					Assert.False(string.IsNullOrWhiteSpace(device.DeviceName));
					Assert.False(string.IsNullOrWhiteSpace(device.Description));
					Assert.False(string.IsNullOrWhiteSpace(device.Name));
					Assert.False(string.IsNullOrWhiteSpace(device.Product));
					Assert.False(string.IsNullOrWhiteSpace(device.ProductCode));
					Assert.False(string.IsNullOrWhiteSpace(device.Type));
					Assert.NotEqual(0, device.Generation);
					Assert.NotNull(device.Description);

					// FeatureSets
					Assert.NotNull(device.FeatureSets);
					Assert.All(device.FeatureSets, featureSet => Assert.NotNull(featureSet));
					foreach (var featureSet in device.FeatureSets)
					{
						Assert.False(string.IsNullOrWhiteSpace(featureSet.Id));
						Assert.False(string.IsNullOrWhiteSpace(featureSet.Name));
						Assert.NotNull(featureSet.Features);
						foreach (var feature in featureSet.Features)
						{
							Assert.False(string.IsNullOrWhiteSpace(feature.Id));
							Assert.False(string.IsNullOrWhiteSpace(feature.Type));
						}
					}
				}
			}
		}
	}
}
