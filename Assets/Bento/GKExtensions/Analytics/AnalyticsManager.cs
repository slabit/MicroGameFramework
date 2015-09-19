#if UseGKExtensions
using UnityEngine;
using System.Collections;
using UnityEngine.Cloud.Analytics;
using System.Collections.Generic;

namespace gk
{
	public class AnalyticsManager : MonoBehaviour 
	{	
		public string unityAnalitycsProjectId;

		public static void CustomEvent(string customEventName, IDictionary<string, object> eventData)
		{
			string customEventLog = "Analytics : CustomEvent : " + customEventName + " : ";
			#if UNITY_EDITOR
			foreach(string key in eventData.Keys)
			{
				customEventLog += " [ " + key + " : " + eventData[key] + " ] ";
			}
			#endif

			Debug.Log(customEventLog);

			UnityEngine.Cloud.Analytics.UnityAnalytics.CustomEvent(customEventName, eventData);
		}

		void Start() 
		{	
			UnityAnalytics.StartSDK(unityAnalitycsProjectId);
		}
	}
}
#endif