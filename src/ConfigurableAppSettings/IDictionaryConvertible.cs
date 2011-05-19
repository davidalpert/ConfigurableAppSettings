using System;
using System.Collections.Generic;

namespace ConfigurableAppSettings
{
	interface IDictionaryConvertible
	{
		void AddProblem(Exception problem);

		IEnumerable<Exception> Problems { get; }
	}
}
