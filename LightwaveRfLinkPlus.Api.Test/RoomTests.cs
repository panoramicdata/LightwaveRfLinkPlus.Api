using LightwaveRfLinkPlus.Api.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace LightwaveRfLinkPlus.Api.Test
{
	public class RoomTests : ApiTest
	{
		public RoomTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
		{
		}

		[Fact]
		public async Task CrudSucceeds()
		{
			await AssertExistingRoomsAreValidAsync().ConfigureAwait(false);

			var structureId = await GetFirstStructureIdAsync().ConfigureAwait(false);
			var roomCreationDto = new Room
			{
				Name = "TestRoomName",
				Type = "TestRoomType",
				ParentGroups = new List<string> { structureId },
				Order = new List<string> { structureId }
			};
			var createdRoom = await Client
				.Rooms
				.CreateAsync(roomCreationDto, CancellationToken.None)
				.ConfigureAwait(false);

			try
			{
				await AssertRoomUpdateCycleAsync(createdRoom.Id, roomCreationDto.Name).ConfigureAwait(false);
			}
			finally
			{
				await Client
					.Rooms
					.DeleteAsync(createdRoom.Id, CancellationToken.None)
					.ConfigureAwait(false);
			}
		}

		private async Task AssertExistingRoomsAreValidAsync()
		{
			var roomList = await Client
				.Rooms
				.GetAllAsync(CancellationToken.None)
				.ConfigureAwait(false);

			Assert.NotNull(roomList);
			Assert.NotNull(roomList.Rooms);
			Assert.All(roomList.Rooms, room =>
			{
				Assert.NotNull(room);
				Assert.False(string.IsNullOrWhiteSpace(room.Id));
				Assert.False(string.IsNullOrWhiteSpace(room.Name));
				Assert.False(string.IsNullOrWhiteSpace(room.Type));
				Assert.NotNull(room.Order);
				Assert.All(room.Order, order => Assert.False(string.IsNullOrWhiteSpace(order)));
				Assert.All(room.ParentGroups, parentGroup => Assert.False(string.IsNullOrWhiteSpace(parentGroup)));
				Assert.All(room.ScriptSets, Assert.NotNull);
			});
		}

		private async Task AssertRoomUpdateCycleAsync(string id, string expectedName)
		{
			var first = await Client.Rooms.GetAsync(id, CancellationToken.None).ConfigureAwait(false);
			Assert.Equal(expectedName, first.Name);
			Assert.Equal(id, first.Id);

			const string newName = "ZZXXX";
			first.Name = newName;
			var changed = await Client.Rooms.UpdateAsync(id, first, CancellationToken.None).ConfigureAwait(false);
			Assert.Equal(newName, changed.Name);
			Assert.Equal(id, changed.Id);

			var second = await Client.Rooms.GetAsync(id, CancellationToken.None).ConfigureAwait(false);
			Assert.Equal(newName, second.Name);
			Assert.Equal(id, second.Id);
		}
	}
}
