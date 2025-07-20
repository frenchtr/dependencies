using System;
using TravisRFrench.Dependencies.Injection;

namespace TravisRFrench.Dependencies.Resolution
{
	public class FactoryCreationException : ObjectCreationException
	{
		public FactoryCreationException(Type type, string message, Exception innerException = null)
			: base(type, message, innerException)
		{
		}
	}
}
