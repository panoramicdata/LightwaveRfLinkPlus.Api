using LightwaveRfLinkPlus.Api.Models;
using Refit;
using System.Threading;
using System.Threading.Tasks;

namespace LightwaveRfLinkPlus.Api.Interfaces
{
	/// <summary>
	/// Rooms interface
	/// </summary>
	public interface IRooms
	{
		/// <summary>
		/// Gets rooms
		/// </summary>
		/// <param name="cancellationToken">The cancellationToken</param>
		[Get("/rooms")]
		public Task<RoomList> GetAllAsync(
			CancellationToken cancellationToken);

		/// <summary>
		/// Gets a room by its id
		/// </summary>
		/// <param name="cancellationToken">The cancellationToken</param>
		[Get("/room/{id}")]
		public Task<Room> GetAsync(
			string id,
			CancellationToken cancellationToken);

		/// <summary>
		/// Creates a room
		/// </summary>
		/// <param name="cancellationToken">The cancellationToken</param>
		[Post("/room")]
		public Task<Room> CreateAsync(
			Room room,
			CancellationToken cancellationToken);

		/// <summary>
		/// Updates a room by its id
		/// </summary>
		/// <param name="cancellationToken">The cancellationToken</param>
		[Put("/room/{id}")]
		public Task<Room> UpdateAsync(
			string id,
			[Body] Room room,
			CancellationToken cancellationToken);

		/// <summary>
		/// Deletes a room by its id
		/// </summary>
		/// <param name="cancellationToken">The cancellationToken</param>
		[Delete("/room/{id}")]
		public Task DeleteAsync(
			string id,
			CancellationToken cancellationToken);
	}
}
