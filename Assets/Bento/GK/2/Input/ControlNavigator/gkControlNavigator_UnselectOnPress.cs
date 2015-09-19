using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[AddComponentMenu("GK/Input/gkControlNavigator_UnselectOnPress")]
	public class gkControlNavigator_UnselectOnPress : MonoBehaviour
	{	
		public gkButton unselectButton;
		
		public gkControlNavigator controlNavigator;
		
		private void Awake()
		{
			unselectButton.onDown += OnDown;
		}
		
		private void OnDestroy()
		{
			if(unselectButton != null)
			{
				unselectButton.onDown -= OnDown;
			}
		}
		
		private void OnDown()
		{
			controlNavigator.Select(null);
		}
	}
}