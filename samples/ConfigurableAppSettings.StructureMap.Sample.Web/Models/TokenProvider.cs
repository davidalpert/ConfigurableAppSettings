using System;

namespace ConfigurableAppSettings.StructureMap.Sample.Web.Models
{
	public class TokenProvider : ITokenProvider
	{
		public string GetToken()
		{
			return "Bumblebee";
		}
	}
}