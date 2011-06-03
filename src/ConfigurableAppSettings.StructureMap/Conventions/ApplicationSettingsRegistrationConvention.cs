using System;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using ConfigurableAppSettings;

namespace ConfigurableAppSettings.StructureMap.Conventions
{
	public class ApplicationSettingsRegistrationConvention : IRegistrationConvention
	{
		public void Process( Type type, Registry registry )
		{
			if ( !type.Name.EndsWith( "Settings" ) || !typeof( DictionaryConvertible ).IsAssignableFrom( type ) ) return;
			registry.For( type )
				.EnrichWith( ( session, original ) =>
				{
					return session.GetInstance<IBuildUpSettings>().InjectConfiguredSettings( (DictionaryConvertible)original );
				} )
				.Use( type );
		}
	}
}
