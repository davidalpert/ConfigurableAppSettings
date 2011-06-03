using System;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace ConfigurableAppSettings
{
	public class SettingsProvider : IBuildUpSettings
	{
		IAppSettingsKeyNamingStrategy namingStrategy;
		IDiscoverSettingProperties settingPropertyProvider;

		/// <summary>
		/// Initializes a new instance of the SettingsProvider class.
		/// </summary>
		/// <param name="namingStrategy"></param>
		/// <param name="settingsPropertyProvider"></param>
		public SettingsProvider( IAppSettingsKeyNamingStrategy namingStrategy, IDiscoverSettingProperties settingsPropertyProvider )
		{
			this.namingStrategy = namingStrategy;
			this.settingPropertyProvider = settingsPropertyProvider;
		}

		public DictionaryConvertible InjectConfiguredSettings( DictionaryConvertible instance )
		{
			Type[] types = new Type[1];
			types[0] = typeof( string );

			if ( instance != null )
			{
				// use reflection to iterate over each public property of instance except for 'Problems' 
				// which is reserved for error messages on the object
				var properties = settingPropertyProvider.GetSettingsProperties( instance );

				foreach ( PropertyInfo property in properties )
				{
					Type t = property.PropertyType;
					string propertyName = namingStrategy.GetKeyFor( instance.GetType(), property );

					if ( ConfigurationManager.AppSettings.AllKeys.Contains( propertyName ) )
					{
						object value = ConfigurationManager.AppSettings[propertyName] as object;
						try
						{
							if ( t.IsEnum )
							{
								value = EnumHelper.ParseAs( (string)value, t, true );
							}

							if ( t == typeof( bool ) )
							{
								bool temp;
								if ( bool.TryParse( (string)value, out temp ) )
								{
									value = temp;
								}
							}

							property.SetValue( instance, value, null );
						}
						catch ( Exception setvalueEx )
						{
							instance.AddProblem( setvalueEx );
						}
					}
				}
			}

			else
			{
				throw new ArgumentNullException( "Instance cannot be null!" );
			}

			return instance;
		}
	}
}