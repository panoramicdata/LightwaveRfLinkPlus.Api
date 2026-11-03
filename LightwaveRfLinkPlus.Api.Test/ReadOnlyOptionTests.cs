using LightwaveRfLinkPlus.Api.Interfaces;
using LightwaveRfLinkPlus.Api.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace LightwaveRfLinkPlus.Api.Test
{
	/// <summary>
	/// Credential-free tests for IsReadOnly. The stub repository throws a sentinel, so no request ever reaches the network.
	/// </summary>
	public class ReadOnlyOptionTests
	{
		public sealed class SentinelException : Exception
		{
		}

		private sealed class RecordingRepository : ICredentialsRepository
		{
			public int CallCount { get; private set; }

			public Task<string> GetBasicAsync(CancellationToken cancellationToken)
			{
				cancellationToken.ThrowIfCancellationRequested();
				throw new SentinelException();
			}

			public Task<string> GetInitialRefreshTokenAsync(CancellationToken cancellationToken)
			{
				cancellationToken.ThrowIfCancellationRequested();
				throw new SentinelException();
			}

			public Task SetAuthTokenAsync(AuthToken newValue, CancellationToken cancellationToken)
			{
				cancellationToken.ThrowIfCancellationRequested();
				throw new SentinelException();
			}

			public Task<AuthToken?> GetAuthTokenAsync(CancellationToken cancellationToken)
			{
				cancellationToken.ThrowIfCancellationRequested();
				CallCount++;
				throw new SentinelException();
			}
		}

		private static bool ChainContains<T>(Exception? exception) where T : Exception
		{
			for (var current = exception; current is not null; current = current.InnerException)
			{
				if (current is T)
				{
					return true;
				}
			}

			return false;
		}

		[Fact]
		public void IsReadOnly_DefaultsToFalse()
			=> Assert.False(new LightwaveRfLinkPlusClientOptions().IsReadOnly);

		[Fact]
		public async Task WriteCall_WhenReadOnly_ThrowsBeforeAnyCredentialOrNetworkWork()
		{
			var repository = new RecordingRepository();
			using var client = new LightwaveRfLinkPlusClient(new LightwaveRfLinkPlusClientOptions { Repository = repository, IsReadOnly = true });

			var exception = await Record.ExceptionAsync(() => client.Rooms.DeleteAsync("room-id", CancellationToken.None));

			Assert.True(ChainContains<InvalidOperationException>(exception) && !ChainContains<SentinelException>(exception));
			Assert.Equal(0, repository.CallCount);
		}

		[Fact]
		public async Task ReadCall_WhenReadOnly_StillReachesTheSendPath()
		{
			var repository = new RecordingRepository();
			using var client = new LightwaveRfLinkPlusClient(new LightwaveRfLinkPlusClientOptions { Repository = repository, IsReadOnly = true });

			var exception = await Record.ExceptionAsync(() => client.Hierarchy.GetAsync(CancellationToken.None));

			Assert.True(ChainContains<SentinelException>(exception));
			Assert.Equal(1, repository.CallCount);
		}

		[Fact]
		public async Task WriteCall_WhenNotReadOnly_IsNotBlocked()
		{
			var repository = new RecordingRepository();
			using var client = new LightwaveRfLinkPlusClient(new LightwaveRfLinkPlusClientOptions { Repository = repository });

			var exception = await Record.ExceptionAsync(() => client.Rooms.DeleteAsync("room-id", CancellationToken.None));

			Assert.True(ChainContains<SentinelException>(exception));
			Assert.Equal(1, repository.CallCount);
		}
	}
}
