using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LightwaveRfLinkPlus.Api.Models
{
	/// <summary>
	/// A hierarchy
	/// </summary>
	[DataContract]
	public class Hierarchy
	{
		/// <summary>
		/// The zones
		/// </summary>
		[DataMember(Name = "zone")]
		public List<Zone> Zones { get; set; } = new List<Zone>();

		/// <summary>
		/// The favourites
		/// </summary>
		[DataMember(Name = "favourite")]
		public List<Favourite> Favourites { get; set; } = new List<Favourite>();

		/// <summary>
		/// The rooms
		/// </summary>
		[DataMember(Name = "room")]
		public List<Room> Rooms { get; set; } = new List<Room>();
	}
}
