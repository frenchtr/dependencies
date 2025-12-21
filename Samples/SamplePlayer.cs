using System;
using NUnit.Framework;
using TravisRFrench.Dependencies.Injection;
using UnityEngine;

namespace TravisRFrench.Dependencies.Samples
{
	public class SamplePlayer : MonoBehaviour, ITargetable
	{
		[Inject]
		private Camera mainCamera;

		private void Start()
		{
			Assert.NotNull(this.mainCamera);
		}
	}
}
