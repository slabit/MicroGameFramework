#if UseGKExtensions
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[AddComponentMenu("GK/RemoveAds/gkRemoveAdsButton")]
	public class gkRemoveAdsButton : MonoBehaviour
	{		
		public gkButton button;
		
		private void Awake()
		{
			gkRemoveAdsManager.onRemoveAds += OnRemoveAds;
			button.onClick += OnClick;
			
			if(gkRemoveAdsManager.Instance.AdsRemoved)
			{
				OnRemoveAds();
			}
		}
		
		private void OnDestroy()
		{
			button.onClick -= OnClick;
			gkRemoveAdsManager.onRemoveAds -= OnRemoveAds;
		}
		
		private void OnClick()
		{
			gkRemoveAdsManager.Instance.TryToRemoveAds();
		}
		
		private void OnRemoveAds()
		{
			Destroy(gameObject);
		}
	}
}
#endif