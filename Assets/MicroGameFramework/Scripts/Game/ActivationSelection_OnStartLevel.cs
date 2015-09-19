using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MicroGameFramework
{
	[AddComponentMenu("MicroGameFramework/ActivationSelection_OnStartLevel")]
	public class ActivationSelection_OnStartLevel : GameBehaviour 
	{
		public gk.ActivationGroupSelection activationSelection;

		protected override void OnStartLevel()
		{
			activationSelection.Select();
		}
	}
}
