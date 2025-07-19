using System;

namespace TravisRFrench.Dependencies.Injection
{
	/// <summary>
	/// Marks a field, property, or method to be injected by the DI container.
	/// Used by <see cref="IInjector"/> and utility methods like <see cref="DI.Inject"/>.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
	public class InjectAttribute : Attribute
	{
	}
}
