using LightwaveRfLinkPlus.Api.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

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
			// Should not throw an exception
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

			// Get structure
			var structureList = await Client
				.Structures
				.GetStructureListAsync(CancellationToken.None)
				.ConfigureAwait(false);
			Assert.NotEmpty(structureList.StructureIds);

			// Create a new zone
			var zoneCreationDto = new Zone
			{
				Name = "TestZoneName",
				Type = "TestZoneType",
				ParentGroups = new List<string>
				{
					structureList.StructureIds[0]
				},
				Order = new List<string>
				{
					structureList.StructureIds[0]
				}
			};
			var createdZone = await Client
				.Zones
				.CreateAsync(zoneCreationDto, CancellationToken.None)
				.ConfigureAwait(false);

			try
			{
				// Fetch it
				var refetchedZone1 = await Client
					.Zones
					.GetAsync(createdZone.Id, CancellationToken.None)
					.ConfigureAwait(false);
				Assert.Equal(zoneCreationDto.Name, refetchedZone1.Name);
				Assert.Equal(createdZone.Id, refetchedZone1.Id);

				// Update it
				const string newName = "ZZXXX";
				refetchedZone1.Name = newName;
				var updatedZone = await Client
					.Zones
					.UpdateAsync(createdZone.Id, refetchedZone1, CancellationToken.None)
					.ConfigureAwait(false);
				Assert.Equal(newName, updatedZone.Name);
				Assert.Equal(createdZone.Id, updatedZone.Id);

				// Fetch it again
				var refetchedZone2 = await Client
					.Zones
					.GetAsync(createdZone.Id, CancellationToken.None)
					.ConfigureAwait(false);
				Assert.Equal(newName, refetchedZone2.Name);
				Assert.Equal(createdZone.Id, refetchedZone2.Id);
			}
			finally
			{
				// Delete it
				await Client
					.Zones
					.DeleteAsync(createdZone.Id, CancellationToken.None)
					.ConfigureAwait(false);
			}
		}
	}
}
