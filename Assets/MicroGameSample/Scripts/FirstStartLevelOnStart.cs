using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace OneButtonPong
{
	[AddComponentMenu("OneButtonPong/FirstStartLevelOnStart")]
	public class FirstStartLevelOnStart : MonoBehaviour 
	{
		void Start()
		{
			SceneManagers.Instance.game.NotifyFirstStartLevel();
		}
	}
}
