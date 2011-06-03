using System;
using System.Collections.Generic;
using System.Reflection;

namespace ConfigurableAppSettings
{
	public interface IDiscoverSettingProperties
	{
		IEnumerable<PropertyInfo> GetSettingsProperties( DictionaryConvertible instance );
	}
}
