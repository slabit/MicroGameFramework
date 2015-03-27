using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[AddComponentMenu("GK/Input/gkControlNavigator_ControlSelection_Activation")]
	public class gkControlNavigator_ControlSelection_Activation : MonoBehaviour
	{	
		public List<GameObject> gameObjects; 
		
		public gkControlNavigator_Control control;
		
		private void Awake()
		{
			control.onSelect += OnSelect;
			
			Activate(control.Selected);
		}
		
		private void OnSelect(bool a_bSelect)
		{
			Activate(a_bSelect);
		}
		
		private void Activate(bool a_bActivate)
		{
			foreach(GameObject rGameObject in gameObjects)
			{
				rGameObject.SetActive(a_bActivate);
			}
		}
	}
}