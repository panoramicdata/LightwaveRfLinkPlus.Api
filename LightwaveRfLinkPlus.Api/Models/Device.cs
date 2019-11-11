using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LightwaveRfLinkPlus.Api.Models
{
	/// <summary>
	/// A device
	/// </summary>
	[DataContract]
	public class Device
	{
		/// <summary>
		/// The device id
		/// </summary>
		[DataMember(Name = "deviceId")]
		public string Id { get; set; } = string.Empty;

		/// <summary>
		/// The name
		/// </summary>
		[DataMember(Name = "name")]
		public string Name { get; set; } = string.Empty;

		/// <summary>
		/// The product code
		/// </summary>
		[DataMember(Name = "productCode")]
		public string ProductCode { get; set; } = string.Empty;

		/// <summary>
		/// The feature sets
		/// </summary>
		[DataMember(Name = "featureSets")]
		public List<FeatureSet> FeatureSets { get; set; } = new List<FeatureSet>();

		/// <summary>
		/// The product
		/// </summary>
		[DataMember(Name = "product")]
		public string Product { get; set; } = string.Empty;

		/// <summary>
		/// The device name
		/// </summary>
		[DataMember(Name = "device")]
		public string DeviceName { get; set; } = string.Empty;

		/// <summary>
		/// The description
		/// </summary>
		[DataMember(Name = "desc")]
		public string Description { get; set; } = string.Empty;

		/// <summary>
		/// The type
		/// </summary>
		[DataMember(Name = "type")]
		public string Type { get; set; } = string.Empty;

		/// <summary>
		/// The category
		/// </summary>
		[DataMember(Name = "cat")]
		public string Category { get; set; } = string.Empty;

		/// <summary>
		/// The generation
		/// </summary>
		[DataMember(Name = "gen")]
		public int Generation { get; set; }
	}
}