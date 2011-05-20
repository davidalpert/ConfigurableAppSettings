using System;

namespace ConfigurableAppSettings
{
	public interface IContainerExtender
	{
		object Container { get; }

		IContainerExtender AddSettingsFromAssemblyContaining<T>();

		void Initialize();
	}
}
