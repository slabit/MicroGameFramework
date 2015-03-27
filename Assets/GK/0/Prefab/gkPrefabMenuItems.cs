#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace gk
{
	public class gkPrefabMenuItems : Editor
	{
		[MenuItem("GK/Disconnect Prefab Instance")]
		private static void DoDisconnectPrefabInstance()
		{
			foreach(GameObject rSelectedGameObject in Selection.gameObjects)
			{
				PrefabUtility.DisconnectPrefabInstance(rSelectedGameObject);
			}
		}
	}
}
#endif