using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using StructureMap;
using ConfigurableAppSettings.StructureMap.Sample.Web.Models;

namespace ConfigurableAppSettings.StructureMap.Sample.Web
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801
	public class MvcApplication : System.Web.HttpApplication
	{
		public static void RegisterGlobalFilters( GlobalFilterCollection filters )
		{
			filters.Add( new HandleErrorAttribute() );
		}

		public static void RegisterRoutes( RouteCollection routes )
		{
			routes.IgnoreRoute( "{resource}.axd/{*pathInfo}" );
			routes.IgnoreRoute( "favicon.ico" );

			routes.MapRoute(
				"Default", // Route name
				"{controller}/{action}/{id}", // URL with parameters
				new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
			);

		}

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			RegisterGlobalFilters( GlobalFilters.Filters );
			RegisterRoutes( RouteTable.Routes );

			InitializeContainer();
		}

		public void InitializeContainer()
		{
			ObjectFactory.Initialize( cfg =>
			{
				// 1. configure your app's dependencies
				cfg.Scan( a =>
				{
					a.TheCallingAssembly();
					a.WithDefaultConventions(); // maps Something classes to ISomething interfaces
				} );

				// or: cfg.For<ITokenProvider>().Use<TokenProvider>();
			} );

			// 2. bootstrap the ConfigurableAppSettings bits
			ConfigurableAppSettings.Bootstrap
				.With<ConfigurableAppSettings.StructureMapExtensions>( ObjectFactory.Container )
				.AddSettingsFromAssemblyContaining<MvcApplication>()
				.Initialize();

			// 3. tell MVC3 to use StructureMap when resolving dependencies (e.g. when building controllers)
			DependencyResolver.SetResolver( new StructureMapDependencyResolver( ObjectFactory.Container ) );
		}
	}
}