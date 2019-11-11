using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LightwaveRfLinkPlus.Api.Models
{
	/// <summary>
	/// A zone list
	/// </summary>
	[DataContract]
	public class ZoneList
	{
		/// <summary>
		/// A list of zones
		/// </summary>
		[DataMember(Name = "zones")]
		public List<Zone> Zones { get; set; } = new List<Zone>();
	}
}
