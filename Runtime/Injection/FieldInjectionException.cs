using System;
using System.Reflection;

namespace TravisRFrench.Dependencies.Injection
{
	public class FieldInjectionException : InjectionException
	{
		public FieldInfo Field { get; }

		public FieldInjectionException(IInjector injector, FieldInfo field, string message = null, Exception innerException = null)
			: base(injector, message, innerException)
		{
			this.Field = field;
		}
	}
}
