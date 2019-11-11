using LightwaveRfLinkPlus.Api.Models;
using Refit;
using System.Threading;
using System.Threading.Tasks;

namespace LightwaveRfLinkPlus.Api.Interfaces
{
	/// <summary>
	/// Users interface
	/// </summary>
	public interface IUsers
	{
		/// <summary>
		/// Gets user info
		/// </summary>
		/// <param name="cancellationToken">The cancellationToken</param>
		[Get("/userinfo")]
		public Task<UserInfo> GetInfoAsync(
			CancellationToken cancellationToken);
	}
}
