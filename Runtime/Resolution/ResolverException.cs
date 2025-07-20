using System;

namespace TravisRFrench.Dependencies.Resolution
{
	public abstract class ResolverException : DependencyInjectionException
	{
		public ResolverException(IResolver resolver, string message, Exception innerException = null)
			: base(message, innerException)
		{
		}
	}
}
