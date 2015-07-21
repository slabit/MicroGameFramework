using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace MicroGameFramework
{
	[AddComponentMenu("MicroGameFramework/GameOverScreenScoreCounterDisplay")]
	public class GameOverScreenScoreCounterDisplay : MonoBehaviour
	{
		public UnityEngine.UI.Text uiText;

		void OnEnable()
		{
			uiText.text = ScoreManager.Instance.Score.ToString();
		}
	}
}