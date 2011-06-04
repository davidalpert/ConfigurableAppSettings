using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using StructureMap;
using ConfigurableAppSettings;

namespace ConfigurableAppSettings.StructureMap.Implementation
{
	public class AppSettingsDiagnosticsProvider : IAppSettingsDiagnosticsProvider
	{
		IAppSettingsKeyNamingStrategy keyNamingStrategy;
		IDiscoverSettingProperties settingPropertyProvider;
		IBuildUpSettings settingsProvider;

		/// <summary>
		/// Initializes a new instance of the AppSettingsDiagnosticsProvider class.
		/// </summary>
		/// <param name="keyNamingStrategy"></param>
		/// <param name="settingPropertyProvider"></param>
		/// <param name="settingsProvider"></param>
		public AppSettingsDiagnosticsProvider( IAppSettingsKeyNamingStrategy keyNamingStrategy, IDiscoverSettingProperties settingPropertyProvider, IBuildUpSettings settingsProvider )
		{
			this.keyNamingStrategy = keyNamingStrategy;
			this.settingPropertyProvider = settingPropertyProvider;
			this.settingsProvider = settingsProvider;
		}

		public string GetSettingsAsXml()
		{
			return GetSettingsAsXml( false );
		}

		public string GetSettingsAsXml( bool showDefaults )
		{
			var settingTypes = ObjectFactory.Container.Model.PluginTypes
				.Where( t => t.PluginType.Name.EndsWith( "Settings" ) && t.PluginType.BaseType == typeof( DictionaryConvertible ) )
				.Select( t => t.PluginType );

			StringWriter output = new StringWriter();
			writeSettings( settingTypes, output, showDefaults );

			return output.ToString();
		}

		private void writeSettings( IEnumerable<Type> settingTypes, TextWriter output, bool showDefaults )
		{
			var xml = new XmlTextWriter( output ) { Formatting = Formatting.Indented };
			xml.WriteStartElement( "appSettings" );
			settingTypes.ToList().ForEach( t =>
			{
				// create an instance
				var settingsInstance = Activator.CreateInstance( t ) as DictionaryConvertible;

				if ( !showDefaults )
					// overwrite defaults with web.config/app.config values
					settingsProvider.InjectConfiguredSettings( settingsInstance );

				// then iterate over the properties
				settingPropertyProvider.GetSettingsProperties( settingsInstance )
										.ToList()
										.ForEach( p =>
				{
					var key = keyNamingStrategy.GetKeyFor( t, p );
					var value = p.GetValue( settingsInstance, null );

					xml.WriteStartElement( "add" );
					xml.WriteAttributeString( "key", key );
					xml.WriteAttributeString( "value", value == null ? "" : value.ToString() );
					xml.WriteEndElement();
				} );
			} );
			xml.WriteEndElement();
			xml.Close();
		}
	}
}
