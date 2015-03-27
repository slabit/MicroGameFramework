using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace gk
{
	[AddComponentMenu("GK/Singleton/gkSingleton_DontDestroyOnLoad")]
	public static class gkSingleton
	{	
		public static void DontDestroyOnLoad(MonoBehaviour a_rMonoBehaviour)
		{
			DontDestroyOnLoad(a_rMonoBehaviour.gameObject);
		}
		
		public static void DontDestroyOnLoad(GameObject a_rGameObject)
		{
			gkSingleton_DontDestroyOnLoad rDontDestroyOnLoad = a_rGameObject.GetComponent<gkSingleton_DontDestroyOnLoad>();
			if(rDontDestroyOnLoad == null)
			{
				rDontDestroyOnLoad = a_rGameObject.AddComponent<gkSingleton_DontDestroyOnLoad>();
			}
		}
	}
}