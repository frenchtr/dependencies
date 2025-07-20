using System;
using TravisRFrench.Dependencies.Injection;

namespace TravisRFrench.Dependencies.dependencies.Runtime.Injection
{
	public static class InjectorExtensions
	{
		public static TInstance Instantiate<TInstance>(this IInjector injector)
		{
			return (TInstance)(injector.Instantiate(typeof(TInstance)));
		}

		public static TInstance InstantiateFromFactory<TInstance>(this IInjector injector, Func<TInstance> factory)
		{
			return (TInstance)injector.InstantiateFromFactory(factory as Func<object>);
		}
	}
}
