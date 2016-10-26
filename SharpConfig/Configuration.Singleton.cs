using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Utilities.Configuration
{
	public partial class Configuration
	{
		internal sealed class Singleton
		{
			/// <summary> 
			///		The one and only Configuration instance.
			///	</summary>
			private static Configuration    mInstance;

			private static string           mConfigurationFileName;

			/// <summary> 
			///		Gets the instance of the configuration object.
			///	</summary>
			public static Configuration Instance
			{
				get { return mInstance; }
			}

			/// <summary> 
			///		Gets the instance of the singleton configuration object.
			///	</summary>
			///	
			/// <returns> The instance of the configuration object. </returns>
			public static Configuration GetInstance()
			{
				return Instance;
			}


			/// <summary>
			///		Loads a configuration from a file.
			/// </summary>
			/// 
			/// <param name="filename"> The location of the configuration file. </param>
			/// <param name="encoding"> The encoding applied to the contents of the file. Specify null to auto-detect the encoding. </param>
			/// 
			/// <exception cref="ArgumentNullException"> When <paramref name="filename"/> is null or empty. </exception> 
			/// <exception cref="FileNotFoundException"> When the specified configuration file is not found. </exception> 
			[MethodImpl(MethodImplOptions.Synchronized)]
			public static void Load(string filename, Encoding encoding = null)
			{
				mConfigurationFileName = filename;

				mInstance = Configuration.Load(filename, encoding);
			}

			/// <summary>
			///		Loads a configuration from a binary file using a specific <see cref="BinaryReader"/>.
			/// </summary>
			/// 
			/// <param name="filename"> The location of the configuration file. </param>
			/// <param name="reader"> The reader to use. Specify null to use the default <see cref="BinaryReader"/>. </param>
			/// 
			/// <exception cref="ArgumentNullException"> When <paramref name="filename"/> is null or empty. </exception> 
			[MethodImpl(MethodImplOptions.Synchronized)]
			public static void LoadBinary(string filename, BinaryReader reader = null)
			{
				mConfigurationFileName = filename;

				mInstance = Configuration.LoadBinary(filename, reader);
			}


			/// <summary>
			///		Saves the configuration to a file using the default character encoding, which is UTF8.
			/// </summary>
			[MethodImpl(MethodImplOptions.Synchronized)]
			public void Save()
			{
				mInstance.Save(mConfigurationFileName);
			}

			/// <summary>
			///		Saves the configuration to a binary file, using the default <see cref="BinaryWriter"/>.
			/// </summary>
			[MethodImpl(MethodImplOptions.Synchronized)]
			public void SaveBinary()
			{
				mInstance.SaveBinary(mConfigurationFileName);
			}
		}


		/// <summary> 
		///		Gets the instance of the singleton configuration object.
		///	</summary>
		public static Configuration Instance
		{
			get { return Singleton.Instance; }
		}

		/// <summary> 
		///		Gets the instance of the singleton configuration object.
		///	</summary>
		///	
		/// <returns> The instance of the configuration object. </returns>
		public static Configuration GetInstance()
		{
			return Instance;
		}
	}
}
