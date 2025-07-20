using System;

namespace TravisRFrench.Dependencies.Bindings
{
	public abstract class BindingBuilderException : DependencyInjectionException
	{
		public IBindingBuilder Builder { get; }

		public BindingBuilderException(IBindingBuilder builder, string message = null, Exception innerException = null)
			: base(message, innerException)
		{
			this.Builder = builder;
		}
	}
}
