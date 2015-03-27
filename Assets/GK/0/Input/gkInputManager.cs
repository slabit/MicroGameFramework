using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace gk
{
	[AddComponentMenu("GK/Input/gkInputManager")]
	public class gkInputManager : MonoBehaviour
	{	
		public List<gkInputMappingScheme> inputMappingSchemes;
		
		private Dictionary<string, gkInputMappingScheme> m_oInputMappingSchemes = new Dictionary<string, gkInputMappingScheme>();
		
		private HashSet<gkInputMappingScheme> m_oCurrentInputMappingSchemes = new HashSet<gkInputMappingScheme>();
		
		private static HashSet<string> ms_oActiveInputMappingSchemeNames = new HashSet<string>();
		
		private static gkInputManager ms_oInstance;

		
		static public gkInputManager Instance
		{
			get
			{
				return ms_oInstance;
			}
		}
		
		public static void ActivateScheme(string a_oSchemeName, bool a_bActivate = true)
		{
			if(a_bActivate)
			{
				if(ms_oActiveInputMappingSchemeNames.Add(a_oSchemeName))
				{
					if(ms_oInstance != null)
					{
						ms_oInstance.AddToActiveScheme(a_oSchemeName);
					}
				}
			}
			else
			{
				if(ms_oActiveInputMappingSchemeNames.Remove(a_oSchemeName))
				{
					if(ms_oInstance != null)
					{
						ms_oInstance.RemoveFromActiveScheme(a_oSchemeName);
					}
				}
			}
		}
		
		public static bool GetButtonDown(string a_oButtonName)
		{
			foreach(gkInputMappingScheme rInputMappingScheme in ms_oInstance.m_oCurrentInputMappingSchemes)
			{
				if(rInputMappingScheme.GetButtonDown(a_oButtonName))
				{
					return true;
				}
			}
			return false;
		}
		
		public static bool GetButton(string a_oButtonName)
		{
			foreach(gkInputMappingScheme rInputMappingScheme in ms_oInstance.m_oCurrentInputMappingSchemes)
			{
				if(rInputMappingScheme.GetButton(a_oButtonName))
				{
					return true;
				}
			}
			return false;
		}
		
		#if UNITY_EDITOR
		public void EditorOnly_AutoFill()
		{
			if(inputMappingSchemes == null)
			{
				inputMappingSchemes = new List<gkInputMappingScheme>();
			}
			else
			{
				inputMappingSchemes.Clear();
			}
			
			inputMappingSchemes.AddRange(GetComponentsInChildren<gkInputMappingScheme>());
			foreach(gkInputMappingScheme rInputMappingScheme in inputMappingSchemes)
			{
				rInputMappingScheme.EditorOnly_AutoFill();
			}
		}
		#endif
		
		private void Awake()
		{
			if(ms_oInstance == null)
			{
				ms_oInstance = this;
			}
			else
			{
				UnityEngine.Debug.LogWarning("A singleton can only be instantiated once!");
				Destroy(gameObject);
				return;
			}
			
			GetInputMappings();
			SetActiveSchemes(ms_oActiveInputMappingSchemeNames);
		}
		
		private void GetInputMappings()
		{
			foreach(gkInputMappingScheme rInputMappingScheme in inputMappingSchemes)
			{
				m_oInputMappingSchemes.Add(rInputMappingScheme.name, rInputMappingScheme);
			}
		}
		
		private void SetActiveSchemes(IEnumerable<string> a_rSchemeNames)
		{
			m_oCurrentInputMappingSchemes.Clear();
			
			foreach(string oSchemeName in a_rSchemeNames)
			{
				AddToActiveScheme(oSchemeName);
			}
		}
		
		private void AddToActiveScheme(string a_oSchemeName)
		{
			gkInputMappingScheme rInputMappingScheme;
			if(m_oInputMappingSchemes.TryGetValue(a_oSchemeName, out rInputMappingScheme))
			{
				m_oCurrentInputMappingSchemes.Add(rInputMappingScheme);
			}
		}
		
		private void RemoveFromActiveScheme(string a_oSchemeName)
		{
			gkInputMappingScheme rInputMappingScheme;
			if(m_oInputMappingSchemes.TryGetValue(a_oSchemeName, out rInputMappingScheme))
			{
				m_oCurrentInputMappingSchemes.Remove(rInputMappingScheme);
			}
		}
	}
}