using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace OneButtonPong
{
	[AddComponentMenu("OneButtonPong/GameOverScreenController")]
	public class GameOverScreenController : GameBehaviour
	{
		public GameObject gameOverScreenRoot;

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
			gameOverScreenRoot.SetActive(false);
		}

		void OnGameOver()
		{
			gameOverScreenRoot.SetActive(true);
		}
	}
}