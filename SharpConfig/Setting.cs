using System;
using Utilities.Configuration.Exceptions;

namespace Utilities.Configuration
{
	/// <summary>
	/// Represents a setting in a <see cref="Configuration"/>.
	/// Settings are always stored in a <see cref="Section"/>.
	/// </summary>
	public sealed class Setting : ConfigurationElement
	{
		#region Fields

		private string	mRawValue;
		private int		mCachedArraySize;
		private bool	mShouldCalculateArraySize;
		private char	mCachedArrayElementSeparator;

		#endregion

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="Setting"/> class.
		/// </summary>
		public Setting(string name) 
			: this(name, string.Empty)
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="Setting"/> class.
		/// </summary>
		///
		/// <param name="name"> The name of the setting.</param>
		/// <param name="value">The value of the setting.</param>
		public Setting(string name, object value) 
			: base(name)
		{
			mRawValue						= string.Empty;
			mCachedArraySize				= 0;
			mShouldCalculateArraySize		= false;
			mCachedArrayElementSeparator	= Configuration.ArrayElementSeparator;

			SetValue(value);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the raw string value of this setting.
		/// Note: this is a shortcut to GetValue.
		/// </summary>
		public object Value
		{
			get { return GetValue<object>(); }
		}

		/// <summary>
		/// Gets or sets the value of this setting as a bool.
		/// Note: this is a shortcut to GetValue and SetValue.
		/// </summary>
		public bool BoolValue
		{
			get { return GetValue<bool>(); }
			set { SetValue(value); }
		}

		/// <summary>
		/// Gets or sets the value of this setting as an short.
		/// Note: this is a shortcut to GetValue and SetValue.
		/// </summary>
		public byte ByteValue
		{
			get { return GetValue<byte>(); }
			set { SetValue(value); }
		}

		/// <summary>
		/// Gets or sets the value of this setting as an short.
		/// Note: this is a shortcut to GetValue and SetValue.
		/// </summary>
		public short ShortValue
		{
			get { return GetValue<short>(); }
			set { SetValue(value); }
		}

		/// <summary>
		/// Gets or sets the value of this setting as an int.
		/// Note: this is a shortcut to GetValue and SetValue.
		/// </summary>
		public int IntValue
		{
			get { return GetValue<int>(); }
			set { SetValue(value); }
		}

		/// <summary>
		/// Gets or sets the value of this setting as an long.
		/// Note: this is a shortcut to GetValue and SetValue.
		/// </summary>
		public long LongValue
		{
			get { return GetValue<long>(); }
			set { SetValue(value); }
		}

		/// <summary>
		/// Gets or sets the value of this setting as a float.
		/// Note: this is a shortcut to GetValue and SetValue.
		/// </summary>
		public float FloatValue
		{
			get { return GetValue<float>(); }
			set { SetValue(value); }
		}

		/// <summary>
		/// Gets or sets the value of this setting as a double.
		/// Note: this is a shortcut to GetValue and SetValue.
		/// </summary>
		public double DoubleValue
		{
			get { return GetValue<double>(); }
			set { SetValue(value); }
		}

		/// <summary>
		/// Gets or sets the raw string value of this setting.
		/// Note: this is a shortcut to GetValue and SetValue.
		/// </summary>
		public string StringValue
		{
			get { return GetValue<string>(); }
			set { SetValue(value); }
		}

		/// <summary>
		/// Gets or sets the value of this settings as a date.
		/// Note: this is a shortcut to GetValue and SetValue.
		/// </summary>
		public DateTime DateTimeValue
		{
			get { return GetValue<DateTime>(); }
			set { SetValue(value); }
		}

		/// <summary>
		/// Gets a value indicating whether this setting is an array.
		/// </summary>
		public bool IsArray
		{
			get { return ArraySize >= 0; }
		}

		/// <summary>
		///		Gets the size of the array that this setting represents.
		///		If this setting is not an array, -1 is returned.
		/// </summary>
		public int ArraySize
		{
			get
			{
				// If the user changed the array element separator during the lifetime
				// of this setting, we have to recalculate the array size.
				if(mCachedArrayElementSeparator != Configuration.ArrayElementSeparator)
				{
					mCachedArrayElementSeparator	= Configuration.ArrayElementSeparator;
					mShouldCalculateArraySize		= true;
				}


				if(mShouldCalculateArraySize)
				{
					mCachedArraySize			= CalculateArraySize();
					mShouldCalculateArraySize	= false;
				}

				return mCachedArraySize;
			}
		}

		/// <summary>
		/// Gets this setting's value as an array of a object.
		/// Note: this is a shortcut to GetValueArray.
		/// </summary>
		public object[] Array
		{
			get { return GetValueArray(typeof(object)); }
		}

		/// <summary>
		/// Gets this setting's value as an array of a bool.
		/// Note: this is a shortcut to GetValueArray and SetValue.
		/// </summary>
		public bool[] BoolArray
		{
			get { return GetValueArray<bool>(); }
			set { SetValue(value); }
		}

		/// <summary>
		/// Gets this setting's value as an array of a byte.
		/// Note: this is a shortcut to GetValueArray and SetValue.
		/// </summary>
		public byte[] ByteArray
		{
			get { return GetValueArray<byte>(); }
			set { SetValue(value); }
		}

		/// <summary>
		/// Gets this setting's value as an array of a short.
		/// Note: this is a shortcut to GetValueArray and SetValue.
		/// </summary>
		public short[] ShortArray
		{
			get { return GetValueArray<short>(); }
			set { SetValue(value); }
		}

		/// <summary>
		/// Gets this setting's value as an array of a int.
		/// Note: this is a shortcut to GetValueArray and SetValue.
		/// </summary>
		public int[] IntArray
		{
			get { return GetValueArray<int>(); }
			set { SetValue(value); }
		}

		/// <summary>
		/// Gets this setting's value as an array of a long.
		/// Note: this is a shortcut to GetValueArray and SetValue.
		/// </summary>
		public long[] LongArray
		{
			get { return GetValueArray<long>(); }
			set { SetValue(value); }
		}

		/// <summary>
		/// Gets this setting's value as an array of a float.
		/// Note: this is a shortcut to GetValueArray and SetValue.
		/// </summary>
		public float[] FloatArray
		{
			get { return GetValueArray<float>(); }
			set { SetValue(value); }
		}

		/// <summary>
		/// Gets this setting's value as an array of a double.
		/// Note: this is a shortcut to GetValueArray and SetValue.
		/// </summary>
		public double[] DoubleArray
		{
			get { return GetValueArray<double>(); }
			set { SetValue(value); }
		}

		/// <summary>
		/// Gets this setting's value as an array of a string.
		/// Note: this is a shortcut to GetValueArray and SetValue.
		/// </summary>
		public string[] StringArray
		{
			get { return GetValueArray<string>(); }
			set { SetValue(value); }
		}

		/// <summary>
		/// Gets this setting's value as an array of a DateTime.
		/// Note: this is a shortcut to GetValueArray and SetValue.
		/// </summary>
		public DateTime[] DateTimeArray
		{
			get { return GetValueArray<DateTime>(); }
			set { SetValue(value); }
		}

		#endregion

		#region GetValue
		
		/// <summary>
		/// Gets this setting's value as a raw type.
		/// </summary>
		///
		/// <typeparam name="T">The type of the object to retrieve.</typeparam>
		public object GetValue()
		{
			return Value;
		}

		/// <summary>
		/// Gets this setting's value as a specific type.
		/// </summary>
		///
		/// <typeparam name="T">The type of the object to retrieve.</typeparam>
		public T GetValue<T>()
		{
			var type = typeof(T);
			
			return (T)GetValue(type);
		}

		/// <summary>
		/// Gets this setting's value as a specific type.
		/// </summary>
		///
		/// <param name="type">The type of the object to retrieve.</param>
		public object GetValue(Type type)
		{
			if(type == null)
				throw new ArgumentNullException(nameof(type));

			if(type.IsArray)
				return GetValueArray(type.GetElementType());
			else
				return CreateObjectFromString(mRawValue, type);
		}

		/// <summary>
		///		Gets this setting's value as an array of a specific type.
		///		Note: this only works if the setting represents an array. If it is not, then null is returned.
		/// </summary>
		/// <typeparam name="T">
		///     The type of elements in the array. All values in the array are going to be converted to objects of this type.
		///     If the conversion of an element fails, an exception is thrown.
		/// </typeparam>
		/// <returns> The values of this setting as an array. </returns>
		public T[] GetValueArray<T>()
		{
			if(typeof(T).IsArray)
				throw new ArgumentException("Jagged arrays are not supported.");
			
			if(this.ArraySize < 0)
				return null;

			var values = new T[this.ArraySize];

			if(this.ArraySize > 0)
			{
				var enumerator = new SettingArrayEnumerator(mRawValue);
				while(enumerator.Next())
				{
					values[enumerator.Index] = (T)CreateObjectFromString(enumerator.Current, typeof(T));
				}
			}

			return values;
		}

		/// <summary>
		///		Gets this setting's value as an array of a specific type.
		///		Note: this only works if the setting represents an array. If it is not, then null is returned.
		/// </summary>
		/// <param name="elementType">
		///     The type of elements in the array. All values in the array are going to be converted to objects of this type.
		///     If the conversion of an element fails, an exception is thrown.
		/// </param>
		/// <returns> The values of this setting as an array. </returns>
		public object[] GetValueArray(Type elementType)
		{
			if(elementType.IsArray)
				throw new ArgumentException("Jagged arrays are not supported.");

			if(this.ArraySize < 0)
				return null;

			var values = new object[this.ArraySize];

			if(this.ArraySize > 0)
			{
				var enumerator = new SettingArrayEnumerator(mRawValue);
				while(enumerator.Next())
				{
					values[enumerator.Index] = CreateObjectFromString(enumerator.Current, elementType);
				}
			}

			return values;
		}

		/// <summary>
		/// Converts the value of a single element to a desired type.
		/// </summary>
		private static object CreateObjectFromString(string value, Type dstType)
		{
			var underlyingType = Nullable.GetUnderlyingType(dstType);
			if(underlyingType != null)
			{
				if(string.IsNullOrEmpty(value))
					// Returns Nullable<type>().
					return null;

				// Otherwise, continue with our conversion using
				// the underlying type of the nullable.
				dstType = underlyingType;
			}

			var converter = Configuration.FindTypeStringConverter(dstType);
			if (converter == Configuration.FallbackConverter)
			{
				// The fallback converter is not able to create arbitrary objects
				// from strings, so this means that there is no type converter
				// registered for dstType.
				throw SettingValueCastException.CreateBecauseConverterMissing(value, dstType);
			}

			try
			{
				return converter.ConvertFromString(value, dstType);
			}
			catch(Exception ex)
			{
				throw SettingValueCastException.Create(value, dstType, ex);
			}
		}

		#endregion

		#region SetValue

		/// <summary>
		///		Sets the value of this setting via an object.
		/// </summary>
		/// <param name="value"> The value to set. </param>
		public void SetValue<T>(T value)
		{
			if(value == null)
			{
				SetEmptyValue();
			}
			else
			{
				var converter	= Configuration.FindTypeStringConverter(typeof(T));
				mRawValue		= converter.ConvertToString(value);
				mShouldCalculateArraySize = true;
			}
		}
		
		/// <summary>
		///		Sets the value of this setting via an array.
		/// </summary>
		/// <param name="values"> The values to set. </param>
		public void SetValue<T>(T[] values)
		{
			if(typeof(T).IsArray)
				throw new ArgumentException("Jagged arrays are not supported.");

			if(values == null)
			{
				SetEmptyValue();
			}
			else
			{
				var strings = new string[values.Length];

				var converter = Configuration.FindTypeStringConverter(typeof(T));
				for(int i = 0; i < values.Length; ++i)
					strings[i] = converter.ConvertToString(values[i]);

				mRawValue					= $"{{{string.Join(", ", strings)}}}";
				mCachedArraySize			= values.Length;
				mShouldCalculateArraySize	= false;
			}
		}

		/// <summary>
		///		Sets the value of this setting via an object.
		/// </summary>
		/// <param name="value"> The value to set. </param>
		public void SetValue(object value)
		{
			if(value == null)
			{
				SetEmptyValue();
				return;
			}

			var type = value.GetType();
			if(type.IsArray)
			{
				if(type.GetElementType().IsArray)
					throw new ArgumentException("Jagged arrays are not supported.");

				var values	= value as Array;
				var strings = new string[values.Length];

				for(int i = 0; i < values.Length; ++i)
				{
					object elemValue	= values.GetValue(i);
					var converter		= Configuration.FindTypeStringConverter(elemValue.GetType());

					strings[i] = converter.ConvertToString(elemValue);
				}
				
				mRawValue					= $"{{{string.Join(", ", strings)}}}";
				mCachedArraySize			= values.Length;
				mShouldCalculateArraySize	= false;
			}
			else
			{
				var converter				= Configuration.FindTypeStringConverter(type);
				mRawValue					= converter.ConvertToString(value);
				mShouldCalculateArraySize	= true;
			}
		}

		/// <summary>
		///		Sets the value of this setting via an array.
		/// </summary>
		/// <param name="values"> The values to set. </param>
		public void SetValue(object[] values)
		{
			if(values == null)
			{
				SetEmptyValue();
			}
			else
			{
				if(values.GetType().GetElementType().IsArray)
					throw new ArgumentException("Jagged arrays are not supported.");

				var strings = new string[values.Length];

				for(int i = 0; i < values.Length; ++i)
				{
					var converter	= Configuration.FindTypeStringConverter(values[i].GetType());
					strings[i]		= converter.ConvertToString(values[i]);
				}
				
				mRawValue					= $"{{{string.Join(", ", strings)}}}";
				mCachedArraySize			= values.Length;
				mShouldCalculateArraySize	= false;
			}
		}
		
		private void SetEmptyValue()
		{
			mRawValue					= string.Empty;
			mCachedArraySize			= 0;
			mShouldCalculateArraySize	= false;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Gets the string representation of the setting, without its comments.
		/// </summary>
		public override string ToString()
		{
			return ToString(false);
		}

		/// <summary>
		/// Gets the string representation of the setting.
		/// </summary>
		///
		/// <param name="includeComment">Specify true to include the comments in the string; false otherwise.</param>
		public string ToString(bool includeComment)
		{
			if (includeComment)
			{
				bool hasPreComments = mPreComments != null && mPreComments.Count > 0;

				string[] preCommentStrings = hasPreComments ?
					mPreComments.ConvertAll(c => c.ToString()).ToArray() : null;

				if (Comment != null && hasPreComments)
					// Include inline comment and pre-comments.
					return $"{string.Join(Environment.NewLine, preCommentStrings)}{Environment.NewLine}{Name}={mRawValue} {Comment.ToString()}";
				else if (Comment != null)
					// Include only the inline comment.
					return $"{Name}={mRawValue} {Comment.ToString()}";
				else if (hasPreComments)
					// Include only the pre-comments.
					return $"{string.Join(Environment.NewLine, preCommentStrings)}{Environment.NewLine}{Name}={mRawValue}";
			}

			// In every other case, include just the assignment in the string.
			return $"{Name}={mRawValue}";
		}

		#endregion
		
		private int CalculateArraySize()
		{
			if(string.IsNullOrEmpty(mRawValue))
				return -1;

			int arrayStartIdx = mRawValue.IndexOf('{');
			int arrayEndIdx = mRawValue.LastIndexOf('}');

			if(arrayStartIdx < 0 || arrayEndIdx < 0)
				// Not an array.
				return -1;

			// There may only be spaces between the beginning
			// of the string and the first left bracket.
			for(int i = 0; i < arrayStartIdx; ++i)
			{
				if(mRawValue[i] != ' ')
					return -1;
			}

			// Also, there may only be spaces between the last
			// right brace and the end of the string.
			for(int i = arrayEndIdx + 1; i < mRawValue.Length; ++i)
			{
				if(mRawValue[i] != ' ')
					return -1;
			}

			int arraySize = 0;

			// Naive algorithm; assume the number of array element delimiters.
			for(int i = 0; i < mRawValue.Length; ++i)
			{
				if(mRawValue[i] == Configuration.ArrayElementSeparator)
					++arraySize;
			}

			if(arraySize == 0)
			{
				// There were no element separators in the array expression. 
				// That does not mean that there are no elements.
				// Check if there is at least something.
				// If so, that is the single element of the array.
				for(int i = arrayStartIdx + 1; i < arrayEndIdx; ++i)
				{
					if(mRawValue[i] != ' ')
					{
						++arraySize;
						break;
					}
				}
			}
			else if(arraySize > 0)
			{
				// If there were any element separators in the array expression,
				// we have to increment the array size, as we assumed
				// that the number of element separators equaled the number of elements + 1.
				++arraySize;
			}

			return arraySize;
		}
	}
}
