using System;

namespace ConfigurableAppSettings
{
	public interface IBuildUpSettings
	{
		DictionaryConvertible InjectConfiguredSettings( DictionaryConvertible instance );
	}
}
