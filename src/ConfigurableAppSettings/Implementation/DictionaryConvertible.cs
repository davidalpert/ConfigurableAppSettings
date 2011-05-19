using System;
using System.Collections.Generic;

namespace ConfigurableAppSettings
{
	public abstract class DictionaryConvertible : IDictionaryConvertible
	{
		private readonly List<Exception> _problems = new List<Exception>();

		public IEnumerable<Exception> Problems { get { return _problems; } }

		public void AddProblem( Exception problem )
		{
			_problems.Add( problem );
		}
	}
}