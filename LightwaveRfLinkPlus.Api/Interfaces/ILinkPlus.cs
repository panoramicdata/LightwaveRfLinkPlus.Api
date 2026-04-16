using Refit;
using System.Threading.Tasks;

namespace LightwaveRfLinkPlus.Api.Interfaces
{
	/// <summary>
	/// Link Plus interface.
	/// </summary>
	public interface ILinkPlus
	{
		/// <summary>
		/// Adds a Link Plus instance.
		/// </summary>
		[Post("/linkplus/add")]
		public Task<string> AddAsync();
	}
}
