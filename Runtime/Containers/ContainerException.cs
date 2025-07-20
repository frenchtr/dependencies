using System;

namespace TravisRFrench.Dependencies.Containers
{
	public abstract class ContainerException : DependencyInjectionException
	{
		public ContainerException(IContainer container, string message = null, Exception innerException = null)
			: base(message, innerException)
		{
		}
	}
}
