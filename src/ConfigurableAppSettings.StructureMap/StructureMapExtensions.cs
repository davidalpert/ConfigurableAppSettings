using System;
using System.Linq;
using StructureMap;
using ConfigurableAppSettings.Implementation;
using ConfigurableAppSettings.StructureMap.Conventions;
using ConfigurableAppSettings.StructureMap.Implementation;

namespace ConfigurableAppSettings
{
	public class StructureMapExtensions : ContainerExtender
	{
		/// <summary>
		/// Initializes a new instance of the StructureMapExtensions class
		/// for the default IContainer (via the static ObjectFactory.Container
		/// property.
		/// </summary>
		/// <param name="assemblyTypeMarkers"></param>
		public StructureMapExtensions() : this( null ) { }

		/// <summary>
		/// Initializes a new instance of the StructureMapExtensions class
		/// for a given IContainer.
		/// </summary>
		public StructureMapExtensions( object container ) : base( container ) { }

		public override void Initialize()
		{
			( ( Container as IContainer ) ?? ObjectFactory.Container ).Configure( cfg =>
			{
				cfg.Scan( a =>
				{
					assemblyTypeMarkers.ToList()
						.ForEach( t => a.AssemblyContainingType( t ) );

					a.Convention<ApplicationSettingsRegistrationConvention>();

					a.AssemblyContainingType<ConfigurableAppSettings.SettingsProvider>();
				} );

				cfg.For<IAppSettingsKeyNamingStrategy>().Use<AppSettingsKeyNamingStrategy>();
				cfg.For<IAppSettingsDiagnosticsProvider>().Use<AppSettingsDiagnosticsProvider>();
				cfg.For<IBuildUpSettings>().Use<SettingsProvider>();
				cfg.For<IDiscoverSettingProperties>().Use<SettingPropertyProvider>();
			} );
		}
	}
}
