using System;
using TravisRFrench.Dependencies.Injection;

namespace TravisRFrench.Dependencies.Resolution
{
	public class ConstructorCreationException : ObjectCreationException
	{
		public ConstructorCreationException(Type type, string message, Exception innerException = null)
			: base(type, message, innerException)
		{
		}
	}
}
