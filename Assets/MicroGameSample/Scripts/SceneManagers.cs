using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace OneButtonPong
{
	[AddComponentMenu("OneButtonPong/SceneManagers")]
	public class SceneManagers : MonoBehaviour 
	{
		public Game game;

		public ScoreManager scoreManager;

		public SoundManager soundManager;

		static SceneManagers instance;

		public static SceneManagers Instance
		{
			get
			{
				return instance;
			}
		}

		void Awake()
		{
			if(instance == null)
			{
				instance = this;
			}
			else
			{
				Destroy(gameObject);
			}
		}

		void OnDestroy()
		{
			if(instance == this)
			{
				instance = null;
			}
		}
	}
}
