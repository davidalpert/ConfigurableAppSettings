using System;

namespace ConfigurableAppSettings
{
	public class Bootstrap
	{
		public static IContainerExtender With<T>()
			where T : IContainerExtender
		{
			var extender = Activator.CreateInstance<T>();

			return extender; // to be fluent
		}

		public static IContainerExtender With<T>( object container )
			where T : IContainerExtender
		{
			var extender = Activator.CreateInstance( typeof( T ), container ) as IContainerExtender;

			return extender; // to be fluent
		}
	}
}