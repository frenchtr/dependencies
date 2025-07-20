using System;

namespace TravisRFrench.Dependencies.Resolution
{
	public class TypeResolutionException : ResolverException
	{
		public Type Type { get; }

		public TypeResolutionException(IResolver resolver, Type type, string message, Exception innerException = null)
			: base(resolver, message, innerException)
		{
			this.Type = type;
		}
	}
}
