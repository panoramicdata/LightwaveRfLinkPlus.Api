using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LightwaveRfLinkPlus.Api.Models
{
	/// <summary>
	/// A favourite
	/// </summary>
	[DataContract]
	public class Favourite
	{
		/// <summary>
		/// The groupId
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
		/// The parentGroups
		/// </summary>
		[DataMember(Name = "parentGroups")]
		public List<string> ParentGroups { get; set; } = new List<string>();

		/// <summary>
		/// The visible
		/// </summary>
		[DataMember(Name = "visible")]
		public bool Visible { get; set; }

		/// <summary>
		/// The order
		/// </summary>
		[DataMember(Name = "order")]
		public List<string> Order { get; set; } = new List<string>();

		/// <summary>
		/// The featureSets
		/// </summary>
		[DataMember(Name = "featureSets")]
		public List<string> FeatureSets { get; set; } = new List<string>();

		/// <summary>
		/// The scriptSets
		/// </summary>
		[DataMember(Name = "scriptSets")]
		public List<object> ScriptSets { get; set; } = new List<object>();

		/// <summary>
		/// The automationSets
		/// </summary>
		[DataMember(Name = "automationSets")]
		public List<object> AutomationSets { get; set; } = new List<object>();
	}
}
