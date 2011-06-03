using System;

namespace ConfigurableAppSettings
{
	/// <summary>
	/// A marker attribute; designates that the marked property is not
	/// to be populated when injecting config values and is not to be
	/// reported when running diagnostics.
	/// </summary>
	public class NotConfigurableAttribute : Attribute
	{
	}
}
