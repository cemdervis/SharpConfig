using System;

namespace Utilities.Configuration
{
	/// <summary>
	///		Defines a type-to-string and string-to-type converter that is used for the conversion of setting values.
	/// </summary>
	public interface ITypeStringConverter
	{
		/// <summary>
		///		Converts an object to its string representation.
		/// </summary>
		/// <param name="value"> The value to convert. </param>
		/// <returns> The object's string representation. </returns>
		string ConvertToString(object value);

		/// <summary>
		///		Converts a string value to an object of this converter's type.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="hint">
		///     A type hint. This is used rarely, such as in the enum converter.
		///     The enum converter's official type is Enum, whereas the type hint represents the underlying enum type.
		///     This parameter can be safely ignored for custom converters.
		/// </param>
		/// <returns> The converted object. </returns>
		object ConvertFromString(string value, Type hint);

		/// <summary>
		///		The type that this converter is able to convert to and from a string.
		/// </summary>
		Type ConvertibleType { get; }
	}

	/// <summary>
	///		Represents a type-to-string and string-to-type converter that is used for the conversion of setting values.
	/// </summary>
	/// <typeparam name="T">The type that this converter is able to convert.</typeparam>
	public abstract class TypeStringConverter<T> : ITypeStringConverter
	{
		/// <summary>
		///		Converts an object to its string representation.
		/// </summary>
		/// <param name="value"> The value to convert. </param>
		/// <returns> The object's string representation. </returns>
		public virtual string ConvertToString(object value)
		{
			return value.ToString();
		}

		/// <summary>
		///		Converts a string value to an object of this converter's type.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="hint">
		///     A type hint. This is used rarely, such as in the enum converter.
		///     The enum converter's official type is Enum, whereas the type hint represents the underlying enum type.
		///     This parameter can be safely ignored for custom converters.
		/// </param>
		/// <returns> The converted object. </returns>
		public abstract object ConvertFromString(string value, Type hint);

		/// <summary>
		///		The type that this converter is able to convert to and from a string.
		/// </summary>
		public Type ConvertibleType
		{
			get { return typeof(T); }
		}
	}

}
