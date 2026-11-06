using Microsoft.Extensions.Logging;
using System;
using Xunit;

namespace LightwaveRfLinkPlus.Api.Test
{
	/// <summary>Writes log output to the xUnit test output.</summary>
	public sealed class XunitLoggerProvider(ITestOutputHelper testOutputHelper) : ILoggerProvider
	{
		public ILogger CreateLogger(string categoryName) => new XunitLogger(categoryName, testOutputHelper);

		public void Dispose()
		{
		}

		private sealed class XunitLogger(string categoryName, ITestOutputHelper testOutputHelper) : ILogger
		{
			public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

			public bool IsEnabled(LogLevel logLevel) => true;

			public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
			{
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
