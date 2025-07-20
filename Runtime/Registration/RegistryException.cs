using System;

namespace TravisRFrench.Dependencies.Registration
{
	public abstract class RegistryException : DependencyInjectionException
	{
		public IRegistry Registry { get; }

		public RegistryException(IRegistry registry, string message = null, Exception innerException = null)
			: base(message, innerException)
		{
			this.Registry = registry;
		}
	}
}
