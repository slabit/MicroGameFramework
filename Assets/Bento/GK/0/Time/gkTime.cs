using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[AddComponentMenu("GK/Time/gkTime")]
	public class gkTime : MonoBehaviour
	{	
		public static string mc_oDefaultPauseName = "";
		
		public static Action<bool> onPause; 
		
		public float maxDeltaTime = 0.1f;
		
		private float m_fDeltaTime; 
		
		private float m_fLastUpdateRealTime;
		
		private List<string> m_oPauseNamesStack = new List<string>();
		
		private bool m_bPaused;
		
		static private gkTime ms_oInstance;
		
		static private bool ms_bApplicationEnd;
		
		public static float DeltaTime
		{
			get
			{
				if(Time.deltaTime == 0.0f)
				{
					TryCreateInstanceIfNeeded();
					if(ms_oInstance == null)
					{
						return Time.deltaTime;
					}
					
					return ms_oInstance.m_fDeltaTime;
				}
				else
				{
					return Time.deltaTime;
				}
			}
		}
		
		public static bool Paused
		{
			get
			{
				if(Application.isPlaying == false)
					return false;

				TryCreateInstanceIfNeeded();
				if(ms_oInstance == null)
				{
					return false;
				}
				
				return ms_oInstance.m_bPaused;
			}
			
			set
			{	
				Pause(mc_oDefaultPauseName, value);
			}
		}
		
		private static void TryCreateInstanceIfNeeded()
		{
			if(ms_bApplicationEnd)
			{
				return;
			}
			
			if(ms_oInstance != null)
			{
				return;
			}
			
			//Debug.Log("Create");
			gkComponentBuilderUtility.BuildComponent<gkTime>();
		}
		
		void Awake()
		{
			if(ms_oInstance == null)
			{
				ms_oInstance = this;
			}
			else
			{
				Debug.LogWarning("A singleton can only be instantiated once!");
				Destroy(gameObject);
			}
		}
		
		private void OnDestroy()
		{
			Time.timeScale = 1.0f;
		}
		
		private void OnApplicationQuit()
		{
			ms_bApplicationEnd = true;
		}
		
		private void Update()
		{
			if(m_bPaused)
			{
				Time.timeScale = 0.0f;
				m_fDeltaTime = Time.realtimeSinceStartup - m_fLastUpdateRealTime;
			}
			else
			{
				m_fDeltaTime = Time.deltaTime;
			}
			if(m_fDeltaTime > maxDeltaTime)
			{
				m_fDeltaTime = 	maxDeltaTime;
			}
			m_fLastUpdateRealTime = Time.realtimeSinceStartup;
		}
		
		static public bool ContainsTheCurrentPauseName(List<string> a_rPauseNames)
		{
			TryCreateInstanceIfNeeded();
			if(ms_oInstance == null)
			{
				return false;
			}
			
			return ms_oInstance._ContainsTheCurrentPauseName(a_rPauseNames);
		}
		
		static public bool IsTheCurrentPauseName(string a_rPauseName)
		{
			TryCreateInstanceIfNeeded();
			if(ms_oInstance == null)
			{
				return false;
			}
			
			return ms_oInstance._IsTheCurrentPauseName(a_rPauseName);
		}
		
		static public void Pause(bool a_bPause, bool a_bStackUpSamePause = false)
		{
			Pause(mc_oDefaultPauseName, a_bPause, a_bStackUpSamePause);
		}
		
		static public void Pause(string a_rPauseName, bool a_bPause, bool a_bStackUpSamePause = false)
		{
			TryCreateInstanceIfNeeded();
			if(ms_oInstance == null)
			{
				return;
			}
			
			ms_oInstance._Pause(a_rPauseName, a_bPause, a_bStackUpSamePause);
		}
		
		private bool _ContainsTheCurrentPauseName(List<string> a_rPauseNames)
		{
			if(m_bPaused)
			{
				return a_rPauseNames.Contains(GetCurrentPauseName());
			}
			else
			{
				return false;
			}
		}
		
		private bool _IsTheCurrentPauseName(string a_rPauseName)
		{
			if(m_bPaused)
			{
				return GetCurrentPauseName() == a_rPauseName;
			}
			else
			{
				return false;
			}
		}
		
		private string GetCurrentPauseName()
		{
			return m_oPauseNamesStack[m_oPauseNamesStack.Count - 1];
		}
		
		
		private void _Pause(string a_rPauseName, bool a_bPause, bool a_bStackUpSamePause)
		{
			bool bIgnoreAction = false;
			if(a_bPause)
			{
				if(a_bStackUpSamePause == false)
				{
					// Check if the pause is already in the stack and remove it if it the case
					int iPauseIndex = m_oPauseNamesStack.FindLastIndex(pauseName => pauseName == a_rPauseName);
					if(iPauseIndex != -1)
					{
						m_oPauseNamesStack.RemoveAt(iPauseIndex);
					}
				}
						
				// Add a pause
				m_oPauseNamesStack.Add(a_rPauseName);
				m_bPaused = true;
			}
			else
			{
				// Remove the last pause that have been added with this name
				int iPauseIndex = m_oPauseNamesStack.FindLastIndex(pauseName => pauseName == a_rPauseName);
				if(iPauseIndex != -1)
				{
					m_oPauseNamesStack.RemoveAt(iPauseIndex);
				}
				else
				{
					bIgnoreAction = true;
				}
				
				// If there is no pause name left
				if(m_oPauseNamesStack.Count == 0)
				{
					m_bPaused = false;
				}
			}
			
			if(bIgnoreAction == false)
			{
				OnPause(m_bPaused);
			}
		}
		
		private void OnPause(bool a_bPause)
		{
			if(a_bPause)
			{
				Time.timeScale = 0.0f;	
			}
			else
			{
				Time.timeScale = 1.0f;
			}
			
			if(onPause != null)
			{
				onPause(a_bPause);
			}
		}
	}
}