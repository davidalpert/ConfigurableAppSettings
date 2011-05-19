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

		/// <summary>
		/// Initializes a new instance of the AppSettingsDiagnosticsProvider class.
		/// </summary>
		/// <param name="keyNamingStrategy"></param>
		public AppSettingsDiagnosticsProvider( IAppSettingsKeyNamingStrategy keyNamingStrategy )
		{
			this.keyNamingStrategy = keyNamingStrategy;
		}

		public string ExtractSampleSettings()
		{
			var settingTypes = ObjectFactory.Container.Model.PluginTypes
				.Where( t => t.PluginType.Name.EndsWith( "Settings" ) && t.PluginType.BaseType == typeof( DictionaryConvertible ) )
				.Select( t => t.PluginType );

			StringWriter output = new StringWriter();
			dumpSettings( settingTypes, output );

			return output.ToString();
		}

		private void dumpSettings( IEnumerable<Type> settingTypes, TextWriter output )
		{
			var xml = new XmlTextWriter( output ) { Formatting = Formatting.Indented };
			xml.WriteStartElement( "appSettings" );
			settingTypes.ToList().ForEach( t =>
			{
				var settings = Activator.CreateInstance( t );
				var properties = t.GetProperties( BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public );

				properties.ToList().ForEach( p =>
				{
					var key = keyNamingStrategy.GetKeyFor( t, p );
					var value = p.GetValue( settings, null );

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
