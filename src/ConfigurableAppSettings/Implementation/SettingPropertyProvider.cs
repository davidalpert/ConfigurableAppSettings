using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ConfigurableAppSettings
{
	public class SettingPropertyProvider : IDiscoverSettingProperties
	{
		public IEnumerable<PropertyInfo> GetSettingsProperties( DictionaryConvertible instance )
		{
			if ( instance != null )
			{
				// use reflection to iterate over each public property of instance except for 'Problems' 
				// which is reserved for error messages on the object
				return instance.GetType().GetProperties()
											.Where( prop => ( prop.Name.Equals( "Problems" ) == false ) )
											.Where( prop => prop.GetCustomAttributes( typeof( NotConfigurableAttribute ), false ).Count() == 0 );
			}

			return Enumerable.Empty<PropertyInfo>();
		}
	}
}
