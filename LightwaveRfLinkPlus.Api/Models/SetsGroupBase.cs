using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LightwaveRfLinkPlus.Api.Models
{
	/// <summary>
	/// A group that also carries feature, script and automation sets
	/// </summary>
	[DataContract]
	public abstract class SetsGroupBase : GroupBase
	{
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
