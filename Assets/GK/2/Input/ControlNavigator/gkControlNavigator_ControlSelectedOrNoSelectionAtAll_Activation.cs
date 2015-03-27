using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[AddComponentMenu("GK/Input/gkControlNavigator_ControlSelectedOrNoSelectionAtAll_Activation")]
	public class gkControlNavigator_ControlSelectedOrNoSelectionAtAll_Activation : MonoBehaviour
	{	
		public List<GameObject> gameObjects; 

		public gkControlNavigator_Control control;

		public List<gkControlNavigator_Control> otherControls;

		private void Awake()
		{
			control.onSelect += OnSelect;
			foreach (gkControlNavigator_Control rOtherControl in otherControls) 
			{
				rOtherControl.onSelect += OnSelect;
			}
			
			UpdateActivation();
		}
		
		private void OnSelect(bool a_bSelect)
		{
			UpdateActivation();
		}

		private void UpdateActivation()
		{
			Activate(MustActivate ());
		}

		private bool MustActivate()
		{
			if(control.Selected)
			{
				return true;
			}
			
			foreach(gkControlNavigator_Control rOtherControl in otherControls)
			{
				if(rOtherControl.Selected)
				{
					return false;
				}
			}
			
			return true;
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