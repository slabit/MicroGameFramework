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
	[AddComponentMenu("GK/Facebook/gkFacebookManager")]
	public class gkFacebookManager : MonoBehaviour
	{	
		public string facebookPageID = "503287153144438";

		public string facebookPageName = "ketchappgames";

		static private gkFacebookManager instance;
		
		static public gkFacebookManager Instance
		{
			get
			{
				return instance;
			}
		}

		public void OpenFacebookPage()
		{
			gkFacebookUtility.OpenFacebook(facebookPageID, facebookPageName);
		}
		
		void Awake()
		{
			if(instance == null)
			{
				instance = this;
			}
			else
			{
				Debug.LogWarning("A singleton can only be instantiated once!");
				Destroy(gameObject);
				return;
			}
		}
	}
}
#endif