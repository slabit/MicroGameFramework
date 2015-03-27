using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace OneButtonPong
{
	[AddComponentMenu("OneButtonPong/GameOverScreenScoreCounterDisplay")]
	public class GameOverScreenScoreCounterDisplay : MonoBehaviour
	{
		public UnityEngine.UI.Text uiText;

		ScoreManager ScoreManager
		{
			get
			{
				if(SceneManagers.Instance == null)
					return null;
				
				return SceneManagers.Instance.scoreManager;
			}
		}

		void OnEnable()
		{
			uiText.text = ScoreManager.Score.ToString();
		}
	}
}