using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[AddComponentMenu("GK/Attraction_Rigidbody_RadialForce")]
	public class Attraction_Rigidbody_RadialForce : MonoBehaviour
	{
		public Rigidbody attracted;

		public Transform target;

		public bool autoStartAttraction = false;

		public float force = 100.0f;

		bool attractionStarted = false;

		bool initialIsKinematic;
		bool initialUseGravity;

		public static void DestroyAttraction(Attraction_Rigidbody_RadialForce attraction)
		{
			attraction.StopAttraction();
			Destroy(attraction);
		}

		public static Attraction_Rigidbody_RadialForce CreateAttraction(Rigidbody attracted, Transform target, float force,
		                                                                    bool startImmediately = true)
		{
			Attraction_Rigidbody_RadialForce attraction = attracted.gameObject.AddComponent<Attraction_Rigidbody_RadialForce>();
			attraction.attracted = attracted;
			attraction.target = target;
			attraction.force = force;

			if(startImmediately)
			{
				attraction.StartAttraction();
			}

			return attraction;
		}

		public void StartAttraction()
		{
			attractionStarted = true;

			initialIsKinematic = attracted.isKinematic;
			attracted.isKinematic = false;

			initialUseGravity = attracted.useGravity;
			attracted.useGravity = false;
		}

		public void StopAttraction()
		{
			attractionStarted = true;

			attracted.isKinematic = initialIsKinematic;
			attracted.useGravity = initialUseGravity;
		}

		void Start()
		{
			if(attractionStarted == false && autoStartAttraction)
			{
				StartAttraction();
			}
		}

		void LateUpdate()
		{
			if(gk.gkTime.Paused)
				return;

			UpdateAttraction();
		}

		void UpdateAttraction()
		{
			Vector3 attractionDistanceVector = target.position - attracted.position;
			Vector3 attractionDirection = attractionDistanceVector.normalized;

			attracted.AddForceAtPosition(attractionDirection * force/attractionDistanceVector.magnitude, attractionDistanceVector);
		}
	}
}