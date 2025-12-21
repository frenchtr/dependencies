using TravisRFrench.Dependencies.Containers;
using TravisRFrench.Dependencies.Installers;

namespace TravisRFrench.Dependencies.Samples.Installers
{
	public class SampleEnemyInstaller : MonoInstaller
	{
		public override void InstallBindings(IContainer container)
		{
			container.Bind<IThreatService>()
				.To<ScaleBasedThreatService>()
				.FromNew()
				.AsSingleton();
		}
	}
}
