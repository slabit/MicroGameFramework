using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[AddComponentMenu("GK/Attraction_Position_SmoothDamp")]
	public class Attraction_Position_SmoothDamp : MonoBehaviour
	{
		public Transform attracted;

		public Transform target;

		public float smoothTime;

		public bool autoStartAttraction = false;

		public Vector3 initialVelocity;

		Vector3 currentVelocity;

		bool attractionStarted = false;

		public static void DestroyAttraction(Attraction_Position_SmoothDamp attraction)
		{
			attraction.StopAttraction();
			Destroy(attraction);
		}

		public static Attraction_Position_SmoothDamp CreateAttraction(Transform attracted, Transform target, float smoothTime,
		                                                                    Vector3 initialVelocity,
		                                                                    bool startImmediately = true)
		{
			Attraction_Position_SmoothDamp attraction = attracted.gameObject.AddComponent<Attraction_Position_SmoothDamp>();
			attraction.attracted = attracted;
			attraction.target = target;
			attraction.smoothTime = smoothTime;
			attraction.initialVelocity = initialVelocity;

			if(startImmediately)
			{
				attraction.StartAttraction();
			}

			return attraction;
		}

		public void StartAttraction()
		{
			attractionStarted = true;
			currentVelocity = initialVelocity;
		}

		public void StopAttraction()
		{
			attractionStarted = true;
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
			attracted.position = Vector3.SmoothDamp(attracted.position, target.position, ref currentVelocity, smoothTime);
		}
	}
}