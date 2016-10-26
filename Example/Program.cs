using SharpConfig;

namespace Example
{
	class Program
	{
		static void Main(string[] args)
		{
			Configuration.Singleton.Load("SampleSingletonConfiguration_1.config");

			var singletonCfg1 = Configuration.GetSingletonInstance();

			var instanceCfg1 = Configuration.Load("SampleInstanceConfiguration_1.config");
			var instanceCfg2 = Configuration.Load("SampleInstanceConfiguration_2.config");

			Configuration.Singleton.Load("SampleSingletonConfiguration_2.config");
			var singletonCfg2 = Configuration.GetSingletonInstance();
		}
	}
}
