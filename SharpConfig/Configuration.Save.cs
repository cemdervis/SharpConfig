using System;
using System.IO;
using System.Text;

namespace SharpConfig
{
	public partial class Configuration
	{
		/// <summary>
		///		Saves the configuration to a file.
		/// </summary>
		/// <param name="filename"> The location of the configuration file. </param>
		/// <param name="encoding"> The character encoding to use. Specify null to use the default encoding, which is UTF8. </param>
		/// <exception cref="ArgumentNullException"> When <paramref name="filename"/> is null or empty. </exception> 
		public void Save(string filename, Encoding encoding = null)
		{
			if(string.IsNullOrEmpty(filename))
				throw new ArgumentNullException(nameof(filename));

			Serialize(filename, encoding);
		}
		
		/// <summary>
		///		Saves the configuration to a stream.
		/// </summary>
		/// <param name="stream"> The stream to save the configuration to. </param>
		/// <param name="encoding"> The character encoding to use. Specify null to use the default encoding, which is UTF8. </param>
		/// <exception cref="ArgumentNullException"> When <paramref name="stream"/> is null. </exception> 
		public void Save(Stream stream, Encoding encoding = null)
		{
			if(stream == null)
				throw new ArgumentNullException(nameof(stream));

			Serialize(stream, encoding);
		}


		/// <summary>
		///		Saves the configuration to a binary file, using a specific <see cref="BinaryWriter"/>.
		/// </summary>
		/// <param name="filename"> The location of the configuration file. </param>
		/// <param name="writer"> The writer to use. Specify null to use the default writer. </param>
		/// <exception cref="ArgumentNullException"> When <paramref name="filename"/> is null or empty. </exception> 
		public void SaveBinary(string filename, BinaryWriter writer = null)
		{
			if(string.IsNullOrEmpty(filename))
				throw new ArgumentNullException(nameof(filename));

			SerializeBinary(writer, filename);
		}
		
		/// <summary>
		///		Saves the configuration to a binary file, using a specific <see cref="BinaryWriter"/>.
		/// </summary>
		/// <param name="stream"> The stream to save the configuration to. </param>
		/// <param name="writer"> The writer to use. Specify null to use the default writer. </param>
		/// <exception cref="ArgumentNullException"> When <paramref name="stream"/> is null. </exception> 
		public void SaveBinary(Stream stream, BinaryWriter writer = null)
		{
			if(stream == null)
				throw new ArgumentNullException(nameof(stream));

			SerializeBinary(writer, stream);
		}
	}
}
