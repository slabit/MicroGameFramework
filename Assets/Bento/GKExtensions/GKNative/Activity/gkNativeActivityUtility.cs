#if UseGKExtensions
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	public static class gkNativeActivityUtility
	{		
		public static void ShowActivityView(bool a_bShow)
		{
			
			if(a_bShow)
			{	
				#if UNITY_IPHONE
				gkTime.Pause(true, true);
				EtceteraBinding.showActivityView();
				#elif UNITY_ANDROID
				//EtceteraAndroid.showProgressDialog("", "Loading...");
				#endif
			}
			else
			{
				#if UNITY_IPHONE
				EtceteraBinding.hideActivityView();
				gkTime.Pause(false, true);
				#elif UNITY_ANDROID
				//EtceteraAndroid.hideProgressDialog();
				#endif
			}
		}
	}
}
#endif