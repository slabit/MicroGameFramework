#if UseGKExtensions
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine.Cloud.Analytics;

using gk;
using System.Runtime.InteropServices;

namespace gk
{
	public static class gkFacebookUtility
	{	
		#if UNITY_ANDROID
		public static void _OpenFacebook(string facebookPageID, string facebookPageName)
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("OpenFacebookBinding");
			androidJavaClass.CallStatic("_OpenFacebook", new object[]{facebookPageID, facebookPageName});
		}
		#elif UNITY_IPHONE
		[DllImport ("__Internal")]
		public static extern void _OpenFacebook(string facebookPageID, string facebookPageName);
		#else
		public static void _OpenFacebook(string facebookPageID, string facebookPageName)
		{
			Debug.Log("OpenFacebook Dummy Call: [id: " + facebookPageID + ", name: " + facebookPageName + "]");
		}
		#endif
		public static void OpenFacebook(string facebookPageID, string facebookPageName)
		{
			Debug.Log("OpenFacebook : [id: " + facebookPageID + ", name: " + facebookPageName + "]");
			_OpenFacebook(facebookPageID, facebookPageName);
		}
	}
}
#endif