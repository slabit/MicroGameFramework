#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace gk
{
public class gkPlayerPrefsMenuItems : Editor
	{
		[MenuItem("GK/Delete All player prefs")]
		private static void DoDeleteAllPlayerPrefs()
		{
			PlayerPrefs.DeleteAll();
		}
	}
}
#endif