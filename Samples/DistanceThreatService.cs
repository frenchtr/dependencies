using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TravisRFrench.Dependencies.Samples
{
	public class DistanceThreatService : IThreatService
	{
		private readonly Transform enemyTransform;

		public DistanceThreatService(Transform enemyTransform)
		{
			this.enemyTransform = enemyTransform;
		}

		public GameObject GetHighestThreatTarget(IEnumerable<GameObject> candidates)
		{
			return candidates
				.OrderBy(target => Vector3.Distance(target.transform.position, enemyTransform.position))
				.FirstOrDefault();
		}
	}
}
