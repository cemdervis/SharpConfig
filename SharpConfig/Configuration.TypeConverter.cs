using System;
using System.Collections.Generic;

namespace Utilities.Configuration
{
	public partial class Configuration
	{
		private static ITypeStringConverter                     mFallbackConverter      = new FallbackStringConverter();

		private static Dictionary<Type, ITypeStringConverter>   mTypeStringConverters   = new Dictionary<Type, ITypeStringConverter>()
																{
																	{ typeof(bool)      , new BoolStringConverter()     },
																	{ typeof(byte)      , new ByteStringConverter()     },
																	{ typeof(char)      , new CharStringConverter()     },
																	{ typeof(DateTime)  , new DateTimeStringConverter() },
																	{ typeof(decimal)   , new DecimalStringConverter()  },
																	{ typeof(double)    , new DoubleStringConverter()   },
																	{ typeof(Enum)      , new EnumStringConverter()     },
																	{ typeof(short)     , new Int16StringConverter()    },
																	{ typeof(int)       , new Int32StringConverter()    },
																	{ typeof(long)      , new Int64StringConverter()    },
																	{ typeof(sbyte)     , new SByteStringConverter()    },
																	{ typeof(float)     , new SingleStringConverter()   },
																	{ typeof(string)    , new StringStringConverter()   },
																	{ typeof(ushort)    , new UInt16StringConverter()   },
																	{ typeof(uint)      , new UInt32StringConverter()   },
																	{ typeof(ulong)     , new UInt64StringConverter()   }
																};
		
		/// <summary>
		///		Registers a type converter to be used for setting value conversions.
		/// </summary>
		/// <param name="converter"> The converter to register. </param>
		public static void RegisterTypeStringConverter(ITypeStringConverter converter)
		{
			if(converter == null)
				throw new ArgumentNullException("converter");

			var type = converter.ConvertibleType;

			if(mTypeStringConverters.ContainsKey(type))
				throw new InvalidOperationException(string.Format("A converter for type '{0}' is already registered.", type.FullName));
			else
				mTypeStringConverters.Add(type, converter);
		}

		/// <summary>
		///		Deregisters a type converter from setting value conversion.
		/// </summary>
		/// <param name="type"> The type whose associated converter to deregister. </param> 
		public static void DeregisterTypeStringConverter(Type type)
		{
			if(type == null)
				throw new ArgumentNullException("type");

			if(!mTypeStringConverters.ContainsKey(type))
				throw new InvalidOperationException(string.Format("No converter is registered for type '{0}'.", type.FullName));
			else
				mTypeStringConverters.Remove(type);
		}

		/// <summary>
		///		Find a proper type converter to be used for setting value conversion
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		internal static ITypeStringConverter FindTypeStringConverter(Type type)
		{
			ITypeStringConverter converter = null;

			// Can't find proper type converter when type is 'enum'
			if(type.IsEnum)
				type = typeof(Enum);

			if(!mTypeStringConverters.TryGetValue(type, out converter))
				converter = mFallbackConverter;

			return converter;
		}
		
		internal static ITypeStringConverter FallbackConverter
		{
			get { return mFallbackConverter; }
		}
	}
}
