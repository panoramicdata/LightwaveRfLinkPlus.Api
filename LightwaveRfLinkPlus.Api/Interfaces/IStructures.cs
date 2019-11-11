using LightwaveRfLinkPlus.Api.Models;
using Refit;
using System.Threading;
using System.Threading.Tasks;

namespace LightwaveRfLinkPlus.Api.Interfaces
{
	public interface IStructures
	{
		/// <summary>
		/// Get the structure list
		/// </summary>
		[Get("/structures")]
		public Task<StructureList> GetStructureListAsync(CancellationToken cancellationToken);

		/// <summary>
		/// Gets a specific strcuture
		/// </summary>
		/// <param name="id"></param>
		/// <param name="cancellationToken"></param>
		[Get("/structure/{id}")]
		public Task<Structure> GetAsync(string id, CancellationToken cancellationToken);
	}
}
