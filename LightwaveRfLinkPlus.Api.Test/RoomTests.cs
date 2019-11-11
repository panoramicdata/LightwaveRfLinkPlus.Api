using LightwaveRfLinkPlus.Api.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

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
			// Should not throw an exception
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

			// Get structure
			var structureList = await Client
				.Structures
				.GetStructureListAsync(CancellationToken.None)
				.ConfigureAwait(false);
			Assert.NotEmpty(structureList.StructureIds);

			// Create a new room
			var roomCreationDto = new Room
			{
				Name = "TestRoomName",
				Type = "TestRoomType",
				ParentGroups = new List<string>
				{
					structureList.StructureIds[0]
				},
				Order = new List<string>
				{
					structureList.StructureIds[0]
				}
			};
			var createdRoom = await Client
				.Rooms
				.CreateAsync(roomCreationDto, CancellationToken.None)
				.ConfigureAwait(false);

			try
			{
				// Fetch it
				var refetchedRoom1 = await Client
					.Rooms
					.GetAsync(createdRoom.Id, CancellationToken.None)
					.ConfigureAwait(false);
				Assert.Equal(roomCreationDto.Name, refetchedRoom1.Name);
				Assert.Equal(createdRoom.Id, refetchedRoom1.Id);

				// Update it
				const string newName = "ZZXXX";
				refetchedRoom1.Name = newName;
				var updatedRoom = await Client
					.Rooms
					.UpdateAsync(createdRoom.Id, refetchedRoom1, CancellationToken.None)
					.ConfigureAwait(false);
				Assert.Equal(newName, updatedRoom.Name);
				Assert.Equal(createdRoom.Id, updatedRoom.Id);

				// Fetch it again
				var refetchedRoom2 = await Client
					.Rooms
					.GetAsync(createdRoom.Id, CancellationToken.None)
					.ConfigureAwait(false);
				Assert.Equal(newName, refetchedRoom2.Name);
				Assert.Equal(createdRoom.Id, refetchedRoom2.Id);
			}
			finally
			{
				// Delete it
				await Client
					.Rooms
					.DeleteAsync(createdRoom.Id, CancellationToken.None)
					.ConfigureAwait(false);
			}
		}
	}
}
