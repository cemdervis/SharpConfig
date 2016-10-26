using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Utilities.Configuration.Attributes;

namespace Utilities.Configuration
{
	/// <summary>
	/// Represents a group of <see cref="Setting"/> objects.
	/// </summary>
	public sealed class Section : ConfigurationElement, IEnumerable<Setting>
	{
		private List<Setting> mSettings;

		/// <summary>
		/// Initializes a new instance of the <see cref="Section"/> class.
		/// </summary>
		///
		/// <param name="name">The name of the section.</param>
		public Section(string name)
			: base(name)
		{
			mSettings = new List<Setting>();
		}

// 		/// <summary>
// 		///		Creates a new instance of the <see cref="Section"/> class that is based on an existing object.
// 		///		Important: the section is built only from the public getter properties and fields of its type.
// 		///		When this method is called, all of those properties will be called and fields accessed once to obtain their values.
// 		///		Properties and fields that are marked with the <see cref="IgnoreAttribute"/> attribute or are of a type that is marked with that attribute, are ignored.
// 		/// </summary>
// 		/// <param name="name"> The name of the section. </param>
// 		/// <param name="obj"></param>
// 		/// <returns> The newly created section. </returns>
// 		public static Section FromObject<T>(string name, T obj)
// 		{
// 			if (string.IsNullOrEmpty(name))
// 				throw new ArgumentException("The section name must not be null or empty.", nameof(name));
// 
// 			if (obj == null)
// 				throw new ArgumentNullException(nameof(obj), $"{nameof(obj)} must not be null.");
// 
// 			var section = new Section(name);
// 			var type	= typeof(T);
// 
// 			foreach (var prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
// 			{
// 				if (!prop.CanRead || ShouldIgnoreMappingFor(prop))
// 					// Skip this property, as it can't be read from.
// 					continue;
// 
// 				var setting = new Setting(prop.Name, prop.GetValue(obj, null));
// 				section.mSettings.Add(setting);
// 			}
// 
// 			// Repeat for each public field.
// 			foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public))
// 			{
// 				if (ShouldIgnoreMappingFor(field))
// 					// Skip this field.
// 					continue;
// 				
// 				var setting = new Setting(field.Name, field.GetValue(obj));
// 				section.mSettings.Add(setting);
// 			}
// 
// 			return section;
// 		}

		/// <summary>
		///		Creates a new instance of the <see cref="Section"/> class that is based on an existing object.
		///		Important: the section is built only from the public getter properties and fields of its type.
		///		When this method is called, all of those properties will be called and fields accessed once to obtain their values.
		///		Properties and fields that are marked with the <see cref="IgnoreAttribute"/> attribute or are of a type that is marked with that attribute, are ignored.
		/// </summary>
		/// <param name="name"> The name of the section. </param>
		/// <param name="obj"></param>
		/// <returns> The newly created section. </returns>
		public static Section FromObject(string name, object obj)
		{
			if(string.IsNullOrEmpty(name))
				throw new ArgumentException("The section name must not be null or empty.", "name");

			if(obj == null)
				throw new ArgumentNullException("obj", "obj must not be null.");

			var section = new Section(name);
			var type	= obj.GetType();

			foreach(var prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
			{
				if(!prop.CanRead || ShouldIgnoreMappingFor(prop))
					// Skip this property, as it can't be read from.
					continue;

				var setting = new Setting(prop.Name, prop.GetValue(obj, null));
				section.mSettings.Add(setting);
			}

			// Repeat for each public field.
			foreach(var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public))
			{
				if(ShouldIgnoreMappingFor(field))
					// Skip this field.
					continue;

				var setting = new Setting(field.Name, field.GetValue(obj));
				section.mSettings.Add(setting);
			}

			return section;
		}

