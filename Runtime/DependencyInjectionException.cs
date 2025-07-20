using System;

namespace TravisRFrench.Dependencies
{
	public abstract class DependencyInjectionException : Exception
	{
		public DependencyInjectionException(string message, Exception innerException = null)
			: base(message, innerException)
		{
		}
	}
}
