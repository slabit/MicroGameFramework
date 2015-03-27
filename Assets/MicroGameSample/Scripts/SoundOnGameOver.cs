using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace OneButtonPong
{
	[AddComponentMenu("OneButtonPong/SoundOnGameOver")]
	public class SoundOnGameOver : GameBehaviour
	{
		public AudioClip sound;

		SoundManager SoundManager
		{
			get
			{
				if(SceneManagers.Instance == null)
					return null;
				
				return SceneManagers.Instance.soundManager;
			}
		}

		protected override void OnAwake()
		{
			Game.onGameOver += OnGameOver;
		}
		
		protected override void OnAwakeEnd()
		{
			if(Game != null)
				Game.onGameOver -= OnGameOver;
		}

		void OnGameOver()
		{
			SoundManager.PlaySound(sound);
		}
	}
}