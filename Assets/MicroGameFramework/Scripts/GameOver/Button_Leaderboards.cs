using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MicroGameFramework
{
	[AddComponentMenu("MicroGameFramework/Button_Leaderboards")]
	public class Button_Leaderboards : MonoBehaviour
	{
		public void OnClick()
		{
			LeaderboardManager.Instance.ShowLeaderboard();
		}
	}
}
