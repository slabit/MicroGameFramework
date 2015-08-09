using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using GoogleMobileAds;
using GoogleMobileAds.Api;

namespace MicroGameFramework
{
	[AddComponentMenu("MicroGameFramework/InterstitialAdsController")]
	public class InterstitialAdsController : GameBehaviour
	{
		public int deathCountBeforeDisplay = 7;
		
		public float minimumTimeBetweenDisplays = 60.0f;
		
		DateTime lastDisplayTime;
		
		int deathCountSinceLastDisplay;
		
		protected override void OnAwake()
		{
			Game.onGameOver += OnGameOver;
			
			StartWait();
		}
		
		protected override void OnAwakeEnd()
		{
			if(Game != null)
				Game.onGameOver -= OnGameOver;
		}
		
		void OnGameOver()
		{
			++deathCountSinceLastDisplay;
			if(deathCountSinceLastDisplay >= deathCountBeforeDisplay)
			{
				float timeElapsedSinceLastDisplay = (float)((DateTime.Now - lastDisplayTime).TotalSeconds);
				if(timeElapsedSinceLastDisplay >= minimumTimeBetweenDisplays)
				{
					AdsManager.Instance.ShowInterstitial();
					StartWait();
				}
			}
		}
		
		void StartWait()
		{
			lastDisplayTime = DateTime.Now;
			deathCountSinceLastDisplay = 0;
		}
	}
}