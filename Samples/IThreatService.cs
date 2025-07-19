using System.Collections.Generic;
using UnityEngine;

namespace TravisRFrench.Dependencies.Samples
{
	public interface IThreatService
	{
		GameObject GetHighestThreatTarget(IEnumerable<GameObject> candidates);
	}
}
