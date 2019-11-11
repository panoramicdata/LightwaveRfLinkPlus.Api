using System.Runtime.Serialization;

namespace LightwaveRfLinkPlus.Api.Models
{
	/// <summary>
	/// A token response
	/// </summary>
	[DataContract]
	public class AuthToken
	{
		/// <summary>
		/// The access token
		/// </summary>
		[DataMember(Name = "access_token")]
		public string AccessToken { get; set; } = string.Empty;

		/// <summary>
		/// The token type
		/// </summary>
		[DataMember(Name = "token_type")]
		public string TokenType { get; set; } = string.Empty;

		/// <summary>
		/// The time until expiry in seconds
		/// </summary>
		[DataMember(Name = "expires_in")]
		public int ExpiresInSeconds { get; set; }

		/// <summary>
		/// The refresh token
		/// </summary>
		[DataMember(Name = "refresh_token")]
		public string RefreshToken { get; set; } = string.Empty;

		/// <summary>
		/// The id token
		/// </summary>
		[DataMember(Name = "id_token")]
		public string IdToken { get; set; } = string.Empty;

		/// <summary>
		/// The session state
		/// </summary>
		[DataMember(Name = "session_state")]
		public string SessionState { get; set; } = string.Empty;
	}
}
