using System;
using System.ComponentModel;

namespace SharpConfig
{
  internal sealed class FallbackStringConverter : ITypeStringConverter
  {
    public string ConvertToString(object value)
    {
      try
      {
        var converter = TypeDescriptor.GetConverter(value);
        return converter.ConvertToString(null, Configuration.CultureInfo, value);
      }
      catch (Exception ex)
      {
        throw SettingValueCastException.Create(value.ToString(), value.GetType(), ex);
      }
    }

    public object ConvertFromString(string value, Type hint)
    {
      try
      {
        var converter = TypeDescriptor.GetConverter(hint);
        return converter.ConvertFrom(null, Configuration.CultureInfo, value);
      }
      catch (Exception ex)
      {
        throw SettingValueCastException.Create(value, hint, ex);
      }
    }

    public Type ConvertibleType => null;
  }

  internal sealed class BoolStringConverter : TypeStringConverter<bool>
  {
    public override string ConvertToString(object value)
    {
      return value.ToString();
    }

    public override object ConvertFromString(string value, Type hint)
    {
      switch (value.ToLowerInvariant())
      {
        case "false":
        case "off":
        case "no":
        case "n":
        case "0":
          return false;
        case "true":
        case "on":
        case "yes":
        case "y":
        case "1":
          return true;
      }

      throw SettingValueCastException.Create(value, hint, null);
    }
  }

  internal sealed class ByteStringConverter : TypeStringConverter<byte>
  {
    public override string ConvertToString(object value)
    {
      return value.ToString();
    }

    public override object ConvertFromString(string value, Type hint)
    {
      return byte.Parse(value, Configuration.CultureInfo.NumberFormat);
    }
  }

  internal sealed class CharStringConverter : TypeStringConverter<char>
  {
    public override string ConvertToString(object value)
    {
      return value.ToString();
    }

    public override object ConvertFromString(string value, Type hint)
    {
      return char.Parse(value);
    }
  }

  internal sealed class DateTimeStringConverter : TypeStringConverter<DateTime>
  {
    public override string ConvertToString(object value)
    {
      return ((DateTime)value).ToString(Configuration.CultureInfo.DateTimeFormat);
    }

    public override object ConvertFromString(string value, Type hint)
    {
      return DateTime.Parse(value, Configuration.CultureInfo.DateTimeFormat);
    }
  }

  internal sealed class DecimalStringConverter : TypeStringConverter<decimal>
  {
    public override string ConvertToString(object value)
    {
      return ((decimal)value).ToString(Configuration.CultureInfo.NumberFormat);
    }

    public override object ConvertFromString(string value, Type hint)
    {
      return decimal.Parse(value, Configuration.CultureInfo.NumberFormat);
    }
  }

  internal sealed class DoubleStringConverter : TypeStringConverter<double>
  {
    public override string ConvertToString(object value)
    {
      return ((double)value).ToString(Configuration.CultureInfo.NumberFormat);
    }

    public override object ConvertFromString(string value, Type hint)
    {
      return double.Parse(value, Configuration.CultureInfo.NumberFormat);
    }
  }

  internal sealed class EnumStringConverter : TypeStringConverter<Enum>
  {
    public override string ConvertToString(object value)
    {
      return value.ToString();
    }

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

      if (indexOfLastDot >= 0)
        value = value.Substring(indexOfLastDot + 1, value.Length - indexOfLastDot - 1).Trim();

      return Enum.Parse(hint, value);
    }
  }

  internal sealed class Int16StringConverter : TypeStringConverter<short>
  {
    public override string ConvertToString(object value)
    {
      return ((short)value).ToString(Configuration.CultureInfo.NumberFormat);
    }

    public override object ConvertFromString(string value, Type hint)
    {
      return short.Parse(value, Configuration.CultureInfo.NumberFormat);
    }
  }

  internal sealed class Int32StringConverter : TypeStringConverter<int>
  {
    public override string ConvertToString(object value)
    {
      return ((int)value).ToString(Configuration.CultureInfo.NumberFormat);
    }

    public override object ConvertFromString(string value, Type hint)
    {
      return int.Parse(value, Configuration.CultureInfo.NumberFormat);
    }
  }

  internal sealed class Int64StringConverter : TypeStringConverter<long>
  {
    public override string ConvertToString(object value)
    {
      return ((long)value).ToString(Configuration.CultureInfo.NumberFormat);
    }

    public override object ConvertFromString(string value, Type hint)
    {
      return long.Parse(value, Configuration.CultureInfo.NumberFormat);
    }
  }

  internal sealed class SByteStringConverter : TypeStringConverter<sbyte>
  {
    public override string ConvertToString(object value)
    {
      return ((sbyte)value).ToString(Configuration.CultureInfo.NumberFormat);
    }

    public override object ConvertFromString(string value, Type hint)
    {
      return sbyte.Parse(value, Configuration.CultureInfo.NumberFormat);
    }
  }

  internal sealed class SingleStringConverter : TypeStringConverter<float>
  {
    public override string ConvertToString(object value)
    {
      return ((float)value).ToString(Configuration.CultureInfo.NumberFormat);
    }

    public override object ConvertFromString(string value, Type hint)
    {
      return float.Parse(value, Configuration.CultureInfo.NumberFormat);
    }
  }

  internal sealed class StringStringConverter : TypeStringConverter<string>
  {
    public override string ConvertToString(object value)
    {
      return value.ToString().Trim('\"');
    }

    public override object ConvertFromString(string value, Type hint)
    {
      return value.Trim('\"');
    }
  }

  internal sealed class UInt16StringConverter : TypeStringConverter<ushort>
  {
    public override string ConvertToString(object value)
    {
      return ((ushort)value).ToString(Configuration.CultureInfo.NumberFormat);
    }

    public override object ConvertFromString(string value, Type hint)
    {
      return ushort.Parse(value, Configuration.CultureInfo.NumberFormat);
    }
  }

  internal sealed class UInt32StringConverter : TypeStringConverter<uint>
  {
    public override string ConvertToString(object value)
    {
      return ((uint)value).ToString(Configuration.CultureInfo.NumberFormat);
    }

    public override object ConvertFromString(string value, Type hint)
    {
      return uint.Parse(value, Configuration.CultureInfo.NumberFormat);
    }
  }

  internal sealed class UInt64StringConverter : TypeStringConverter<ulong>
  {
    public override string ConvertToString(object value)
    {
      return ((ulong)value).ToString(Configuration.CultureInfo.NumberFormat);
    }

    public override object ConvertFromString(string value, Type hint)
    {
      return ulong.Parse(value, Configuration.CultureInfo.NumberFormat);
    }
  }
}
