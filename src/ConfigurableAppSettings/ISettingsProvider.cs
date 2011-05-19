using System;

namespace ConfigurableAppSettings
{
	public interface ISettingsProvider
	{
		DictionaryConvertible PopulateSettings(DictionaryConvertible instance);
	}
}
