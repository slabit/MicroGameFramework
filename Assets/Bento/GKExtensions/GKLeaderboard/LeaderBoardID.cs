using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class LeaderBoardID
{	
	public string unityID;
	
	public string iosID;
	
	public string androidID;
	
	public string CurrentPlatformID
	{
		get
		{
			#if UNITY_EDITOR
			return unityID;
			#elif UNITY_IPHONE
			return iosID;
			#elif UNITY_ANDROID
			return androidID;
			#else
			return unityID;
			#endif
		}
	}
}