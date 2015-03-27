using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[AddComponentMenu("GK/Attractor_Position_OffsetSmoothDamp")]
	public class Attraction_Position_OffsetSmoothDamp : MonoBehaviour
	{
		public Transform attracted;

		public Transform target;

		public float smoothTime;

		public bool autoStartAttraction = false;

		public Vector3 initialVelocity;

		Vector3 currentVelocity;

		Vector3 offsetFromTarget;

		bool attractionStarted = false;

		public static Attraction_Position_OffsetSmoothDamp CreateAttraction(Transform attracted, Transform target, float smoothTime,
		                                                                    Vector3 initialRelativeVelocity,
		                                                                    bool startImmediately = true)
		{
			Attraction_Position_OffsetSmoothDamp attraction = attracted.gameObject.AddComponent<Attraction_Position_OffsetSmoothDamp>();
			attraction.attracted = attracted;
			attraction.target = target;
			attraction.smoothTime = smoothTime;
			attraction.initialVelocity = initialRelativeVelocity;

			if(startImmediately)
			{
				attraction.StartAttraction();
			}

			return attraction;
		}

		public void StartAttraction()
		{
			attractionStarted = true;
			offsetFromTarget = attracted.position - target.position;
			currentVelocity = initialVelocity;
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
			offsetFromTarget = Vector3.SmoothDamp(offsetFromTarget, Vector3.zero, ref currentVelocity, smoothTime);
			attracted.position = target.position + offsetFromTarget;
		}
	}
}