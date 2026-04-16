using LightwaveRfLinkPlus.Api.Models;
using Refit;
using System.Threading;
using System.Threading.Tasks;

namespace LightwaveRfLinkPlus.Api.Interfaces
{
	/// <summary>
	/// Zones interface
	/// </summary>
	public interface IZones
	{
		/// <summary>
		/// Gets zones
		/// </summary>
		/// <param name="cancellationToken">The cancellationToken</param>
		[Get("/zones")]
		public Task<ZoneList> GetAllAsync(
			CancellationToken cancellationToken);

		/// <summary>
		/// Gets a zone by its id
		/// </summary>
		/// <param name="id">The zone id.</param>
		/// <param name="cancellationToken">The cancellationToken</param>
		[Get("/zone/{id}")]
		public Task<Zone> GetAsync(
			string id,
			CancellationToken cancellationToken);

		/// <summary>
		/// Creates a zone
		/// </summary>
		/// <param name="zone">The zone payload.</param>
		/// <param name="cancellationToken">The cancellationToken</param>
		[Post("/zone")]
		public Task<Zone> CreateAsync(
			Zone zone,
			CancellationToken cancellationToken);

		/// <summary>
		/// Updates a zone by its id
		/// </summary>
		/// <param name="id">The zone id.</param>
		/// <param name="zone">The zone payload.</param>
		/// <param name="cancellationToken">The cancellationToken</param>
		[Put("/zone/{id}")]
		public Task<Zone> UpdateAsync(
			string id,
			[Body] Zone zone,
			CancellationToken cancellationToken);

		/// <summary>
		/// Deletes a zone by its id
		/// </summary>
		/// <param name="id">The zone id.</param>
		/// <param name="cancellationToken">The cancellationToken</param>
		[Delete("/zone/{id}")]
		public Task DeleteAsync(
			string id,
			CancellationToken cancellationToken);
	}
}
