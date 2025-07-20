using System;

namespace TravisRFrench.Dependencies.Resolution
{
	public class BindingNotFoundException : TypeResolutionException
	{
		public BindingNotFoundException(IResolver resolver, Type type, string message, Exception innerException = null)
			: base(resolver, type, message, innerException)
		{
		}
	}
}
