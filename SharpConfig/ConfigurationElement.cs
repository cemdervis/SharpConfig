using System;
using System.Collections.Generic;

namespace Utilities.Configuration
{
	/// <summary>
	///		Represents the base class of all elements that exist in a <see cref="Configuration"/>.
	///		For example sections and settings.
	/// </summary>
	public abstract class ConfigurationElement
	{
		private		string			mName;
		private		Comment?		mComment;
		internal	List<Comment>	mPreComments;

		internal ConfigurationElement(string name)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException(nameof(name));

			mName = name;
		}

		/// <summary>
		///		Gets or sets the name of this element.
		/// </summary>
		public string Name
		{
			get { return mName; }
			set
			{
				if (string.IsNullOrEmpty(value))
					throw new ArgumentNullException(nameof(value));

				mName = value;
			}
		}

		/// <summary>
		///		Gets or sets the comment of this element.
		/// </summary>
		public Comment? Comment
		{
			get { return mComment; }
			set { mComment = value; }
		}

		/// <summary>
		///		Gets the list of comments above this element.
		/// </summary>
		public List<Comment> PreComments
		{
			get
			{
				if (mPreComments == null)
					mPreComments = new List<Comment>();

				return mPreComments;
			}
		}
	}
}