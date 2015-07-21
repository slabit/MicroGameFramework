using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MicroGameFramework
{
	[AddComponentMenu("MicroGameFramework/Game")]
	public class Game : MonoBehaviour 
	{
		public System.Action onStartLevel;

		public System.Action onGameOver;

		List<GameBehaviour> gameBehaviours = new List<GameBehaviour>();

		bool loopingThroughGameBehaviours;

		List<GameBehaviour> gamebehavioursToUnregister = new List<GameBehaviour>();
		List<GameBehaviour> gamebehavioursToRegister = new List<GameBehaviour>();

		bool firstLevelStart = true;

		bool levelStarted;

		bool gameOver;

		static Game instance;

		public static Game Instance
		{
			get
			{
				return instance;
			}
		}

		public bool FirstLevelStart
		{
			get
			{
				return firstLevelStart;
			}
		}

		public bool LevelStarted
		{
			get
			{
				return levelStarted;
			}
		}
		
		public void NotifyGameOver()
		{
			if(gameOver)
				return;
			
			gameOver = true;
			
			if(onGameOver != null)
				onGameOver();
		}

		public void Register(GameBehaviour gameBehaviour)
		{
			if(loopingThroughGameBehaviours)
			{
				gamebehavioursToRegister.Add(gameBehaviour);
				return;
			}

			gameBehaviours.Add(gameBehaviour);
		}

		public void Unregister(GameBehaviour gameBehaviour)
		{
			if(loopingThroughGameBehaviours)
			{
				//Debug.Log("Want to unregister : " + gameBehaviour);
				gamebehavioursToUnregister.Add(gameBehaviour);
				return;
			}

			gameBehaviours.Remove(gameBehaviour);
		}

		public void NotifyFirstStartLevel()
		{
			StartLevel();
		}

		public void Restart()
		{
			StartLevel();
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

		void StartLevel()
		{
			gameOver = false;
			levelStarted = true;

			loopingThroughGameBehaviours = true;
			foreach(GameBehaviour gameBehaviour in gameBehaviours)
				gameBehaviour.NotifyStartLevel();
			loopingThroughGameBehaviours = false;
			DelayedRegister();

			if(onStartLevel != null)
			{
				onStartLevel();
			}
				
			firstLevelStart = false;
		}

		void DelayedRegister()
		{
			foreach(GameBehaviour gameBehaviour in gamebehavioursToRegister)
			{
				gameBehaviours.Add(gameBehaviour);
			}
			gamebehavioursToRegister.Clear();

			foreach(GameBehaviour gameBehaviour in gamebehavioursToUnregister)
			{
				gameBehaviours.Remove(gameBehaviour);
			}
			gamebehavioursToUnregister.Clear();
		}
	}
}
