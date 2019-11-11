using LightwaveRfLinkPlus.Api.Models;
using Refit;
using System.Threading;
using System.Threading.Tasks;

namespace LightwaveRfLinkPlus.Api.Interfaces
{
	/// <summary>
	/// Hierarchy interface
	/// </summary>
	public interface IHierarchy
	{
		/// <summary>
		/// Gets user info
		/// </summary>
		/// <param name="cancellationToken">The cancellationToken</param>
		[Get("/hierarchy")]
		public Task<Hierarchy> GetAsync(
			CancellationToken cancellationToken);
	}
}
