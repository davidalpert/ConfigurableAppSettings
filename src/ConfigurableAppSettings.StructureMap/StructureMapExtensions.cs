using System;
using System.Collections.Generic;
using System.Linq;
using StructureMap;
using ConfigurableAppSettings.StructureMap.Conventions;
using ConfigurableAppSettings.StructureMap.Implementation;

namespace ConfigurableAppSettings
{
	public class StructureMapExtensions : ContainerExtender
	{
		/// <summary>
		/// Initializes a new instance of the Bootstrap class.
		/// </summary>
		/// <param name="assemblyTypeMarkers"></param>
		public StructureMapExtensions()
			: this( null )
		{
		}

		/// <summary>
		/// Initializes a new instance of the StructureMapExtensions class.
		/// </summary>
		public StructureMapExtensions( object container )
			: base( container )
		{
			this.assemblyTypeMarkers = new HashSet<Type>();
		}

		ICollection<Type> assemblyTypeMarkers;

		public override IContainerExtender AddSettingsFromAssemblyContaining<T>()
		{
			assemblyTypeMarkers.Add( typeof( T ) );

			return this;
		}

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
				cfg.For<ISettingsProvider>().Use<SettingsProvider>();
			} );
		}
	}
}
