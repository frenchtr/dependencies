using System;

namespace TravisRFrench.Dependencies.Resolution
{
	public class FactoryCreationException : TypeResolutionException
	{
		public FactoryCreationException(IResolver resolver, Type type, string message, Exception innerException = null)
			: base(resolver, type, message, innerException)
		{
		}
	}
}
