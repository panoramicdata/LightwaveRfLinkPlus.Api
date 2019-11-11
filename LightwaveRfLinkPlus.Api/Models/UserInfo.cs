using System.Runtime.Serialization;

namespace LightwaveRfLinkPlus.Api.Models
{
	/// <summary>
	/// User info
	/// </summary>
	[DataContract]
	public class UserInfo
	{
		/// <summary>
		/// The user id
		/// </summary>
		[DataMember(Name = "userId")]
		public string Id { get; set; } = string.Empty;

		/// <summary>
		/// The user email address
		/// </summary>
		[DataMember(Name = "email")]
		public string Email { get; set; } = string.Empty;
	}
}
