using LightwaveRfLinkPlus.Api.Models;
using Refit;
using System.Threading;
using System.Threading.Tasks;

namespace LightwaveRfLinkPlus.Api.Interfaces
{
	/// <summary>
	/// Devices interface
	/// </summary>
	public interface IDevices
	{
		/// <summary>
		/// Add a device
		/// </summary>
		/// <param name="device">The device to add</param>
		/// <param name="cancellationToken">The cancellationToken</param>
		[Post("/device/add")]
		public Task<Device> AddAsync(
			[Body] Device device,
			CancellationToken cancellationToken);

		/// <summary>
		/// Delete a device
		/// </summary>
		/// <param name="id">The device</param>
		/// <param name="cancellationToken">The cancellationToken</param>
		[Delete("/device/delete/{id}")]
		public Task DeleteAsync(
			string id,
			CancellationToken cancellationToken);
	}
}
