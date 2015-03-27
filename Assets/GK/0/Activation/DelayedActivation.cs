using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[AddComponentMenu("GK/DelayedActivation")]
	public class DelayedActivation : MonoBehaviour
	{
		public float activationDelay;

		public MonoBehaviour[] components;

		public GameObject[] gameObjects;

		void Start()
		{
			Activate(false);
			Invoke("Activate", activationDelay);
		}

		void Activate()
		{
			Activate(true);
		}

		void Activate(bool activate)
		{
			foreach(MonoBehaviour component in components)
			{
				component.enabled = activate;
			}

			foreach(GameObject gameObject in gameObjects)
			{
				gameObject.SetActive(activate);
			}
		}
	}
}