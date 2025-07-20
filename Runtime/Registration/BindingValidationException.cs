using System;
using TravisRFrench.Dependencies.Bindings;

namespace TravisRFrench.Dependencies.Registration
{
	public class BindingValidationException : RegistryException
	{
		public IBinding Binding { get; }
		
		public BindingValidationException(IRegistry registry, IBinding binding, string message = null, Exception innerException = null)
			: base(registry, message, innerException)
		{
			this.Binding = binding;
		}
	}
}
