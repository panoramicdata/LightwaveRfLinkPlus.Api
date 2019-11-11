using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LightwaveRfLinkPlus.Api.Models
{
	/// <summary>
	/// A structure list
	/// </summary>
	[DataContract]
	public class StructureList
	{
		/// <summary>
		/// A list of structure ids
		/// </summary>
		[DataMember(Name = "structures")]
		public List<string> StructureIds { get; set; } = new List<string>();
	}
}
