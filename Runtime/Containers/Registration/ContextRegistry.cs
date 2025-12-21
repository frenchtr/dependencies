using System.Collections.Generic;
using TravisRFrench.Dependencies.Contexts;

namespace TravisRFrench.Dependencies.Containers.Registration
{
	public class ContextRegistry : IContextRegistry
	{
		private readonly Dictionary<string, IContext> contexts;

		public ContextRegistry()
		{
			this.contexts = new Dictionary<string, IContext>();
		}

		public IContext Get(string key)
		{
			return this.contexts[key];
		}

		public bool TryGet(string key, out IContext container)
		{
			return this.contexts.TryGetValue(key, out container);
		}

		public void Register(string key, IContext container)
		{
			if (this.contexts.ContainsKey(key))
			{
				return;
			}
			
			this.contexts.Add(key, container);
		}

		public void Unregister(string key)
		{
			if (!this.contexts.ContainsKey(key))
			{
				return;
			}
			
			this.contexts.Remove(key);
		}
	}
}
