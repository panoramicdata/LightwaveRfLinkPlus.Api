using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LightwaveRfLinkPlus.Api.Models
{
	/// <summary>
	/// A zone
	/// </summary>
	public class Zone
	{
		/// <summary>
		/// The group id
		/// </summary>
		[DataMember(Name = "groupId")]
		public string Id { get; set; } = string.Empty;

		/// <summary>
		/// The name
		/// </summary>
		[DataMember(Name = "name")]
		public string Name { get; set; } = string.Empty;

		/// <summary>
		/// The type
		/// </summary>
		[DataMember(Name = "type")]
		public string Type { get; set; } = string.Empty;

		/// <summary>
		/// The parent groups
		/// </summary>
		[DataMember(Name = "parentGroups")]
		public List<string> ParentGroups { get; set; } = new List<string>();

		/// <summary>
		/// Whether it is visible
		/// </summary>
		[DataMember(Name = "visible")]
		public bool Visible { get; set; }

		/// <summary>
		/// The order
		/// </summary>
		[DataMember(Name = "order")]
		public List<string> Order { get; set; } = new List<string>();

		/// <summary>
		/// The rooms
		/// </summary>
		[DataMember(Name = "rooms")]
		public List<string> Rooms { get; set; } = new List<string>();
	}
}
