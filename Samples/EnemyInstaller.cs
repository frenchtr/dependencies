using System.Collections.Generic;
using TravisRFrench.Dependencies.Containers;
using TravisRFrench.Dependencies.Installers;
using UnityEngine;

namespace TravisRFrench.Dependencies.Samples
{
	[CreateAssetMenu(menuName = "Scriptables/DI/Enemy Installer")]
	public class EnemyInstaller : ScriptableInstaller
	{
		[SerializeField]
		private bool useDistanceThreat = true;

		public override void InstallBindings(IContainer container)
		{
			if (useDistanceThreat)
			{
				container.Bind<IThreatService>()
					.FromFactory(() => new DistanceThreatService(null))
					.AsSingleton();
			}
			else
			{
				var mockThreatLevels = new Dictionary<GameObject, int>();
				container.Bind<IThreatService>()
					.FromFactory(() => new DamageThreatService(mockThreatLevels))
					.AsSingleton();
			}
		}
	}
}
