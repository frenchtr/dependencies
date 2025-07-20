using System;

namespace TravisRFrench.Dependencies.Injection
{
	public class ObjectCreationException : DependencyInjectionException
	{
		public ObjectCreationException(Type type, string message, Exception innerException = null)
			: base(message, innerException)
		{
		}
	}
}
