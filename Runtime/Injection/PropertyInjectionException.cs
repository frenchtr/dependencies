using System;
using System.Reflection;

namespace TravisRFrench.Dependencies.Injection
{
	public class PropertyInjectionException : InjectionException
	{
		public PropertyInfo Property { get; }

		public PropertyInjectionException(IInjector injector, PropertyInfo property, string message = null, Exception innerException = null)
			: base(injector, message, innerException)
		{
			this.Property = property;
		}
	}
}
