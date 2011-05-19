using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ConfigurableAppSettings.StructureMap.Sample.Web.Settings;
using ConfigurableAppSettings.StructureMap.Sample.Web.Models;

namespace ConfigurableAppSettings.StructureMap.Sample.Web.Controllers
{
	public class HomeController : Controller
	{
		ITokenProvider tokenProvider;
		SampleSettings sampleSettings;

		/// <summary>
		/// Initializes a new instance of the HomeController class.
		/// </summary>
		/// <param name="sampleSettings"></param>
		public HomeController( ITokenProvider tokenProvider, SampleSettings sampleSettings )
		{
			this.tokenProvider = tokenProvider;
			this.sampleSettings = sampleSettings;
		}

		//
		// GET: /Home/
		public ActionResult Index()
		{
			var model = new HomeIndexViewModel
			{
				Token = tokenProvider.GetToken(),
				Greeting = sampleSettings.Greeting
			};

			return View( model );
		}
	}
}
