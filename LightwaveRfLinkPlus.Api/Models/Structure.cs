using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LightwaveRfLinkPlus.Api.Models
{
	/// <summary>
	/// A structure
	/// </summary>
	[DataContract]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Low importance for sake of clarity.")]
	public class Structure
	{
		/// <summary>
		/// The name
		/// </summary>
		[DataMember(Name = "name")]
		public string Name { get; set; } = string.Empty;

		/// <summary>
		/// The group id
		/// </summary>
		[DataMember(Name = "groupId")]
		public string GroupId { get; set; } = string.Empty;

		/// <summary>
		/// The devices
		/// </summary>
		[DataMember(Name = "devices")]
		public List<Device> Devices { get; set; } = new List<Device>();
	}
}
