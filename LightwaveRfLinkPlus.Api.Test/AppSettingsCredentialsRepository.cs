using LightwaveRfLinkPlus.Api.Interfaces;
using LightwaveRfLinkPlus.Api.Models;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace LightwaveRfLinkPlus.Api.Test
{
	internal class AppSettingsCredentialsRepository : ICredentialsRepository
	{
		private readonly FileInfo _fileInfo;

		public AppSettingsCredentialsRepository(FileInfo fileInfo)
		{
			_fileInfo = fileInfo;
		}

		public async Task<string> GetBasicAsync(CancellationToken cancellationToken)
		{
			return (await GetOptionsAsync(cancellationToken).ConfigureAwait(false)).Basic;
		}

		private async Task<LightwaveRfLinkPlusCredentials> GetOptionsAsync(CancellationToken cancellationToken)
		{
			var appsettingsAsJson = await File
				.ReadAllTextAsync(_fileInfo.FullName, cancellationToken)
				.ConfigureAwait(false);
			return JsonSerializer.Deserialize<LightwaveRfLinkPlusCredentials>(appsettingsAsJson);
		}

		public async Task<string> GetInitialRefreshTokenAsync(CancellationToken cancellationToken)
		{
			var lightwaveRfLinkPlusCredentials = await GetOptionsAsync(cancellationToken).ConfigureAwait(false);
			return lightwaveRfLinkPlusCredentials.AuthToken?.RefreshToken
				?? lightwaveRfLinkPlusCredentials.InitialRefreshToken;
		}

		public async Task SetAuthTokenAsync(AuthToken newValue, CancellationToken cancellationToken)
		{
			var options = await GetOptionsAsync(cancellationToken).ConfigureAwait(false);
			options.AuthToken = newValue;
			options.InitialRefreshToken = newValue.RefreshToken;
			var optionsAsJson = JsonSerializer.Serialize(options, new JsonSerializerOptions
			{
				WriteIndented = true
			});
			await File
				.WriteAllTextAsync(_fileInfo.FullName, optionsAsJson, cancellationToken)
				.ConfigureAwait(false);
		}

		public async Task<AuthToken?> GetAuthTokenAsync(CancellationToken cancellationToken)
		{
			return (await GetOptionsAsync(cancellationToken).ConfigureAwait(false)).AuthToken;
		}
	}
}
