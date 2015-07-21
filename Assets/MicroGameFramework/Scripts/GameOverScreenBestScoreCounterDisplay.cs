using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace MicroGameFramework
{
	[AddComponentMenu("MicroGameFramework/GameOverScreenBestScoreCounterDisplay")]
	public class GameOverScreenBestScoreCounterDisplay : MonoBehaviour
	{
		public UnityEngine.UI.Text uiText;

		void OnEnable()
		{
			uiText.text = ScoreManager.Instance.BestScore.ToString();
		}
	}
}