using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace OneButtonPong
{
	[AddComponentMenu("OneButtonPong/SoundOnStartLevel")]
	public class SoundOnStartLevel : GameBehaviour
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

		protected override void OnStartLevel()
		{
			SoundManager.PlaySound(sound);
		}
	}
}