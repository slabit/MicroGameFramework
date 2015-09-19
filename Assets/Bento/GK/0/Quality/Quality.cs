using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace gk
{
	public static class Quality
	{	
		#if UNITY_ANDROID
		static bool lowEndOverride;
		#endif

		public static bool HighEnd
		{
			get
			{
				return !LowEnd;
			}

			set
			{
				LowEnd = !value;
			}
		}

		public static bool LowEnd
		{
			get
			{
				#if UNITY_IOS
				if(iPhone.generation == iPhoneGeneration.iPhone
				   ||
				   iPhone.generation == iPhoneGeneration.iPhone3G
				   ||
				   iPhone.generation == iPhoneGeneration.iPhone3GS
				   ||
				   iPhone.generation == iPhoneGeneration.iPhone4
				   ||
				   iPhone.generation == iPhoneGeneration.iPodTouch1Gen
				   ||
				   iPhone.generation == iPhoneGeneration.iPodTouch2Gen
				   ||
				   iPhone.generation == iPhoneGeneration.iPodTouch3Gen
				   ||
				   iPhone.generation == iPhoneGeneration.iPodTouch4Gen
				   ||
				   iPhone.generation == iPhoneGeneration.iPodTouch5Gen
				   ||
				   iPhone.generation == iPhoneGeneration.iPad1Gen
				   )
				{
					return true;
				}
				#elif UNITY_ANDROID
				if(lowEndOverride)
				{
					return true; 
				}
				#endif

				return false;
			}

			set
			{
				#if UNITY_ANDROID
				UnityEngine.Debug.Log(value);
				lowEndOverride = value;
				#endif
			}
		}
	}
}