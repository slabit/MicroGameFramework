using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace OneButtonPong
{
	[AddComponentMenu("OneButtonPong/ScoreManager")]
	public class ScoreManager : GameBehaviour
	{
		public Action onScoreChange;

		public bool incrementScoreFasterForTest;

		int score;

		string bestScoreKeyName = "BestScore";

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
			++score;
			if(incrementScoreFasterForTest)
			{
				score += score * 10;
			}

			if(score > BestScore)
			{
				BestScore = score;
			}
			OnScoreChange();
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