#if UseGKExtensions
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine.Cloud.Analytics;

using gk;

namespace gk
{
	[AddComponentMenu("GK/Sharing/gkSharingManager")]
	public class gkSharingManager : MonoBehaviour
	{	
		GeneralSharing generalSharingComponent;

		static private gkSharingManager instance;
		
		static public gkSharingManager Instance
		{
			get
			{
				return instance;
			}
		}

		public void Share(string title, string message, Texture2D image)
		{
			generalSharingComponent.Share(title, message, image);
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

			generalSharingComponent = gameObject.AddComponent<GeneralSharing>();
		}
	}
}
#endif