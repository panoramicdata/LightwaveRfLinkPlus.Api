using Microsoft.Extensions.Logging;
using System;
using Xunit;

namespace LightwaveRfLinkPlus.Api.Test
{
	/// <summary>Writes log output to the xUnit test output.</summary>
	internal sealed class XunitLoggerProvider(ITestOutputHelper testOutputHelper) : ILoggerProvider
	{
		/// <inheritdoc />
		public ILogger CreateLogger(string categoryName) => new XunitLogger(categoryName, testOutputHelper);

		/// <inheritdoc />
		public void Dispose()
		{
			// Nothing to dispose: the output helper belongs to the test.
		}

		private sealed class XunitLogger(string categoryName, ITestOutputHelper testOutputHelper) : ILogger
		{
			/// <inheritdoc />
			public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

			/// <inheritdoc />
			public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

			/// <inheritdoc />
			public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
			{
				if (!IsEnabled(logLevel))
				{
					return;
				}

				try
				{
					testOutputHelper.WriteLine($"[{logLevel}] {categoryName}: {formatter(state, exception)}");
					if (exception is not null)
					{
						testOutputHelper.WriteLine(exception.ToString());
					}
				}
				catch (InvalidOperationException)
				{
					// Output is closed once the test has finished.
				}
			}
		}
	}
}
