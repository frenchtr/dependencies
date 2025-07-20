using System;
using System.Reflection;

namespace TravisRFrench.Dependencies.Injection
{
	public class ParameterInjectionException : InjectionException
	{
		public ParameterInfo Parameter { get; }

		public ParameterInjectionException(IInjector injector, ParameterInfo parameter, string message = null, Exception innerException = null) : base(injector, message, innerException)
		{
			this.Parameter = parameter;
		}
	}
}
