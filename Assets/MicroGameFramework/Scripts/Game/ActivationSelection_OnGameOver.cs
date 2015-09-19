using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MicroGameFramework
{
	[AddComponentMenu("MicroGameFramework/ActivationSelection_OnGameOver")]
	public class ActivationSelection_OnGameOver : GameBehaviour 
	{
		public gk.ActivationGroupSelection activationSelection;

		protected override void OnAwake()
		{
			Game.onGameOver += OnGameOver;
		}

		protected override void OnAwakeEnd()
		{
			Game.onGameOver -= OnGameOver;
		}

		void OnGameOver()
		{
			activationSelection.Select();
		}
	}
}
