using System;

namespace TravisRFrench.Dependencies.Bindings
{
	public class BindingBuilderValidationException : BindingBuilderException
	{
		public IBinding Binding { get; }
		
		public BindingBuilderValidationException(IBindingBuilder builder, IBinding binding, string message = null, Exception innerException = null)
			: base(builder, message, innerException)
		{
			this.Binding = binding;
		}
	}
}