		/// <summary>
		///		Creates an object of a specific type, and maps the settings in this section to the public properties and writable fields of the object.
		///		Properties and fields that are marked with the <see cref="IgnoreAttribute"/> attribute or are of a type that is marked with that attribute, are ignored.
		/// </summary>
		/// <returns>The created object.</returns>
		/// <remarks>
		///		The specified type must have a public default constructor in order to be created.
		/// </remarks>
		public T ToObject<T>() where T : new()
		{
			T obj = Activator.CreateInstance<T>();
			SetValuesTo(obj);

			return obj;
		}

		/// <summary>
		///		Creates an object of a specific type, and maps the settings in this section to the public properties and writable fields of the object.
		///		Properties and fields that are marked with the <see cref="IgnoreAttribute"/> attribute or are of a type that is marked with that attribute, are ignored.
		/// </summary>
		/// <param name="type"> 
		///		The type of object to create. 
		///		Note: the type must be default-constructible, meaning it has a public default constructor. 
		/// </param> 
		/// <returns> The created object. </returns>
		/// <remarks>
		///		The specified type must have a public default constructor in order to be created.
		/// </remarks>
		public object ToObject(Type type)
		{
			if(type == null)
				throw new ArgumentNullException(nameof(type));

			var obj = Activator.CreateInstance(type);
			SetValuesTo(obj);

			return obj;
		}

// 		/// <summary>
// 		///		Assigns the values of an object's public properties and fields to the corresponding <b>already existing</b> settings in this section.
// 		///		Properties and fields that are marked with the <see cref="IgnoreAttribute"/> attribute or are of a type that is marked with that attribute, are ignored.
// 		/// </summary>
// 		/// <param name="obj"> The object from which the values are obtained. </param>
// 		public void GetValuesFrom<T>(T obj)
// 		{
// 			if(obj == null)
// 				throw new ArgumentNullException("obj");
// 
// 			var type = typeof(T);
// 
// 			// Scan the type's properties.
// 			foreach(var prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
// 			{
// 				if(!prop.CanRead || ShouldIgnoreMappingFor(prop))
// 					continue;
// 
// 				var setting = FindSetting(prop.Name);
// 				if(setting != null)
// 				{
// 					object value = prop.GetValue(obj, null);
// 					setting.SetValue(value);
// 				}
// 			}
// 
// 			// Scan the type's fields.
// 			foreach(var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public))
// 			{
// 				// Skip readonly fields.
// 				if(ShouldIgnoreMappingFor(field))
// 					continue;
// 
// 				var setting = FindSetting(field.Name);
// 				if(setting != null)
// 				{
// 					object value = field.GetValue(obj);
// 					setting.SetValue(value);
// 				}
// 			}
// 		}

