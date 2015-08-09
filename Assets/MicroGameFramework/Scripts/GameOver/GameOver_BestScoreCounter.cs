using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace MicroGameFramework
{
	[AddComponentMenu("MicroGameFramework/GameOver_BestScoreCounter")]
	public class GameOver_BestScoreCounter : MonoBehaviour
	{
		public UnityEngine.UI.Text uiText;

		void OnEnable()
		{
			uiText.text = ScoreManager.Instance.BestScore.ToString();
		}
	}
}