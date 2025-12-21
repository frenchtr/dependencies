using System.Collections.Generic;
using UnityEngine;

namespace TravisRFrench.Dependencies.Samples
{
	public interface IThreatService
	{
		GameObject GetEnemyWithHighestThreat(GameObject requestor, IEnumerable<GameObject> candidates);
	}
}
