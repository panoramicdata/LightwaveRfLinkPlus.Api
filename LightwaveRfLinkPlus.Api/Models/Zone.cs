using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LightwaveRfLinkPlus.Api.Models
{
	/// <summary>
	/// A zone
	/// </summary>
	public class Zone : GroupBase
	{
		/// <summary>
		/// The rooms
		/// </summary>
		[DataMember(Name = "rooms")]
		public List<string> Rooms { get; set; } = new List<string>();
	}
}
