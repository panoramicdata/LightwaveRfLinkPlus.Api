using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LightwaveRfLinkPlus.Api.Models
{
	/// <summary>
	/// A feature set
	/// </summary>
	[DataContract]
	public class FeatureSet
	{
		/// <summary>
		/// The feature id
		/// </summary>
		[DataMember(Name = "featureSetId")]
		public string Id { get; set; } = string.Empty;

		/// <summary>
		/// The type
		/// </summary>
		[DataMember(Name = "name")]
		public string Name { get; set; } = string.Empty;

		/// <summary>
		/// The features
		/// </summary>
		[DataMember(Name = "features")]
		public List<Feature> Features { get; set; } = new List<Feature>();
	}
}