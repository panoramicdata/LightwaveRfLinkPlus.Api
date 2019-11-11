namespace LightwaveRfLinkPlus.Api.Models
{
	/// <summary>
	/// Credentials
	/// </summary>
	public class LightwaveRfLinkPlusCredentials
	{
		/// <summary>
		/// The Basic
		/// </summary>
		public string Basic { get; set; } = string.Empty;

		/// <summary>
		/// The initial refreshtoken.  Only used if AuthToken is null.
		/// </summary>
		public string InitialRefreshToken { get; set; } = string.Empty;

		/// <summary>
		/// The auth token
		/// Must be initialised
		/// </summary>
		public AuthToken? AuthToken { get; set; }
	}
}
