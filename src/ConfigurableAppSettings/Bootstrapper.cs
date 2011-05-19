using System;

namespace ConfigurableAppSettings
{
	public interface IContainerExtender
	{
		object Container { get; }

		IContainerExtender AddSettingsFromAssemblyContaining<T>();

		void Initialize();
	}

	public abstract class ContainerExtender : IContainerExtender
	{
		public object Container { get; private set; }

		/// <summary>
		/// Initializes a new instance of the ContainerExtender class.
		/// </summary>
		public ContainerExtender() : this( null ) { }

		/// <summary>
		/// Initializes a new instance of the ContainerExtender class.
		/// </summary>
		public ContainerExtender( object container )
		{
			Container = container;
		}

		abstract public IContainerExtender AddSettingsFromAssemblyContaining<T>();

		abstract public void Initialize();
	}

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