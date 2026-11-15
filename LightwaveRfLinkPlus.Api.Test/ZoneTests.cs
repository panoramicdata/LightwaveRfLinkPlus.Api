using LightwaveRfLinkPlus.Api.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace LightwaveRfLinkPlus.Api.Test
{
	public class ZoneTests : ApiTest
	{
		public ZoneTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
		{
		}

		[Fact]
		public async Task CrudSucceeds()
		{
			await AssertExistingZonesAreValidAsync().ConfigureAwait(false);

			var structureId = await GetFirstStructureIdAsync().ConfigureAwait(false);
			var zoneCreationDto = new Zone
			{
				Name = "TestZoneName",
				Type = "TestZoneType",
				ParentGroups = new List<string> { structureId },
				Order = new List<string> { structureId }
			};
			var createdZone = await Client
				.Zones
				.CreateAsync(zoneCreationDto, CancellationToken.None)
				.ConfigureAwait(false);

			try
			{
				await AssertFetchUpdateRefetchAsync(createdZone.Id, zoneCreationDto.Name).ConfigureAwait(false);
			}
			finally
			{
				await Client
					.Zones
					.DeleteAsync(createdZone.Id, CancellationToken.None)
					.ConfigureAwait(false);
			}
		}

		private async Task AssertExistingZonesAreValidAsync()
		{
			var zoneList = await Client
				.Zones
				.GetAllAsync(CancellationToken.None)
				.ConfigureAwait(false);

			Assert.NotNull(zoneList);
			Assert.NotNull(zoneList.Zones);
			Assert.All(zoneList.Zones, zone =>
			{
				Assert.NotNull(zone);
				Assert.False(string.IsNullOrWhiteSpace(zone.Id));
				Assert.False(string.IsNullOrWhiteSpace(zone.Name));
				Assert.False(string.IsNullOrWhiteSpace(zone.Type));
				Assert.NotNull(zone.Order);
				Assert.All(zone.Order, order => Assert.False(string.IsNullOrWhiteSpace(order)));
				Assert.All(zone.ParentGroups, parentGroup => Assert.False(string.IsNullOrWhiteSpace(parentGroup)));
				Assert.All(zone.Rooms, room => Assert.False(string.IsNullOrWhiteSpace(room)));
			});
		}

		private async Task AssertFetchUpdateRefetchAsync(string id, string expectedName)
		{
			var fetched = await Client.Zones.GetAsync(id, CancellationToken.None).ConfigureAwait(false);
			Assert.Equal(expectedName, fetched.Name);
			Assert.Equal(id, fetched.Id);

			const string newName = "ZZXXX";
			fetched.Name = newName;
			var updated = await Client.Zones.UpdateAsync(id, fetched, CancellationToken.None).ConfigureAwait(false);
			Assert.Equal(newName, updated.Name);
			Assert.Equal(id, updated.Id);

			var refetched = await Client.Zones.GetAsync(id, CancellationToken.None).ConfigureAwait(false);
			Assert.Equal(newName, refetched.Name);
			Assert.Equal(id, refetched.Id);
		}
	}
}
