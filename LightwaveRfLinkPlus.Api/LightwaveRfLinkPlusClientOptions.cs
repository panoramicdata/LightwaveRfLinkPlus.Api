using LightwaveRfLinkPlus.Api.Interfaces;
using Microsoft.Extensions.Logging;

namespace LightwaveRfLinkPlus.Api
{
	/// <summary>
	/// The options for setting up a LightwaveRfLinkPlusClient.
	/// </summary>
	public class LightwaveRfLinkPlusClientOptions
	{
		/// <summary>
		/// A mandatory credential repository
		/// Use this to get/set the refresh token
		/// </summary>
		public ICredentialsRepository? Repository { get; set; }

		/// <summary>
		/// An optional logger factory
		/// </summary>
		public ILoggerFactory? LoggerFactory { get; set; }
	}
}