using System;

namespace ConfigurableAppSettings
{
	public interface IAppSettingsDiagnosticsProvider
	{
		string GetSettingsAsXml();
		string GetSettingsAsXml( bool showDefaults );
	}
}
