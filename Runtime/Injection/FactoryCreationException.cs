using System;

namespace TravisRFrench.Dependencies.Injection
{
	public class FactoryCreationException : ObjectCreationException
	{
		public FactoryCreationException(Type type, string message, Exception innerException = null)
			: base(type, message, innerException)
		{
		}
	}
}
