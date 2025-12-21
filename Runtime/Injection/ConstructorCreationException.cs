using System;

namespace TravisRFrench.Dependencies.Injection
{
	public class ConstructorCreationException : ObjectCreationException
	{
		public ConstructorCreationException(Type type, string message, Exception innerException = null)
			: base(type, message, innerException)
		{
		}
	}
}
