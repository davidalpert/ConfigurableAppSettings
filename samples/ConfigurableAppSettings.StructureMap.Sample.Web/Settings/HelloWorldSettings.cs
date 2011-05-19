using System;

namespace ConfigurableAppSettings.StructureMap.Sample.Web.Settings
{
	public class SampleSettings : DictionaryConvertible
	{
		/// <summary>
		/// Initializes a new instance of the HelloWorldSettings class.
		/// </summary>
		public SampleSettings()
		{
			Greeting = "Hello world.";
		}

		public string Greeting { get; set; }
	}
}