using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace OneButtonPong
{
	[AddComponentMenu("OneButtonPong/RestartOnGameOver")]
	public class RestartOnGameOver : GameBehaviour
	{
		bool canRestart;

		protected override void OnAwake()
		{
			Game.onGameOver += OnGameOver;
		}

		protected override void OnAwakeEnd()
		{
			if(Game != null)
				Game.onGameOver -= OnGameOver;
		}

		protected override void OnStartLevel()
		{
			canRestart = false;
			CancelInvoke("CanRestart");
		}

		void Update()
		{
			if(canRestart == false)
				return;

			if(Input.GetButtonDown("Action"))
				Game.Restart();
		}

		void OnGameOver()
		{
			Invoke("CanRestart", 0.2f);
		}

		void CanRestart()
		{
			canRestart = true;
		}
	}
}