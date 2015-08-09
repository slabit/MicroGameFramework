using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MicroGameFramework
{
	[AddComponentMenu("MicroGameFramework/FirstStartLevelOnStart")]
	public class FirstStartLevelOnStart : MonoBehaviour 
	{
		void Start()
		{
			Game.Instance.NotifyFirstStartLevel();
		}
	}
}
