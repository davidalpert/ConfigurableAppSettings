using System;
using System.Reflection;

namespace ConfigurableAppSettings
{
	public interface IAppSettingsKeyNamingStrategy
	{
		string GetKeyFor( Type t, PropertyInfo p );
	}
}
