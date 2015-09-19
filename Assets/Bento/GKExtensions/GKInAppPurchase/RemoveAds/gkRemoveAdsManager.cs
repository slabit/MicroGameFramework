#if UseGKExtensions
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[AddComponentMenu("GK/RemoveAds/gkRemoveAdsManager")]
	public class gkRemoveAdsManager : MonoBehaviour
	{		
		public static Action onRemoveAds;
		
		public string removeAdsUnityProductID = "RemoveAds";
		
		private bool m_bAdsRemoved;
	
		static private gkRemoveAdsManager ms_oInstance;
		
		static private string mc_oAdsRemovedKey = "AdsRemoved";
		
		#if removeAds 
		private static bool ms_bDebugForceRemoveAds = true;
		#else
		private static bool ms_bDebugForceRemoveAds = false;
		#endif
		
		static public gkRemoveAdsManager Instance
		{
			get
			{
				return ms_oInstance;
			}
		}
		
		public bool AdsRemoved
		{
			get
			{
				return m_bAdsRemoved || ms_bDebugForceRemoveAds;
			}
		}
		
		public void TryToRemoveAds()
		{
			if(m_bAdsRemoved == false)
			{
				gkInAppPurchaseManager.Instance.TryPurchase(removeAdsUnityProductID, OnPurchaseResult);
			}
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
			
			InitializeAdsRemoved();
		}
		
		private void InitializeAdsRemoved()
		{
			m_bAdsRemoved = (PlayerPrefs.GetInt(mc_oAdsRemovedKey, 0) == 1);
			if(m_bAdsRemoved)
			{
				OnRemoveAd();
			}
		}
		
		private void OnPurchaseResult(bool a_bSuccess)
		{
			if(a_bSuccess)
			{
				RemoveAds();
			}
		}
		
		private void RemoveAds()
		{
			m_bAdsRemoved = true;
			PlayerPrefs.SetInt(mc_oAdsRemovedKey, 1);
			OnRemoveAd();
		}
		
		private void OnRemoveAd()
		{
			if(onRemoveAds != null)
			{
				onRemoveAds();
			}
		}
	}
}
#endif