using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LightwaveRfLinkPlus.Api.Models
{
	/// <summary>
	/// A room list
	/// </summary>
	[DataContract]
	public class RoomList
	{
		/// <summary>
		/// A list of rooms
		/// </summary>
		[DataMember(Name = "rooms")]
		public List<Room> Rooms { get; set; } = new List<Room>();
	}
}
