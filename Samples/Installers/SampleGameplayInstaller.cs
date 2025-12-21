using TravisRFrench.Dependencies.Containers;
using TravisRFrench.Dependencies.Installers;
using UnityEngine;

namespace TravisRFrench.Dependencies.Samples.Installers
{
	public class SampleGameplayInstaller : MonoInstaller
	{
		[SerializeField]
		private SamplePlayer samplePlayer;
		
		public override void InstallBindings(IContainer container)
		{
			container.Bind<SamplePlayer>()
				.ToSelf()
				.FromInstance(this.samplePlayer)
				.AsSingleton();

			container.Bind<IThreatService>()
				.To<DistanceBasedThreatService>()
				.FromNew()
				.AsSingleton();
		}
	}
}
