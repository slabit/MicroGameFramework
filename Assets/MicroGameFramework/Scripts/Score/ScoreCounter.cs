using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace MicroGameFramework
{
	[AddComponentMenu("MicroGameFramework/ScoreCounter")]
	public class ScoreCounter : GameBehaviour
	{
		public UnityEngine.UI.Text uiText;
		
		protected override void OnAwake()
		{
			ScoreManager.Instance.onScoreChange += OnScoreChange;
		}

		protected override void OnAwakeEnd()
		{
			if(ScoreManager.Instance != null)
				ScoreManager.Instance.onScoreChange -= OnScoreChange;
		}

		void OnScoreChange()
		{
			uiText.text = ScoreManager.Instance.Score.ToString();
		}
	}
}