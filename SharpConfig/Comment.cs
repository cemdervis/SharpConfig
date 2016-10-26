namespace SharpConfig
{
	/// <summary>
	///		Represents a comment in a configuration.
	/// </summary>
	public struct Comment
	{
		/// <summary>
		///		The string value of the comment.
		/// </summary>
		public string mValue;

		/// <summary>
		///		The delimiting symbol of the comment.
		/// </summary>
		public char mSymbol;

		/// <summary>
		///		Initializes a new instance of the <see cref="Comment"/> structure,
		///		using the first element in <see cref="Configuration.ValidCommentChars"/> as the comment symbol.
		/// </summary>
		/// <param name="value"> The string value of the comment. </param>
		public Comment(string value)
		{
			mValue	= value;
			mSymbol = Configuration.ValidCommentChars[0];
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="Comment"/> structure.
		/// </summary>
		/// <param name="value"> The string value of the comment. </param>
		/// <param name="symbol"> The delimiting symbol of the comment. </param>
		public Comment(string value, char symbol)
		{
			mValue	= value;
			mSymbol	= symbol;
		}

		/// <summary>
		///		Gets the string representation of the comment.
		/// </summary>
		public override string ToString()
		{
			return $"{mSymbol} {mValue ?? string.Empty}";
		}
	}
}
