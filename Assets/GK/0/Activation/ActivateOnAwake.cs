using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[AddComponentMenu("GK/ActivateOnAwake")]
	public class ActivateOnAwake : MonoBehaviour
	{
		public MonoBehaviour[] components;

		public GameObject[] gameObjects;

		void Awake()
		{
			Activate();
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