using System;

namespace TravisRFrench.Dependencies.Contexts
{
	public abstract class ContextException : DependencyInjectionException
	{
		public ContextException(IContext context, string message = null, Exception innerException = null)
			: base(message, innerException)
		{
		}
	}
}
