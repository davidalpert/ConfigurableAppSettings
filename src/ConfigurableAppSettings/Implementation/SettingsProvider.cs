using System;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace ConfigurableAppSettings
{
	public class SettingsProvider : ISettingsProvider
	{
		IAppSettingsKeyNamingStrategy namingStrategy;

		/// <summary>
		/// Initializes a new instance of the SettingsProvider class.
		/// </summary>
		/// <param name="namingStrategy"></param>
		public SettingsProvider( IAppSettingsKeyNamingStrategy namingStrategy )
		{
			this.namingStrategy = namingStrategy;
		}

		public DictionaryConvertible PopulateSettings( DictionaryConvertible instance )
		{
			Type[] types = new Type[1];
			types[0] = typeof( string );

			if ( instance != null )
			{
				// use reflection to iterate over each public property of instance except for 'Problems' 
				// which is reserved for error messages on the object
				var properties = instance.GetType().GetProperties().Where( prop => ( prop.Name.Equals( "Problems" ) == false ) );

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