using Refit;
using System.Threading.Tasks;

namespace LightwaveRfLinkPlus.Api.Interfaces
{
	public interface ILinkPlus
	{
		[Post("/linkplus/add")]
		public Task<string> AddAsync();
	}
}
