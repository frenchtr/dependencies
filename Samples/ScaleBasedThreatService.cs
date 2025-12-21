using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TravisRFrench.Dependencies.Samples
{
	public class ScaleBasedThreatService : IThreatService
	{
		public GameObject GetEnemyWithHighestThreat(GameObject requestor, IEnumerable<GameObject> candidates)
		{
			return candidates
				.OrderByDescending(go => go.transform.localScale.sqrMagnitude)
				.FirstOrDefault(go => go != requestor);
		}
	}
}
