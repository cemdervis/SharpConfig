using System;
using System.IO;
using System.Text;

namespace SharpConfig
{
	public partial class Configuration
	{
		/// <summary>
		///		Loads a configuration from a file.
		/// </summary>
		/// <param name="filename"> The location of the configuration file. </param>
		/// <param name="encoding"> The encoding applied to the contents of the file. Specify null to auto-detect the encoding. </param>
		/// <returns>
		///		The loaded <see cref="Configuration"/> object.
		/// </returns>
		/// <exception cref="ArgumentNullException"> When <paramref name="filename"/> is null or empty. </exception> 
		/// <exception cref="FileNotFoundException"> When the specified configuration file is not found. </exception> 
		public static Configuration Load(string filename, Encoding encoding = null)
		{
			if(string.IsNullOrEmpty(filename))
				throw new ArgumentNullException(nameof(filename));

			if(!File.Exists(filename))
				throw new FileNotFoundException("Configuration file not found.", filename);
			
			return encoding == null ?
				LoadFromString(File.ReadAllText(filename)) :
				LoadFromString(File.ReadAllText(filename, encoding));
		}
		
		/// <summary>
		///		Loads a configuration from a text stream.
		/// </summary>
		/// <param name="stream"> The text stream to load the configuration from. </param>
		/// <param name="encoding"> The encoding applied to the contents of the stream. Specify null to auto-detect the encoding. </param>
		/// <returns>
		///		The loaded <see cref="Configuration"/> object.
		/// </returns>
		/// <exception cref="ArgumentNullException"> When <paramref name="stream"/> is null. </exception> 
		public static Configuration Load(Stream stream, Encoding encoding = null)
		{
			if(stream == null)
				throw new ArgumentNullException(nameof(stream));

			string source = null;

			var reader = encoding == null ?
				new StreamReader(stream) :
				new StreamReader(stream, encoding);

			using(reader)
			{
				source = reader.ReadToEnd();
			}

			return LoadFromString(source);
		}

		/// <summary>
		///		Loads a configuration from text (source code).
		/// </summary>
		/// <param name="source"> The text (source code) of the configuration. </param>
		/// <returns>
		///		The loaded <see cref="Configuration"/> object.
		/// </returns>
		/// <exception cref="ArgumentNullException">When <paramref name="source"/> is null.</exception> 
		private static Configuration LoadFromString(string source)
		{
			if(source == null)
				throw new ArgumentNullException(nameof(source));

			return Parse(source);
		}
		

		/// <summary>
		///		Loads a configuration from a binary file using a specific <see cref="BinaryReader"/>.
		/// </summary>
		/// <param name="filename"> The location of the configuration file. </param>
		/// <param name="reader"> The reader to use. Specify null to use the default <see cref="BinaryReader"/>. </param>
		/// <returns>
		///		The loaded configuration.
		/// </returns>
		/// <exception cref="ArgumentNullException"> When <paramref name="filename"/> is null or empty. </exception> 
		public static Configuration LoadBinary(string filename, BinaryReader reader = null)
		{
			if(string.IsNullOrEmpty(filename))
				throw new ArgumentNullException(nameof(filename));

			return DeserializeBinary(reader, filename);
		}

		/// <summary>
		///		Loads a configuration from a binary stream, using a specific <see cref="BinaryReader"/>.
		/// </summary>
		/// <param name="stream"> The stream to load the configuration from. </param>
		/// <param name="reader"> The reader to use. Specify null to use the default <see cref="BinaryReader"/>. </param>
		/// <returns>
		///		The loaded configuration.
		/// </returns>
		/// <exception cref="ArgumentNullException"> When <paramref name="stream"/> is null. </exception> 
		public static Configuration LoadBinary(Stream stream, BinaryReader reader = null)
		{
			if(stream == null)
				throw new ArgumentNullException(nameof(stream));

			return DeserializeBinary(reader, stream);
		}
	}
}
