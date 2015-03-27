using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace gk
{
	[AddComponentMenu("GK/Singleton/gkSingleton_DontDestroyOnLoad")]
	public class gkSingleton_DontDestroyOnLoad : MonoBehaviour
	{
		private void Awake()
		{
			DontDestroyOnLoad(gameObject);
		}
	}
}