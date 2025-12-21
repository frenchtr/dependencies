using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TravisRFrench.Dependencies.Samples
{
	public class DistanceBasedThreatService : IThreatService
	{
		public GameObject GetEnemyWithHighestThreat(GameObject requestor, IEnumerable<GameObject> candidates)
		{
			return candidates
				.OrderBy(go => Vector3.Distance(requestor.transform.position, go.transform.position))
				.FirstOrDefault(go => go != requestor);
		}
	}
}