		/// <summary>
		///		Assigns the values of an object's public properties and fields to the corresponding <b>already existing</b> settings in this section.
		///		Properties and fields that are marked with the <see cref="IgnoreAttribute"/> attribute or are of a type that is marked with that attribute, are ignored.
		/// </summary>
		/// <param name="obj"> The object from which the values are obtained. </param>
		public void GetValuesFrom(object obj)
		{
			if(obj == null)
				throw new ArgumentNullException("obj");

			var type = obj.GetType();

			// Scan the type's properties.
			foreach(var prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
			{
				if(!prop.CanRead || ShouldIgnoreMappingFor(prop))
					continue;

				var setting = FindSetting(prop.Name);
				if(setting != null)
				{
					object value = prop.GetValue(obj, null);
					setting.SetValue(value);
				}
			}

			// Scan the type's fields.
			foreach(var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public))
			{
				// Skip readonly fields.
				if(ShouldIgnoreMappingFor(field))
					continue;

				var setting = FindSetting(field.Name);
				if(setting != null)
				{
					object value = field.GetValue(obj);
					setting.SetValue(value);
				}
			}
		}
		
// 		/// <summary>
// 		///		Assigns the values of this section to an object's public properties and fields.
// 		///		Properties and fields that are marked with the <see cref="IgnoreAttribute"/> attribute or are of a type that is marked with that attribute, are ignored.
// 		/// </summary>
// 		/// <param name="obj"> The object that is modified based on the section. </param>
// 		public void SetValuesTo<T>(T obj)
// 		{
// 			if(obj == null)
// 				throw new ArgumentNullException("obj");
// 
// 			var type = typeof(T);
// 
// 			// Scan the type's properties.
// 			foreach(var prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
// 			{
// 				if(!prop.CanWrite || ShouldIgnoreMappingFor(prop))
// 					continue;
// 
// 				var setting = FindSetting(prop.Name);
// 				if(setting == null)
// 					continue;
// 
// 				object value = setting.GetValue(prop.PropertyType);
// 				if(value is Array)
// 				{
// 					var settingArray = value as Array;
// 					var propArray = prop.GetValue(obj, null) as Array;
// 					if(propArray == null || propArray.Length != settingArray.Length)
// 					{
// 						// (Re)create the property's array.
// 						propArray = Array.CreateInstance(prop.PropertyType.GetElementType(), settingArray.Length);
// 					}
// 
// 					for(int i = 0; i < settingArray.Length; i++)
// 						propArray.SetValue(settingArray.GetValue(i), i);
// 
// 					prop.SetValue(obj, propArray, null);
// 				}
// 				else
// 				{
// 					prop.SetValue(obj, value, null);
// 				}
// 			}
// 
// 			// Scan the type's fields.
// 			foreach(var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public))
// 			{
// 				// Skip readonly fields.
// 				if(field.IsInitOnly || ShouldIgnoreMappingFor(field))
// 					continue;
// 
// 				var setting = FindSetting(field.Name);
// 				if(setting == null)
// 					continue;
// 
// 				object value = setting.GetValue(field.FieldType);
// 				if(value is Array)
// 				{
// 					var settingArray = value as Array;
// 					var fieldArray = field.GetValue(obj) as Array;
// 					if(fieldArray == null || fieldArray.Length != settingArray.Length)
// 					{
// 						// (Re)create the field's array.
// 						fieldArray = Array.CreateInstance(field.FieldType.GetElementType(), settingArray.Length);
// 					}
// 
// 					for(int i = 0; i < settingArray.Length; i++)
// 						fieldArray.SetValue(settingArray.GetValue(i), i);
// 
// 					field.SetValue(obj, fieldArray);
// 				}
// 				else
// 				{
// 					field.SetValue(obj, value);
// 				}
// 			}
// 		}

		/// <summary>
		/// Assigns the values of this section to an object's public properties and fields.
		/// Properties and fields that are marked with the <see cref="IgnoreAttribute"/> attribute or are of a type that is marked with that attribute, are ignored.
		/// </summary>
		/// <param name="obj"> The object that is modified based on the section. </param>
		public void SetValuesTo(object obj)
		{
			if(obj == null)
				throw new ArgumentNullException("obj");

			var type = obj.GetType();

			// Scan the type's properties.
			foreach(var prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
			{
				if(!prop.CanWrite || ShouldIgnoreMappingFor(prop))
					continue;

				var setting = FindSetting(prop.Name);
				if(setting == null)
					continue;

				object value = setting.GetValue(prop.PropertyType);
				if(value is Array)
				{
					var settingArray = value as Array;
					var propArray = prop.GetValue(obj, null) as Array;
					if(propArray == null || propArray.Length != settingArray.Length)
					{
						// (Re)create the property's array.
						propArray = Array.CreateInstance(prop.PropertyType.GetElementType(), settingArray.Length);
					}

					for(int i = 0; i < settingArray.Length; i++)
						propArray.SetValue(settingArray.GetValue(i), i);

					prop.SetValue(obj, propArray, null);
				}
				else
				{
					prop.SetValue(obj, value, null);
				}
			}

			// Scan the type's fields.
			foreach(var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public))
			{
				// Skip readonly fields.
				if(field.IsInitOnly || ShouldIgnoreMappingFor(field))
					continue;

				var setting = FindSetting(field.Name);
				if(setting == null)
					continue;

				object value = setting.GetValue(field.FieldType);
				if(value is Array)
				{
					var settingArray = value as Array;
					var fieldArray = field.GetValue(obj) as Array;
					if(fieldArray == null || fieldArray.Length != settingArray.Length)
					{
						// (Re)create the field's array.
						fieldArray = Array.CreateInstance(field.FieldType.GetElementType(), settingArray.Length);
					}

					for(int i = 0; i < settingArray.Length; i++)
						fieldArray.SetValue(settingArray.GetValue(i), i);

					field.SetValue(obj, fieldArray);
				}
				else
				{
					field.SetValue(obj, value);
				}
			}
		}

