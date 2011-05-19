using System;

namespace ConfigurableAppSettings.StructureMap.Sample.Web.Models
{
	public interface ITokenProvider
	{
		string GetToken();
	}
}
