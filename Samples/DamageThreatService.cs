using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TravisRFrench.Dependencies.Samples
{
	public class DamageThreatService : IThreatService
	{
		private readonly Dictionary<GameObject, int> threatLevels;

		public DamageThreatService(Dictionary<GameObject, int> threatLevels)
		{
			this.threatLevels = threatLevels;
		}

		public GameObject GetHighestThreatTarget(IEnumerable<GameObject> candidates)
		{
			return candidates
				.OrderByDescending(c => threatLevels.TryGetValue(c, out var threat) ? threat : 0)
				.FirstOrDefault();
		}
	}
}