using System;

namespace Utilities.Configuration.Exceptions
{
	/// <summary>
	/// Represents an error that occurred during
	/// the configuration parsing stage.
	/// </summary>
	[Serializable]
	public sealed class ParserException : Exception
	{
		internal ParserException(string message, int line)
			: base($"Line {line}: {message}")
		{ }
	}
}