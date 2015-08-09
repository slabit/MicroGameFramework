using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MicroGameFramework
{
	[AddComponentMenu("MicroGameFramework/Button_Achievements")]
	public class Button_Achievements : MonoBehaviour
	{
		public void OnClick()
		{
			LeaderboardManager.Instance.ShowAchievements();
		}
	}
}
