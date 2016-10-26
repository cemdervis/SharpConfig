using System;

namespace Utilities.Configuration.Exceptions
{
	/// <summary>
	///		Represents an error that occurs when a string value could not be converted to a specific instance.
	/// </summary>
	[Serializable]
	public sealed class SettingValueCastException : Exception
	{
		private SettingValueCastException(string message, Exception innerException)
			: base(message, innerException)
		{ }

		internal static SettingValueCastException Create(string stringValue, Type dstType, Exception innerException)
		{
			string msg = $"Failed to convert value '{stringValue}' to type {dstType.FullName}.";
			return new SettingValueCastException(msg, innerException);
		}

		internal static SettingValueCastException CreateBecauseConverterMissing(string stringValue, Type dstType)
		{
			string msg = $"Failed to convert value '{stringValue}' to type {dstType.FullName}; no converter for this type is registered.";
			var innerException = new NotImplementedException("No converter for this type is registered.");

			return new SettingValueCastException(msg, innerException);
		}
	}

}