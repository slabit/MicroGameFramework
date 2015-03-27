using UnityEngine;
using System.Collections;
using System;

namespace gk
{
	[AddComponentMenu("GK/Input/Button/Renderer/gkButtonRenderer_GameObjectActivation")]
	public class gkButtonRenderer_GameObjectActivation : gkButtonRenderer
	{	
		public GameObject[] activateOnUp;
		
		public GameObject[] activateOnDown;
		
		protected override void SetUp()
		{
			Activate(activateOnDown, false);
			Activate(activateOnUp, true);
		}
		
		protected override void SetDown()
		{
			Activate(activateOnUp, false);
			Activate(activateOnDown, true);
		}
		
		private void Activate(GameObject[] a_rGameObjects, bool a_bActivate)
		{
			foreach(GameObject rGameObject in a_rGameObjects)
			{
				if(rGameObject != null)
				{
					rGameObject.SetActive(a_bActivate);
				}
			}
		}
	}
}