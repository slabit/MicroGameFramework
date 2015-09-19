using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[AddComponentMenu("GK/InAppPurchase/gkNativePopUpManager")]
	public class gkNativePopUpManager : MonoBehaviour
	{	
		private Action<bool> m_rOnPopUpResult;
		
		private string m_oCancelButton;

		static private gkNativePopUpManager ms_oInstance;
		
		static public gkNativePopUpManager Instance
		{
			get
			{
				if(ms_oInstance == null)
				{
					gkComponentBuilderUtility.BuildComponent(typeof(gkNativePopUpManager));
				}
				
				return ms_oInstance;
			}
		}
	
		public void ShowPopUp(string a_oTitle, string a_oMessage)
		{
			ShowPopUp(a_oTitle, a_oMessage, "Ok", null);
		}
			
		public void ShowPopUp(string a_oTitle, string a_oMessage, string a_oValidateButton, Action<bool> a_rOnPopUpResult)
		{
			gkTime.Pause(true, true);
			
			Debug.Log("Show Pop Up : " + a_oTitle + ", " + a_oMessage + ", " + a_oValidateButton);
			
			m_rOnPopUpResult = a_rOnPopUpResult;
			m_oCancelButton = "";
			
			#if UNITY_EDITOR || !UseGKExtensions
			OnPopUpResult(true);
			#elif UNITY_IPHONE
			EtceteraBinding.showAlertWithTitleMessageAndButtons(a_oTitle, a_oMessage, new string[]{a_oValidateButton});
			#elif UNITY_ANDROID
			EtceteraAndroid.showAlert(a_oTitle, a_oMessage, a_oValidateButton);
			#else
			OnPopUpResult(true);
			#endif
		}
		
		public void ShowPopUp(string a_oTitle, string a_oMessage, string a_oValidateButton, string a_oCancelButton, Action<bool> a_rOnPopUpResult)
		{
			gkTime.Pause(true, true);
			
			Debug.Log("Show Pop Up : " + a_oTitle + ", " + a_oMessage + ", " + a_oValidateButton + ", " + a_oCancelButton);
			
			m_rOnPopUpResult = a_rOnPopUpResult;
			m_oCancelButton = a_oCancelButton;
			
			#if UNITY_EDITOR || !UseGKExtensions
			OnPopUpResult(true);
			#elif UNITY_IPHONE
			EtceteraBinding.showAlertWithTitleMessageAndButtons(a_oTitle, a_oMessage, new string[]{a_oCancelButton, a_oValidateButton});
			#elif UNITY_ANDROID
			EtceteraAndroid.showAlert(a_oTitle, a_oMessage, a_oCancelButton, a_oValidateButton);
			#else
			OnPopUpResult(true);
			#endif
		}
		
		private void Awake()
		{
			if(ms_oInstance == null)
			{
				ms_oInstance = this;
			}
			else
			{
				Debug.LogWarning("A singleton can only be instantiated once!");
				Destroy(gameObject);
				return;
			}
			
			Initialize();
		}
		
		private void OnDestroy()
		{
			Terminate();
		}
		
		private void Initialize()
		{
			#if !UseGKExtensions
			#elif UNITY_IPHONE
			EtceteraManager.alertButtonClickedEvent += OnClick;
			#elif UNITY_ANDROID
			EtceteraAndroidManager.alertButtonClickedEvent += OnClick;
			#endif
		}
		
		private void Terminate()
		{
			#if !UseGKExtensions
			#elif UNITY_IPHONE
			EtceteraManager.alertButtonClickedEvent -= OnClick;
			#elif UNITY_ANDROID
			EtceteraAndroidManager.alertButtonClickedEvent -= OnClick;
			#endif
		}
	
		private void OnPopUpResult(bool a_bSuccess)
		{
			Debug.Log("Pop up result : " + a_bSuccess);
			if(m_rOnPopUpResult != null)
			{
				m_rOnPopUpResult(a_bSuccess);
				m_rOnPopUpResult = null;
			}
			
			gkTime.Pause(false, true);
		}
		
		private void OnClick(string a_oButton)
		{
			if(a_oButton == m_oCancelButton)
			{
				OnPopUpResult(false);
			}
			else
			{
				OnPopUpResult(true);
			}
		}	
	}
}