		private static bool ShouldIgnoreMappingFor(MemberInfo member)
		{
			if(member.GetCustomAttributes(typeof(IgnoreAttribute), false).Length > 0)
				return true;
			else
			{
				var prop = member as PropertyInfo;
				if(prop != null)
					return prop.PropertyType.GetCustomAttributes(typeof(IgnoreAttribute), false).Length > 0;

				var field = member as FieldInfo;
				if(field != null)
					return field.FieldType.GetCustomAttributes(typeof(IgnoreAttribute), false).Length > 0;
			}

			return false;
		}

		/// <summary>
		/// Gets an enumerator that iterates through the section.
		/// </summary>
		public IEnumerator<Setting> GetEnumerator()
		{
			return mSettings.GetEnumerator();
		}

		/// <summary>
		/// Gets an enumerator that iterates through the section.
		/// </summary>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <summary>
		/// Adds a setting to the section.
		/// </summary>
		/// <param name="setting">The setting to add.</param>
		public void Add(Setting setting)
		{
			if (setting == null)
				throw new ArgumentNullException(nameof(setting));

			if (Contains(setting))
				throw new ArgumentException("The specified setting already exists in the section.");

			mSettings.Add(setting);
		}

		/// <summary>
		/// Removes a setting from the section by its name.
		/// If there are multiple settings with the same name, only the first setting is removed.
		/// To remove all settings that have the name name, use the RemoveAllNamed() method instead.
		/// </summary>
		/// <param name="settingName">The case-sensitive name of the setting to remove.</param>
		public void Remove(string settingName)
		{
			if (string.IsNullOrEmpty(settingName))
				throw new ArgumentNullException(nameof(settingName));

			var setting = FindSetting(settingName);
			if (setting == null)
				throw new ArgumentException($"A setting named '{settingName}' does not exist in the section.");

			mSettings.Remove(setting);
		}

		/// <summary>
		/// Removes all settings that have a specific name.
		/// </summary>
		/// <param name="settingName">The case-sensitive name of the settings to remove.</param>
		public void RemoveAllNamed(string settingName)
		{
			if (string.IsNullOrEmpty(settingName))
				throw new ArgumentNullException(nameof(settingName));

			for (int i = mSettings.Count - 1; i >= 0; i--)
			{
				if (string.Equals(mSettings[i].Name, settingName, StringComparison.OrdinalIgnoreCase))
					mSettings.RemoveAt(i);
			}
		}

		/// <summary>
		/// Removes a setting from the section.
		/// </summary>
		/// <param name="setting">The setting to remove.</param>
		public void Remove(Setting setting)
		{
			if (setting == null)
				throw new ArgumentNullException(nameof(setting));

			if (!Contains(setting))
				throw new ArgumentException("The specified setting does not exist in the section.");

			mSettings.Remove(setting);
		}

		/// <summary>
		/// Clears the section of all settings.
		/// </summary>
		public void Clear()
		{
			mSettings.Clear();
		}

		/// <summary>
		/// Determines whether a specified setting is contained in the section.
		/// </summary>
		/// <param name="setting">The setting to check for containment.</param>
		/// <returns>True if the setting is contained in the section; false otherwise.</returns>
		public bool Contains(Setting setting)
		{
			return mSettings.Contains(setting);
		}

		/// <summary>
		/// Determines whether a specifically named setting is contained in the section.
		/// </summary>
		/// <param name="settingName">The case-sensitive name of the setting.</param>
		/// <returns>True if the setting is contained in the section; false otherwise.</returns>
		public bool Contains(string settingName)
		{
			return FindSetting(settingName) != null;
		}

