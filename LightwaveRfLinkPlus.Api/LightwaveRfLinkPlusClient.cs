using LightwaveRfLinkPlus.Api.Interfaces;
using Newtonsoft.Json;
using Refit;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LightwaveRfLinkPlus.Api
{
	public class LightwaveRfLinkPlusClient : IDisposable
	{
		private readonly LightwaveRfLinkPlusClientHttpClientHandler _httpClientHandler;
		private readonly HttpClient _httpClient;

		/// <summary>
		/// A client for the API documented at https://support.lightwaverf.com/hc/en-us/articles/360020665652
		/// </summary>
		/// <param name="options"></param>
		public LightwaveRfLinkPlusClient(LightwaveRfLinkPlusClientOptions options)
		{
			_httpClientHandler = new LightwaveRfLinkPlusClientHttpClientHandler(options ?? throw new ArgumentNullException(nameof(options)));
			_httpClient = new HttpClient(_httpClientHandler) { BaseAddress = new Uri("https://publicapi.lightwaverf.com/v1") };

			var refitSettings = new RefitSettings
			{
				ContentSerializer = new JsonContentSerializer(
				new JsonSerializerSettings
				{
					NullValueHandling = NullValueHandling.Ignore
				})
			};

			Devices = RestService.For<IDevices>(_httpClient, refitSettings);
			Hierarchy = RestService.For<IHierarchy>(_httpClient, refitSettings);
			LinkPlus = RestService.For<ILinkPlus>(_httpClient, refitSettings);
			Rooms = RestService.For<IRooms>(_httpClient, refitSettings);
			Structures = RestService.For<IStructures>(_httpClient, refitSettings);
			Users = RestService.For<IUsers>(_httpClient, refitSettings);
			Zones = RestService.For<IZones>(_httpClient, refitSettings);
		}

		/// <summary>
		/// Devices endpoints
		/// </summary>
		public IDevices Devices { get; }

		/// <summary>
		/// Hierarchy endpoints
		/// </summary>
		public IHierarchy Hierarchy { get; }

		/// <summary>
		/// Link plus endpoints
		/// </summary>
		public ILinkPlus LinkPlus { get; }

		/// <summary>
		/// Structure endpoints
		/// </summary>
		public IStructures Structures { get; }

		/// <summary>
		/// Room endpoints
		/// </summary>
		public IRooms Rooms { get; }

		/// <summary>
		/// User endpoints
		/// </summary>
		public IUsers Users { get; }

		/// <summary>
		/// Zone endpoints
		/// </summary>
		public IZones Zones { get; }

		/// <summary>
		/// Throws an exception if there is anything wrong with the connection
		/// </summary>
		public Task ValidateConnectionAsync(CancellationToken cancellationToken)
			=> Structures.GetStructureListAsync(cancellationToken);

		#region IDisposable Support
		private bool _disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposedValue)
			{
				if (disposing)
				{
					_httpClientHandler?.Dispose();
					_httpClient?.Dispose();
				}

				_disposedValue = true;
			}
		}

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion
	}
}
