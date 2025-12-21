using TravisRFrench.Dependencies.Containers;
using TravisRFrench.Dependencies.Installers;
using UnityEngine;

namespace TravisRFrench.Dependencies.Samples.Installers
{
	public class SampleApplicationInstaller : MonoInstaller
	{
		[SerializeField]
		private Camera mainCamera;
		
		public override void InstallBindings(IContainer container)
		{
			container.Bind<Camera>()
				.ToSelf()
				.FromInstance(this.mainCamera)
				.AsSingleton()
				.When(ctx => ctx.MemberName == nameof(this.mainCamera) || ctx.ParameterName == nameof(this.mainCamera));
		}
	}
}
