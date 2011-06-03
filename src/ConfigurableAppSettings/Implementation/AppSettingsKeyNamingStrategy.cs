using System;
using System.Reflection;

namespace ConfigurableAppSettings
{
	public class AppSettingsKeyNamingStrategy : IAppSettingsKeyNamingStrategy
	{
		public string GetKeyFor( Type t, PropertyInfo p )
		{
			return String.Format( "{0}.{1}", t.Name, p.Name );
		}
	}
}
