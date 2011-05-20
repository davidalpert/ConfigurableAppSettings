using System;
using System.Collections.Generic;

namespace ConfigurableAppSettings.Implementation
{
	public abstract class ContainerExtender : IContainerExtender
	{
		public object Container { get; protected set; }

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

			assemblyTypeMarkers = new HashSet<Type>();
		}

		protected ICollection<Type> assemblyTypeMarkers;

		public IContainerExtender AddSettingsFromAssemblyContaining<T>()
		{
			assemblyTypeMarkers.Add( typeof( T ) );

			return this;
		}

		abstract public void Initialize();
	}
}
