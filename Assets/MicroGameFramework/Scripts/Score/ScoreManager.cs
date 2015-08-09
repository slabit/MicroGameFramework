using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace MicroGameFramework
{
	[AddComponentMenu("MicroGameFramework/ScoreManager")]
	public class ScoreManager : GameBehaviour
	{
		public Action onScoreChange;

		int score;

		string bestScoreKeyName = "BestScore";

		static ScoreManager instance;
		
		public static ScoreManager Instance
		{
			get
			{
				return instance;
			}
		}

		public int Score
		{
			get
			{
				return score;
			}
		}

		public int BestScore
		{
			get
			{
				return PlayerPrefs.GetInt(bestScoreKeyName, 0);
			}

			set
			{
				PlayerPrefs.SetInt(bestScoreKeyName, value);
			}
		}

		public void IncrementScore()
		{
			if(Game.GameOver)
				return;
				
			++score;

			if(score > BestScore)
			{
				BestScore = score;
			}
			OnScoreChange();
		}
		
		protected override void OnAwake()
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
		
		protected override void OnAwakeEnd()
		{
			if(instance == this)
			{
				instance = null;
			}
		}

		protected override void OnStartLevel()
		{
			score = 0;
			OnScoreChange();
		}

		void OnScoreChange()
		{
			if(onScoreChange != null)
			{
				onScoreChange();
			}
		}
	}
}