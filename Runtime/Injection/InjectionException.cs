using System;

namespace TravisRFrench.Dependencies.Injection
{
	public class InjectionException : DependencyInjectionException
	{
		public IInjector Injector { get; }

		public InjectionException(IInjector injector, string message = null, Exception innerException = null)
			: base(message, innerException)
		{
			this.Injector = injector;
		}
	}
}
