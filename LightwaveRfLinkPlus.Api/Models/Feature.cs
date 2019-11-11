using System.Runtime.Serialization;

namespace LightwaveRfLinkPlus.Api.Models
{
	/// <summary>
	/// A feature set
	/// </summary>
	[DataContract]
	public class Feature
	{
		/// <summary>
		/// The feature id
		/// </summary>
		[DataMember(Name = "featureId")]
		public string Id { get; set; } = string.Empty;

		/// <summary>
		/// The type
		/// </summary>
		[DataMember(Name = "type")]
		public string Type { get; set; } = string.Empty;

		/// <summary>
		/// Whether it is writable
		/// </summary>
		[DataMember(Name = "writeable")]
		public bool Writeable { get; set; }
	}
}