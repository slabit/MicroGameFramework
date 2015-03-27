using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace OneButtonPong
{
	[AddComponentMenu("OneButtonPong/GameBehaviour")]
	public abstract class GameBehaviour : MonoBehaviour 
	{
		bool awaken;

		bool started;

		public bool FirstLevelStart
		{
			get
			{
				return Game.FirstLevelStart;
			}
		}

		public bool Started
		{
			get
			{
				return started;
			}
		}

		public bool Awaken
		{
			get
			{
				return awaken;
			}
		}

		protected Game Game
		{
			get
			{
				if(SceneManagers.Instance == null)
					return null;

				return SceneManagers.Instance.game;
			}
		}

		public void NotifyStartLevel()
		{
			OnStartLevel();
		}

		protected virtual void OnStartLevel()
		{
		}

		protected virtual void OnAwake()
		{
		}

		protected virtual void OnAwakeEnd()
		{
		}

		protected virtual void OnStart()
		{
		}
		
		protected virtual void OnStartEnd()
		{
		}

		void Awake()
		{			
			if(awaken)
				return;

			if(Application.isPlaying)
			{
				Game.Register(this);
			}

			OnAwake();
			awaken = true;
			if(Application.isPlaying)
			{
				if(Game.LevelStarted)
				{
					OnStartLevel();
				}
			}
		}

		void Start()
		{
			if(started)
				return;

			OnStart();
			started = true;
		}
		
		void OnDestroy()
		{
			if(started)
			{
				OnStartEnd();
				started = false;
			}

			if(awaken)
			{
			
				if(Game != null)
				{
					Game.Unregister(this);
				}

				OnAwakeEnd();
				awaken = false;
			}
		}
	}
}