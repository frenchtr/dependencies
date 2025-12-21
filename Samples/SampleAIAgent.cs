using System.Collections;
using System.Linq;
using TravisRFrench.Dependencies.Injection;
using UnityEngine;

namespace TravisRFrench.Dependencies.Samples
{
	public class SampleAIAgent : MonoBehaviour, ITargetable
	{
		[SerializeField]
		private float attackSpeed = 1f;
		[Inject]
		private IThreatService threatService;
		
		private IEnumerator Start()
		{
			while (true)
			{
				yield return new WaitForSeconds(this.attackSpeed);

				var candidates =
					FindObjectsByType<GameObject>(FindObjectsInactive.Exclude, FindObjectsSortMode.InstanceID)
						.Where(go => go != this.gameObject)
						.Select(go => go.GetComponent<ITargetable>())
						.Where(t => t != null)
						.OfType<MonoBehaviour>()
						.Select(mb => mb.gameObject);
				
				var target = this.threatService.GetEnemyWithHighestThreat(this.gameObject, candidates);
				
				Debug.Log($"[{this.name}]: Attacking {target.name}!");
			}
		}
	}
}
