namespace SharpConfig
{
	/// <summary>
	///		Enumerates the elements of a Setting that represents an array.
	/// </summary>
	internal struct SettingArrayEnumerator
	{
		private string	mRawValue;
		private string	mCurrentElement;
		private int		mIndex;
		private int		mArrayEndIdx;
		private int		mLastElementIdx;

		public SettingArrayEnumerator(string rawValue)
		{
			mRawValue		= rawValue;
			mCurrentElement = null;
			mIndex			= -1;

			// Jump to the beginning of the array expression.
			mLastElementIdx = rawValue.IndexOf('{') + 1;
			mArrayEndIdx	= rawValue.LastIndexOf('}');
		}

		public bool Next()
		{
			if (mLastElementIdx > mArrayEndIdx)
				return false;

			int subStrBegin;
			int subStrEnd;

			int nextElementIdx = mRawValue.IndexOf(Configuration.ArrayElementSeparator, mLastElementIdx + 1);
			if (nextElementIdx < 0)
			{
				// Last element.
				subStrBegin = mLastElementIdx;
				subStrEnd	= mArrayEndIdx;
			}
			else
			{
				// An element has been found.
				subStrBegin = mLastElementIdx;
				subStrEnd	= nextElementIdx;
			}

			mCurrentElement = mRawValue.Substring(subStrBegin, subStrEnd - subStrBegin);
			mCurrentElement = mCurrentElement.Trim();
			mLastElementIdx = subStrEnd + 1;
			++mIndex;

			return true;
		}

		public string Current
		{
			get { return mCurrentElement; }
		}

		public int Index
		{
			get { return mIndex; }
		}
	}
}
