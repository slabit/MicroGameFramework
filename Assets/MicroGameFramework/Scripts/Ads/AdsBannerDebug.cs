using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace MicroGameFramework
{
	[AddComponentMenu("MicroGameFramework/AdsBannerDebug")]
	public class AdsBannerDebug : MonoBehaviour
	{
		void Awake()
		{
			#if (!UNITY_EDITOR && !adsBannerDebug) || removeAdsBannerInEditor
			Destroy(gameObject);
			#endif
		}
	}
}