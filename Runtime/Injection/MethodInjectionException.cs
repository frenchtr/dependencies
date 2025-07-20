using System;
using System.Reflection;

namespace TravisRFrench.Dependencies.Injection
{
	public class MethodInjectionException : InjectionException
	{
		public MethodInfo Method { get; }

		public MethodInjectionException(IInjector injector, MethodInfo method, string message = null, Exception innerException = null)
			: base(injector, message, innerException)
		{
			this.Method = method;
		}
	}
}
