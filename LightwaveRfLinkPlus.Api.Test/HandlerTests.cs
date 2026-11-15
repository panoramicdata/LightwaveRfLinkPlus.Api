using LightwaveRfLinkPlus.Api.Interfaces;
using LightwaveRfLinkPlus.Api.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace LightwaveRfLinkPlus.Api.Test
{
	/// <summary>
	/// Offline tests for the request handler, run against a loopback listener and an in-memory repository.
	/// </summary>
	public class HandlerTests
	{
		private sealed class MemoryRepository(AuthToken? token, string basic, string refresh) : ICredentialsRepository
		{
			public Task<string> GetBasicAsync(CancellationToken cancellationToken) => Task.FromResult(Checked(cancellationToken, basic));

			public Task<string> GetInitialRefreshTokenAsync(CancellationToken cancellationToken) => Task.FromResult(Checked(cancellationToken, refresh));

			public Task SetAuthTokenAsync(AuthToken newValue, CancellationToken cancellationToken)
				=> Task.FromResult(Checked(cancellationToken, newValue));

			public Task<AuthToken?> GetAuthTokenAsync(CancellationToken cancellationToken) => Task.FromResult(Checked(cancellationToken, token));

			private static T Checked<T>(CancellationToken cancellationToken, T value)
			{
				cancellationToken.ThrowIfCancellationRequested();
				return value;
			}
		}

		public class ListLogger : DispatchProxy
		{
			private List<string> _messages = [];

			public static ILogger Create(List<string> messages)
			{
				var logger = Create<ILogger, ListLogger>();
				((ListLogger)(object)logger)._messages = messages;
				return logger;
			}

			protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
			{
				switch (targetMethod?.Name)
				{
					case nameof(ILogger.IsEnabled):
						return true;
					case nameof(ILogger.Log):
						var text = ((Delegate)args![4]!).DynamicInvoke(args[2], args[3]);
						_messages.Add($"{args[0]}: {text}");
						return null;
					default:
						return null;
				}
			}
		}
		private sealed class ListLoggerFactory(List<string> messages) : ILoggerFactory
		{
			public ILogger CreateLogger(string categoryName)
			{
				messages.Add($"category {categoryName}");
				return ListLogger.Create(messages);
			}

			public void AddProvider(ILoggerProvider provider) => ArgumentNullException.ThrowIfNull(provider);

			public void Dispose() => messages.Add("disposed");
		}
		private sealed class LoopbackServer : IDisposable
		{
			private readonly HttpListener _listener = new();
			private readonly Task _serving;

			public LoopbackServer(int status, string body)
			{
				var probe = new TcpListener(IPAddress.Loopback, 0);
				probe.Start();
				var port = ((IPEndPoint)probe.LocalEndpoint).Port;
				probe.Stop();

				BaseUri = new Uri($"http://127.0.0.1:{port}/");
				_listener.Prefixes.Add(BaseUri.AbsoluteUri);
				_listener.Start();
				_serving = Task.Run(async () =>
				{
					var context = await _listener.GetContextAsync();
					Authorization = context.Request.Headers["Authorization"];
					context.Response.StatusCode = status;
					await context.Response.OutputStream.WriteAsync(Encoding.UTF8.GetBytes(body));
					context.Response.Close();
				});
			}

			public Uri BaseUri { get; }

			public string? Authorization { get; private set; }

			public Task WaitAsync() => _serving;

			public void Dispose() => _listener.Close();
		}

		private static HttpClient CreateClient(LightwaveRfLinkPlusClientOptions options)
			=> new(new LightwaveRfLinkPlusClientHttpClientHandler(options));

		private static LightwaveRfLinkPlusClientOptions OptionsWithToken(List<string> messages, bool isReadOnly = false)
			=> new()
			{
				Repository = new MemoryRepository(new AuthToken { AccessToken = "abc" }, "b", "r"),
				LoggerFactory = new ListLoggerFactory(messages),
				IsReadOnly = isReadOnly
			};

		[Fact]
		public async Task Send_WithStoredToken_AddsBearerHeaderAndLogsSuccess()
		{
			var messages = new List<string>();
			using var server = new LoopbackServer(200, "ok");
			using var client = CreateClient(OptionsWithToken(messages));

			using var response = await client.GetAsync(server.BaseUri, TestContext.Current.CancellationToken);
			await server.WaitAsync();

			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.Equal("bearer abc", server.Authorization);
			Assert.Contains(messages, m => m.EndsWith("success", StringComparison.Ordinal));
			Assert.Contains(messages, m => m.EndsWith("complete.", StringComparison.Ordinal));
		}

		[Fact]
		public async Task Send_WhenServerFails_LogsTheResponseBodyAsError()
		{
			var messages = new List<string>();
			using var server = new LoopbackServer(500, "kaput");
			using var client = CreateClient(OptionsWithToken(messages));

			using var response = await client.GetAsync(server.BaseUri, TestContext.Current.CancellationToken);
			await server.WaitAsync();

			Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
			Assert.Contains(messages, m => m.StartsWith("Error:", StringComparison.Ordinal) && m.EndsWith("failure: kaput", StringComparison.Ordinal));
		}

		[Theory]
		[InlineData("POST")]
		[InlineData("PUT")]
		[InlineData("DELETE")]
		public async Task Send_WhenReadOnly_RejectsWrites(string method)
		{
			using var client = CreateClient(OptionsWithToken([], true));
			using var request = new HttpRequestMessage(new HttpMethod(method), "http://127.0.0.1:1/");

			var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => client.SendAsync(request, TestContext.Current.CancellationToken));

			Assert.Contains(method, exception.Message, StringComparison.Ordinal);
		}

		[Fact]
		public async Task Send_WithoutRepository_Throws()
		{
			using var client = CreateClient(new LightwaveRfLinkPlusClientOptions());

			var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => client.GetAsync("http://127.0.0.1:1/", TestContext.Current.CancellationToken));

			Assert.Equal("Missing repository.", exception.Message);
		}

		[Theory]
		[InlineData("", "refresh", "Options error: Basic not set")]
		[InlineData("basic", "", "Options error: RefreshToken not set")]
		public async Task Send_WithIncompleteCredentials_ThrowsBeforeAnyNetworkCall(string basic, string refresh, string expected)
		{
			using var client = CreateClient(new LightwaveRfLinkPlusClientOptions { Repository = new MemoryRepository(null, basic, refresh) });

			var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => client.GetAsync("http://127.0.0.1:1/", TestContext.Current.CancellationToken));

			Assert.Equal(expected, exception.Message);
		}
	}
}
