using System;

namespace TravisRFrench.Dependencies.Resolution
{
	public class ConstructorCreationException : TypeResolutionException
	{
		public ConstructorCreationException(IResolver resolver, Type type, string message, Exception innerException = null)
			: base(resolver, type, message, innerException)
		{
		}
	}
}