		/// <summary>
		/// Gets the number of settings that are in the section.
		/// </summary>
		public int SettingCount
		{
			get { return mSettings.Count; }
		}

		/// <summary>
		/// Gets or sets a setting by index.
		/// </summary>
		/// <param name="index">The index of the setting in the section.</param>
		/// 
		/// <returns>
		/// The setting at the specified index.
		/// Note: no setting is created when using this accessor.
		/// </returns>
		public Setting this[int index]
		{
			get
			{
				if (index < 0 || index >= mSettings.Count)
					throw new ArgumentOutOfRangeException(nameof(index));

				return mSettings[index];
			}
			set
			{
				if (index < 0 || index >= mSettings.Count)
					throw new ArgumentOutOfRangeException(nameof(index));

				mSettings[index] = value;
			}
		}

		/// <summary>
		/// Gets or sets a setting by its name.
		/// If there are multiple settings with the same name, the first setting is returned.
		/// If you want to obtain all settings that have the same name, use the GetSettingsNamed() method instead.
		/// </summary>
		///
		/// <param name="name">The case-sensitive name of the setting.</param>
		///
		/// <returns>
		/// The setting if found, otherwise a new setting with
		/// the specified name is created, added to the section and returned.
		/// </returns>
		public Setting this[string name]
		{
			get
			{
				var setting = FindSetting(name);

				if (setting == null)
				{
					setting = new Setting(name);
					Add(setting);
				}

				return setting;
			}
			set
			{
				// Check if there already is a setting by that name.
				var setting = FindSetting(name);

				int settingIndex = setting != null ? mSettings.IndexOf(setting) : -1;

				if (settingIndex < 0)
					// A setting with that name does not exist yet; add it.
					mSettings.Add(setting);
				else
					// A setting with that name exists; overwrite.
					mSettings[settingIndex] = setting;
			}
		}

		/// <summary>
		/// Gets all settings that have a specific name.
		/// </summary>
		/// <param name="name">The case-sensitive name of the settings.</param>
		/// <returns>
		/// The found settings.
		/// </returns>
		public IEnumerable<Setting> GetSettingsNamed(string name)
		{
			var settings = new List<Setting>();

			foreach(var setting in mSettings)
			{
				if(string.Equals(setting.Name, name, StringComparison.OrdinalIgnoreCase))
					settings.Add(setting);
			}

			return settings;
		}

		/// <summary>
		/// Finds a setting by its name.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		private Setting FindSetting(string name)
		{
			foreach (var setting in mSettings)
			{
				if (string.Equals(setting.Name, name, StringComparison.OrdinalIgnoreCase))
					return setting;
			}

			return null;
		}

		/// <summary>
		/// Gets the string representation of the section, without its comments.
		/// </summary>
		public override string ToString()
		{
			return ToString(false);
		}

		/// <summary>
		/// Gets the string representation of the section.
		/// </summary>
		///
		/// <param name="includeComment">True to include, false to exclude the comment.</param>
		///
		/// <returns>
		/// A string that represents the current object.
		/// </returns>
		public string ToString(bool includeComment)
		{
			if (includeComment)
			{
				bool hasPreComments = mPreComments?.Count > 0;

				string[] preCommentStrings = hasPreComments ?
					mPreComments.ConvertAll(c => c.ToString()).ToArray() : null;

				if (Comment != null && hasPreComments)
					// Include inline comment and pre-comments.
					return $"{string.Join(Environment.NewLine, preCommentStrings)}{Environment.NewLine}[{Name}] {Comment.ToString()}";
				else if (Comment != null)
					// Include only the inline comment.
					return $"[{Name}] {Comment.ToString()}";
				else if (hasPreComments)
					// Include only the pre-comments.
					return $"{string.Join(Environment.NewLine, preCommentStrings)}{Environment.NewLine}[{Name}]";
			}

			return $"[{Name}]";
		}
	}
}