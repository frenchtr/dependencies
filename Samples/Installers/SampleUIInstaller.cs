using TravisRFrench.Dependencies.Containers;
using TravisRFrench.Dependencies.Installers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TravisRFrench.Dependencies.Samples.Installers
{
	public class SampleUIInstaller : MonoInstaller
	{
		[SerializeField]
		private Canvas rootCanvas;
		[SerializeField]
		private EventSystem eventSystem;
		
		public override void InstallBindings(IContainer container)
		{
			container.Bind<EventSystem>()
				.ToSelf()
				.FromInstance(this.eventSystem)
				.AsSingleton();

			container.Bind<Canvas>()
				.ToSelf()
				.FromInstance(this.rootCanvas)
				.AsSingleton()
				.When(ctx => ctx.MemberName == nameof(this.rootCanvas) || ctx.ParameterName == nameof(this.rootCanvas));
		}
	}
}
