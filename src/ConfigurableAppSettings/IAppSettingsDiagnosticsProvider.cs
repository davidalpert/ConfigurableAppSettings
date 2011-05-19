using System;

namespace ConfigurableAppSettings
{
	public interface IAppSettingsDiagnosticsProvider
	{
		string ExtractSampleSettings();
	}
}
