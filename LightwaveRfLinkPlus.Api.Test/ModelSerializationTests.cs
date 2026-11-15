using LightwaveRfLinkPlus.Api.Models;
using Newtonsoft.Json;
using System.Linq;
using Xunit;

namespace LightwaveRfLinkPlus.Api.Test
{
	/// <summary>
	/// Offline tests that the wire names of the models are preserved.
	/// </summary>
	public class ModelSerializationTests
	{
		private const string GroupJson = """{"groupId":"g1","name":"N","type":"T","parentGroups":["p"],"visible":true,"order":["o1","o2"]}""";

		[Fact]
		public void Zone_MapsGroupFieldsAndRooms()
		{
			var zone = JsonConvert.DeserializeObject<Zone>(GroupJson.Replace("}", ",\"rooms\":[\"r1\"]}", System.StringComparison.Ordinal))!;

			Assert.Equal("g1", zone.Id);
			Assert.Equal("N", zone.Name);
			Assert.Equal("T", zone.Type);
			Assert.Equal(["p"], zone.ParentGroups);
			Assert.True(zone.Visible);
			Assert.Equal(["o1", "o2"], zone.Order);
			Assert.Equal(["r1"], zone.Rooms);
		}

		[Theory]
		[InlineData(typeof(Room))]
		[InlineData(typeof(Favourite))]
		public void SetsGroups_MapGroupFieldsAndSets(System.Type type)
		{
			var json = GroupJson.Replace("}", ",\"featureSets\":[\"f\"],\"scriptSets\":[1],\"automationSets\":[2,3]}", System.StringComparison.Ordinal);

			var group = (SetsGroupBase)JsonConvert.DeserializeObject(json, type)!;

			Assert.Equal("g1", group.Id);
			Assert.Equal(["f"], group.FeatureSets);
			Assert.Single(group.ScriptSets);
			Assert.Equal(2, group.AutomationSets.Count);
		}

		[Fact]
		public void Zone_SerialisesIdAsGroupId()
		{
			var json = JsonConvert.SerializeObject(new Zone { Id = "g9", Name = "N" });

			Assert.Contains("\"groupId\":\"g9\"", json, System.StringComparison.Ordinal);
			Assert.DoesNotContain("\"Id\"", json, System.StringComparison.Ordinal);
		}

		[Fact]
		public void Hierarchy_MapsZonesFavouritesAndRooms()
		{
			var json = $$"""{"zone":[{{GroupJson}}],"favourite":[{{GroupJson}}],"room":[{{GroupJson}}]}""";

			var hierarchy = JsonConvert.DeserializeObject<Hierarchy>(json)!;

			Assert.Equal(["g1"], hierarchy.Zones.Select(z => z.Id));
			Assert.Equal(["g1"], hierarchy.Favourites.Select(f => f.Id));
			Assert.Equal(["g1"], hierarchy.Rooms.Select(r => r.Id));
		}

		[Fact]
		public void AuthToken_MapsSnakeCaseFields()
		{
			const string json = """{"access_token":"a","token_type":"bearer","expires_in":3600,"refresh_token":"r","id_token":"i","session_state":"s"}""";

			var token = JsonConvert.DeserializeObject<AuthToken>(json)!;

			Assert.Equal("a", token.AccessToken);
			Assert.Equal("bearer", token.TokenType);
			Assert.Equal(3600, token.ExpiresInSeconds);
			Assert.Equal("r", token.RefreshToken);
			Assert.Equal("i", token.IdToken);
			Assert.Equal("s", token.SessionState);
		}

		[Fact]
		public void Options_DefaultToNothingConfigured()
		{
			var options = new LightwaveRfLinkPlusClientOptions();

			Assert.Null(options.Repository);
			Assert.Null(options.LoggerFactory);
			Assert.False(options.IsReadOnly);
		}

		[Fact]
		public void Client_RejectsNullOptions()
			=> Assert.Throws<System.ArgumentNullException>(() => new LightwaveRfLinkPlusClient(null!));
	}
}
