using LightwaveRfLinkPlus.Api.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LightwaveRfLinkPlus.Api
{
#pragma warning disable CA1303 // Do not pass literals as localized parameters
	internal class LightwaveRfLinkPlusClientHttpClientHandler : HttpClientHandler
	{
		private readonly LightwaveRfLinkPlusClientOptions _options;
		private readonly ILogger? _logger;
		private AuthToken? _authToken;

		public LightwaveRfLinkPlusClientHttpClientHandler(LightwaveRfLinkPlusClientOptions options)
		{
			_options = options;
			_logger = options.LoggerFactory?.CreateLogger<LightwaveRfLinkPlusClient>();
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			var guid = Guid.NewGuid();
			try
			{
				_logger?.LogDebug($"{guid} request starting {request.RequestUri}");

				// Ensure that we have a bearer token
				_authToken ??= await GetOrFetchAuthTokenAsync(cancellationToken).ConfigureAwait(false);

				// Add the request headers
				request.Headers.Add("Authorization", $"bearer {_authToken.AccessToken}");

				// Complete the action
				_logger?.LogDebug($"{guid} sending");
				var httpResponseMessage = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
				if (httpResponseMessage.IsSuccessStatusCode)
				{
					_logger?.LogDebug($"{guid} success");
				}
				else
				{
					_logger?.LogError($"{guid} failure: {await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false)}");
				}
				return httpResponseMessage;
			}
			finally
			{
				_logger?.LogDebug($"{guid} complete.");
			}
		}

		private async Task<AuthToken> GetOrFetchAuthTokenAsync(CancellationToken cancellationToken)
		{
			var optionsRepository = _options.Repository ?? throw new InvalidOperationException("Missing repository.");

			// try to get the AutoToken from the repository
			var authToken = await optionsRepository
				.GetAuthTokenAsync(cancellationToken)
				.ConfigureAwait(false);
			if (authToken != null)
			{
				return authToken;
			}

			// It's not set, get a new one...
			// Read the options
			var basic = await optionsRepository
				.GetBasicAsync(cancellationToken)
				.ConfigureAwait(false);

			// Ensure the auth parameters are set
			if (basic.Length == 0)
			{
				throw new InvalidOperationException("Options error: Basic not set");
			}

			var refreshToken = await optionsRepository
				.GetInitialRefreshTokenAsync(cancellationToken)
				.ConfigureAwait(false);
			if (refreshToken.Length == 0)
			{
				throw new InvalidOperationException("Options error: RefreshToken not set");
			}

			using var tokenRequestMessage = new HttpRequestMessage
			{
				Method = HttpMethod.Post,
				RequestUri = new Uri("https://auth.lightwaverf.com/token"),
				Content = new StringContent($"{{ \"grant_type\": \"refresh_token\", \"refresh_token\": \"{refreshToken}\" }}", Encoding.UTF8, "application/json")
			};
			tokenRequestMessage.Headers.Add("Authorization", $"basic {basic}");

			var tokenResponse = await base.SendAsync(tokenRequestMessage, cancellationToken).ConfigureAwait(false);
			string authResponse = await tokenResponse.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
			authToken = JsonConvert.DeserializeObject<AuthToken>(authResponse)
				?? throw new InvalidOperationException("Could not deserialize auth token response.");

			// Having fetched the AuthToken, it is important that we write this back out to the accesstoken repository
			await optionsRepository
				.SetAuthTokenAsync(authToken, cancellationToken)
				.ConfigureAwait(false);

			return authToken;
		}
	}
}