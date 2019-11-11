using LightwaveRfLinkPlus.Api.Models;
using System.Threading;
using System.Threading.Tasks;

namespace LightwaveRfLinkPlus.Api.Interfaces
{
	/// <summary>
	/// A way to get and set the refresh token
	/// </summary>
	public interface ICredentialsRepository
	{
		/// <summary>
		/// Used to get the current value of the basic
		/// </summary>
		Task<string> GetBasicAsync(CancellationToken cancellationToken);

		/// <summary>
		/// Used to get the current value of the refresh token
		/// </summary>
		Task<string> GetInitialRefreshTokenAsync(CancellationToken cancellationToken);

		/// <summary>
		/// Set the new value of the refreh token
		/// </summary>
		Task SetAuthTokenAsync(AuthToken newValue, CancellationToken cancellationToken);

		/// <summary>
		/// Gets the auth token
		/// </summary>
		/// <param name="cancellationToken"></param>
		Task<AuthToken?> GetAuthTokenAsync(CancellationToken cancellationToken);
	}
}
