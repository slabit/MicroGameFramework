using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[Serializable]
	public class ActivationGroup
	{
		public GameObject[] gameObjects;
		public MonoBehaviour[] components;

		public void Activate(bool activate)
		{
			foreach(MonoBehaviour component in components)
			{
				if(component == null)
					continue;

				component.enabled = activate;
			}
			
			foreach(GameObject gameObject in gameObjects)
			{
				if(gameObject == null)
					continue;

				gameObject.SetActive(activate);
			}
		}
	}
}