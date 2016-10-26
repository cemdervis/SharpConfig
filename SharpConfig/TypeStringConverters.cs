using System;

namespace Utilities.Configuration
{
	internal sealed class FallbackStringConverter : ITypeStringConverter
	{
		public string ConvertToString(object value)
		{
			return value.ToString();
		}

		public object ConvertFromString(string value, Type hint)
		{
			throw new NotImplementedException();
		}

		public Type ConvertibleType
		{
			get { return null; }
		}
	}

	internal sealed class BoolStringConverter : TypeStringConverter<bool>
	{
		public override object ConvertFromString(string value, Type hint)
		{
			switch(value.ToLowerInvariant())
			{
				case "false":
				case "off":
				case "no":
				case "0":
					return false;

				case "true":
				case "on":
				case "yes":
				case "1":
					return true;
			}

			throw new ArgumentException(string.Format("The value cannot be converted to type '{0}'.", hint.FullName), "value");
		}
	}

	internal sealed class ByteStringConverter : TypeStringConverter<byte>
	{
		public override object ConvertFromString(string value, Type hint)
		{
			return sbyte.Parse(value, Configuration.NumberFormat);
		}
	}

	internal sealed class CharStringConverter : TypeStringConverter<char>
	{
		public override object ConvertFromString(string value, Type hint)
		{
			return char.Parse(value);
		}
	}

	internal sealed class DateTimeStringConverter : TypeStringConverter<DateTime>
	{
		public override string ConvertToString(object value)
		{
			return ((DateTime)value).ToString(Configuration.DateTimeFormat);
		}

		public override object ConvertFromString(string value, Type hint)
		{
			return DateTime.Parse(value, Configuration.DateTimeFormat);
		}
	}

	internal sealed class DecimalStringConverter : TypeStringConverter<decimal>
	{
		public override string ConvertToString(object value)
		{
			return ((decimal)value).ToString(Configuration.NumberFormat);
		}

		public override object ConvertFromString(string value, Type hint)
		{
			return decimal.Parse(value, Configuration.NumberFormat);
		}
	}

	internal sealed class DoubleStringConverter : TypeStringConverter<double>
	{
		public override string ConvertToString(object value)
		{
			return ((double)value).ToString(Configuration.NumberFormat);
		}

		public override object ConvertFromString(string value, Type hint)
		{
			return double.Parse(value, Configuration.NumberFormat);
		}
	}

	internal sealed class EnumStringConverter : TypeStringConverter<Enum>
	{
		public override object ConvertFromString(string value, Type hint)
		{
			// It's possible that the value is something like:
			// UriFormat.Unescaped
			// We, and especially Enum.Parse do not want this format.
			// Instead, it wants the clean name like:
			// Unescaped
			//
			// Because of that, let's get rid of unwanted type names.
			int indexOfLastDot = value.LastIndexOf('.');

			if(indexOfLastDot >= 0)
				value = value.Substring(indexOfLastDot + 1, value.Length - indexOfLastDot - 1).Trim();

			return Enum.Parse(hint, value);
		}
	}

	internal sealed class Int16StringConverter : TypeStringConverter<short>
	{
		public override string ConvertToString(object value)
		{
			return ((short)value).ToString(Configuration.NumberFormat);
		}

		public override object ConvertFromString(string value, Type hint)
		{
			return short.Parse(value, Configuration.NumberFormat);
		}
	}

	internal sealed class Int32StringConverter : TypeStringConverter<int>
	{
		public override string ConvertToString(object value)
		{
			return ((int)value).ToString(Configuration.NumberFormat);
		}

		public override object ConvertFromString(string value, Type hint)
		{
			return int.Parse(value, Configuration.NumberFormat);
		}
	}

	internal sealed class Int64StringConverter : TypeStringConverter<long>
	{
		public override string ConvertToString(object value)
		{
			return ((long)value).ToString(Configuration.NumberFormat);
		}

		public override object ConvertFromString(string value, Type hint)
		{
			return long.Parse(value, Configuration.NumberFormat);
		}
	}

	internal sealed class SByteStringConverter : TypeStringConverter<sbyte>
	{
		public override string ConvertToString(object value)
		{
			return ((sbyte)value).ToString(Configuration.NumberFormat);
		}

		public override object ConvertFromString(string value, Type hint)
		{
			return sbyte.Parse(value, Configuration.NumberFormat);
		}
	}

	internal sealed class SingleStringConverter : TypeStringConverter<float>
	{
		public override string ConvertToString(object value)
		{
			return ((float)value).ToString(Configuration.NumberFormat);
		}

		public override object ConvertFromString(string value, Type hint)
		{
			return float.Parse(value, Configuration.NumberFormat);
		}
	}

	internal sealed class StringStringConverter : TypeStringConverter<string>
	{
		public override object ConvertFromString(string value, Type hint)
		{
			return value;
		}
	}

	internal sealed class UInt16StringConverter : TypeStringConverter<ushort>
	{
		public override string ConvertToString(object value)
		{
			return ((ushort)value).ToString(Configuration.NumberFormat);
		}

		public override object ConvertFromString(string value, Type hint)
		{
			return ushort.Parse(value, Configuration.NumberFormat);
		}
	}

	internal sealed class UInt32StringConverter : TypeStringConverter<uint>
	{
		public override string ConvertToString(object value)
		{
			return ((uint)value).ToString(Configuration.NumberFormat);
		}

		public override object ConvertFromString(string value, Type hint)
		{
			return uint.Parse(value, Configuration.NumberFormat);
		}
	}

	internal sealed class UInt64StringConverter : TypeStringConverter<ulong>
	{
		public override string ConvertToString(object value)
		{
			return ((ulong)value).ToString(Configuration.NumberFormat);
		}

		public override object ConvertFromString(string value, Type hint)
		{
			return ulong.Parse(value, Configuration.NumberFormat);
		}
	}

}
