using System.Collections.Generic;
using TravisRFrench.Dependencies.Injection;
using UnityEngine;

namespace TravisRFrench.Dependencies.Samples
{
	public class EnemyAI : MonoBehaviour
	{
		[Inject]
		private IThreatService threatService;

		public void DecideTarget(IEnumerable<GameObject> potentialTargets)
		{
			var target = threatService.GetHighestThreatTarget(potentialTargets);
			if (target != null)
			{
				Debug.Log($"[EnemyAI] Attacking: {target.name}");
			}
		}
	}
}
