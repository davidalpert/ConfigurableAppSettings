using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConfigurableAppSettings.StructureMap.Sample.Web.Controllers
{
	public class DiagnosticsController : Controller
	{
		IAppSettingsDiagnosticsProvider appSettingsDiagnosticsProvider;

		/// <summary>
		/// Initializes a new instance of the DiagnosticsController class.
		/// </summary>
		/// <param name="appSettingsDiagnosticsProvider"></param>
		public DiagnosticsController( IAppSettingsDiagnosticsProvider appSettingsDiagnosticsProvider )
		{
			this.appSettingsDiagnosticsProvider = appSettingsDiagnosticsProvider;
		}

		// 
		// GET: /Diagnostics/
		// NOTE: you can "hide" this route by only mapping it when Debug is enabled, for example.
		public ActionResult Index()
		{
			string sampleConfigEntries = appSettingsDiagnosticsProvider.GetSettingsAsXml();

			return Content( "<pre>"+Server.HtmlEncode( sampleConfigEntries )+"</pre>" );
		}

	}
}
