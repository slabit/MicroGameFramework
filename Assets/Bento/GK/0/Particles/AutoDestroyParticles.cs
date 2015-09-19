using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine.UI;

namespace gk
{
	[AddComponentMenu("GK/Particles/AutoDestroyParticles")]
	public class AutoDestroyParticles : MonoBehaviour
	{
		public new ParticleSystem particleSystem;

		bool particlesHaveSpawned;

		void Update()
		{
			if(particlesHaveSpawned)
			{
				if(particleSystem.particleCount <= 0)
					Destroy(gameObject);
			}
			else
			{
				if(particleSystem.particleCount > 0)
					particlesHaveSpawned = true;
			}
		}
	}
}