using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using StructureMap;

namespace ConfigurableAppSettings.StructureMap.Sample.Web
{
	/// <summary>
	/// This is Steve Smith's StructureMapDependencyResolver: http://stevesmithblog.com/blog/how-do-i-use-structuremap-with-asp-net-mvc-3/
	/// </summary>
	/// <remarks>
	/// In MVC2 you'd use a StructureMapControllerFactory and probably a CommonServiceLocator
	/// </remarks>
	public class StructureMapDependencyResolver : IDependencyResolver
	{
		public StructureMapDependencyResolver( IContainer container )
		{
			_container = container;
		}

		public object GetService( Type serviceType )
		{
			if ( serviceType.IsAbstract || serviceType.IsInterface )
			{
				return _container.TryGetInstance( serviceType );
			}
			else
			{
				return _container.GetInstance( serviceType );
			}
		}

		public IEnumerable<object> GetServices( Type serviceType )
		{
			return _container.GetAllInstances<object>()

				.Where( s => s.GetType() == serviceType );
		}

		private readonly IContainer _container;
	}
}
