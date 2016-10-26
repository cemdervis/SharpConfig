using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace Utilities.Configuration
{
	/// <summary>
	///		Represents a configuration.
	///		Configurations contain one or multiple sections that in turn can contain one or multiple settings.
	///		The <see cref="Configuration"/> class is designed to work with classic configuration formats such as .ini and .cfg, but is not limited to these.
	/// </summary>
	public partial class Configuration : IEnumerable<Section>
	{
		#region Fields

		private static NumberFormatInfo							mNumberFormat			= CultureInfo.InvariantCulture.NumberFormat;
		private static DateTimeFormatInfo						mDateTimeFormat			= CultureInfo.InvariantCulture.DateTimeFormat;

		private static char[]									mValidCommentChars		= new[] { '#', ';', '\'' };
		private static char										mArrayElementSeparator	= ',';

		private List<Section>									mSections;

		#endregion

		/// <summary>
		///		Initializes a new instance of the <see cref="Configuration"/> class.
		/// </summary>
		protected Configuration()
		{
			mSections = new List<Section>();
		}

		#region Properties

		/// <summary>
		///		Gets or sets the number format that is used for value conversion in Section.GetValue().
		///		The default value is <b>CultureInfo.InvariantCulture.NumberFormat</b>.
		/// </summary>
		/// <exception cref="ArgumentNullException"> When a null reference is set as the value. </exception> 
		public static NumberFormatInfo NumberFormat
		{
			get { return mNumberFormat; }
			set
			{
				if(value == null)
					throw new ArgumentNullException(nameof(value));

				mNumberFormat = value;
			}
		}

		/// <summary>
		///		Gets or sets the DateTime format that is used for value conversion in SharpConfig.
		///		The default value is CultureInfo.InvariantCulture.DateTimeFormat.
		/// </summary>
		/// <exception cref="ArgumentNullException"> When a null reference is set as the value. </exception> 
		public static DateTimeFormatInfo DateTimeFormat
		{
			get { return mDateTimeFormat; }
			set
			{
				if(value == null)
					throw new ArgumentNullException(nameof(value));

				mDateTimeFormat = value;
			}
		}

		/// <summary>
		///		Gets or sets the array that contains all comment delimiting characters.
		/// </summary>
		/// <exception cref="ArgumentNullException"> When a null reference is set as the value. </exception> 
		/// <exception cref="ArgumentException"> When an empty array is set as the value. </exception> 
		public static char[] ValidCommentChars
		{
			get { return mValidCommentChars; }
			set
			{
				if(value == null)
					throw new ArgumentNullException(nameof(value));

				if(value.Length == 0)
					throw new ArgumentException("The comment chars array must not be empty.", nameof(value));

				mValidCommentChars = value;
			}
		}

		/// <summary>
		///		Gets or sets the array element separator character for settings.
		///		The default value is ','.
		///		NOTE: remember that after you change this value while <see cref="Setting"/> instances exist, 
		///		to expect their ArraySize and other array-related values to return different values.
		/// </summary>
		/// <exception cref="ArgumentException"> When a zero-character ('\0') is set as the value. </exception>
		public static char ArrayElementSeparator
		{
			get { return mArrayElementSeparator; }
			set
			{
				if(value == '\0')
					throw new ArgumentException("Zero-character is not allowed.");

				mArrayElementSeparator = value;
			}
		}

		/// <summary>
		///		Gets or sets a value indicating whether inline-comments should be ignored when parsing a configuration.
		/// </summary>
		public static bool IgnoreInlineComments { get; set; } = false;

		/// <summary>
		///		Gets or sets a value indicating whether pre-comments should be ignored when parsing a configuration.
		/// </summary>
		public static bool IgnorePreComments { get; set; } = false;

		/// <summary>
		///		Gets the number of sections that are in the configuration.
		/// </summary>
		public int SectionCount
		{
			get { return mSections.Count; }
		}

		/// <summary>
		///		Gets or sets a section by index.
		/// </summary>
		/// <param name="index"> The index of the section in the configuration. </param>
		/// <returns>
		///		The section at the specified index.
		///		Note: no section is created when using this accessor.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException"> When the index is out of range. </exception> 
		public Section this[int index]
		{
			get
			{
				if(index < 0 || index >= mSections.Count)
					throw new ArgumentOutOfRangeException(nameof(index));

				return mSections[index];
			}
			set
			{
				if(index < 0 || index >= mSections.Count)
					throw new ArgumentOutOfRangeException(nameof(index));

				mSections[index] = value;
			}
		}

		/// <summary>
		///		Gets or sets a section by its name.
		///		If there are multiple sections with the same name, the first section is returned.
		///		If you want to obtain all sections that have the same name, use the GetSectionsNamed() method instead.
		/// </summary>
		///
		/// <param name="name"> The case-sensitive name of the section. </param>
		///
		/// <returns>
		///		The section if found, otherwise a new section with the specified name is created, added to the configuration and returned.
		/// </returns>
		public Section this[string name]
		{
			get
			{
				var section = FindSection(name);

				if(section == null)
				{
					section = new Section(name);
					Add(section);
				}

				return section;
			}
			set
			{
				if(string.IsNullOrEmpty(name))
					throw new ArgumentNullException(nameof(name), "The section name must not be null or empty.");

				if(value == null)
					throw new ArgumentNullException(nameof(value), "The specified value must not be null.");

				// Check if there already is a section by that name.
				var section = FindSection(name);
				int settingIndex = section != null ? mSections.IndexOf(section) : -1;

				if(settingIndex < 0)
				{
					// A section with that name does not exist yet; add it.
					mSections.Add(section);
				}
				else
				{
					// A section with that name exists; overwrite.
					mSections[settingIndex] = section;
				}
			}
		}

		#endregion

		#region IEnumerable

		/// <summary>
		///		Gets an enumerator that iterates through the configuration.
		/// </summary>
		public IEnumerator<Section> GetEnumerator()
		{
			return mSections.GetEnumerator();
		}

		/// <summary>
		///		Gets an enumerator that iterates through the configuration.
		/// </summary>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion IEnumerable

		#region Private methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		private Section FindSection(string name)
		{
			foreach(var section in mSections)
			{
				if(string.Equals(section.Name, name, StringComparison.OrdinalIgnoreCase))
					return section;
			}

			return null;
		}

		#endregion Private methods

		#region Public methods

		/// <summary>
		///		Adds a section to the configuration.
		/// </summary>
		/// <param name="section"> The section to add. </param>
		/// <exception cref="ArgumentNullException"> When <paramref name="section"/> is null. </exception> 
		/// <exception cref="ArgumentException"> When the section already exists in the configuration. </exception> 
		public void Add(Section section)
		{
			if (section == null)
				throw new ArgumentNullException(nameof(section));

			if (Contains(section))
				throw new ArgumentException("The specified section already exists in the configuration.", nameof(section));

			mSections.Add(section);
		}

		/// <summary>
		///		Removes a section from the configuration by its name.
		///		If there are multiple sections with the same name, only the first section is removed.
		///		To remove all sections that have the name name, use the RemoveAllNamed() method instead.
		/// </summary>
		/// <param name="sectionName"> The case-sensitive name of the section to remove. </param>
		/// <exception cref="ArgumentNullException"> When <paramref name="sectionName"/> is null or empty. </exception> 
		public void Remove(string sectionName)
		{
			if(string.IsNullOrEmpty(sectionName))
				throw new ArgumentNullException(nameof(sectionName));

			var section = FindSection(sectionName);

			if(section == null)
				throw new ArgumentException("The specified section does not exist in the section.", nameof(section));

			Remove(section);
		}

		/// <summary>
		///		Removes all sections that have a specific name.
		/// </summary>
		/// <param name="sectionName"> The case-sensitive name of the sections to remove. </param>
		/// <exception cref="ArgumentNullException"> When <paramref name="sectionName"/> is null or empty. </exception> 
		public void RemoveAllNamed(string sectionName)
		{
			if (string.IsNullOrEmpty(sectionName))
			{
				throw new ArgumentNullException("sectionName");
			}

			for (int i = mSections.Count - 1; i >= 0; i--)
			{
				if (string.Equals(mSections[i].Name, sectionName, StringComparison.OrdinalIgnoreCase))
				{
					mSections.RemoveAt(i);
				}
			}
		}

		/// <summary>
		///		Removes a section from the configuration.
		/// </summary>
		/// <param name="section"> The section to remove. </param>
		public void Remove(Section section)
		{
			if(section == null)
				throw new ArgumentNullException(nameof(section));

			if(!Contains(section))
				throw new ArgumentException("The specified section does not exist in the section.", nameof(section));

			mSections.Remove(section);
		}

		/// <summary>
		///		Clears the configuration of all sections.
		/// </summary>
		public void Clear()
		{
			mSections.Clear();
		}

		/// <summary>
		///		Determines whether a specified section is contained in the configuration.
		/// </summary>
		/// <param name="section"> The section to check for containment. </param>
		/// <returns> True if the section is contained in the configuration; false otherwise. </returns>
		public bool Contains(Section section)
		{
			return mSections.Contains(section);
		}

		/// <summary>
		///		Determines whether a specifically named setting is contained in the section.
		/// </summary>
		/// <param name="sectionName"> The name of the section. </param>
		/// <returns> True if the setting is contained in the section; false otherwise. </returns>
		public bool Contains(string sectionName)
		{
			return FindSection(sectionName) != null;
		}

		/// <summary>
		///		Gets all sections that have a specific name.
		/// </summary>
		/// <param name="name"> The case-sensitive name of the sections. </param>
		/// <returns>
		///		The found sections.
		/// </returns>
		public IEnumerable<Section> GetSectionsNamed(string name)
		{
			var sections = new List<Section>();

			foreach(var section in mSections)
			{
				if(string.Equals(section.Name, name, StringComparison.OrdinalIgnoreCase))
				{
					sections.Add(section);
				}
			}

			return sections;
		}

		#endregion Public methods
	}
}