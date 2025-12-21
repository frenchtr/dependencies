using TravisRFrench.Dependencies.Contexts;

namespace TravisRFrench.Dependencies.Containers.Registration
{
	public interface IContextRegistry
	{
		IContext Get(string key);
		bool TryGet(string key, out IContext context);
		void Register(string key, IContext context);
		void Deregister(string key);
	}
}
