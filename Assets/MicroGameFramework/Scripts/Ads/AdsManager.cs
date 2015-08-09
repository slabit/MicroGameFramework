using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using GoogleMobileAds;
using GoogleMobileAds.Api;

namespace MicroGameFramework
{
	[AddComponentMenu("MicroGameFramework/AdsManager")]
	public class AdsManager : MonoBehaviour
	{
		public string adMob_android_Banner = "ca-app-pub-9805242932080290/4419903160";
		
		public string adMob_android_Interstitial = "ca-app-pub-9805242932080290/5896636365";
		
		BannerView adMob_BannerView;
		InterstitialAd adMob_Interstitial;
		
		static AdsManager instance;
		
		public static AdsManager Instance
		{
			get
			{
				return instance;
			}
		}
		
		public void ShowInterstitial()
		{
			Debug.Log("Try Show Interstitial");
			
			if (adMob_Interstitial.IsLoaded())
	        {
				Debug.Log("Show Interstitial");
				adMob_Interstitial.Show();
				
				Debug.Log("Request new interstitial");
				adMob_Interstitial.LoadAd(AdMob_CreateAdRequest());
	        }
	        else
	        {
	            print("Interstitial is not ready yet.");
	        }
		}
				
		void Awake()
		{
			if(instance == null)
			{
				instance = this;
			}
			else
			{
				Destroy(gameObject);
			}
			
			AdMob_CreateBanner();
			AdMob_CreateInterstitial();
		}
		
		void AdMob_CreateBanner()
		{
			adMob_BannerView = new BannerView(adMob_android_Banner, AdSize.SmartBanner, AdPosition.Bottom);
			adMob_BannerView.LoadAd(AdMob_CreateAdRequest());
		}
		
		public AdRequest AdMob_CreateAdRequest()
		{
			return new AdRequest.Builder()
				.AddTestDevice(AdRequest.TestDeviceSimulator)
					.AddTestDevice("7D87C54ED341E851A7DA7E4E5CAE6313")
					.AddKeyword("game")
					.SetGender(Gender.Male)
					.SetBirthday(new DateTime(1985, 1, 1))
					.TagForChildDirectedTreatment(false)
					.AddExtra("color_bg", "9B30FF")
					.Build();
		}
		
		public void AdMob_CreateInterstitial()
	    {
			adMob_Interstitial = new InterstitialAd(adMob_android_Interstitial);
			adMob_Interstitial.LoadAd(AdMob_CreateAdRequest());
	    }
	}
}