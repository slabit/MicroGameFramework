using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace gk
{
	[AddComponentMenu("GK/Input/gkInputMappingScheme")]
	public class gkInputMappingScheme : MonoBehaviour
	{	
		public List<gkInputButtonMappingList> inputButtonMappingLists;
		
		private Dictionary<string, gkInputButtonMappingList> m_oInputButtonMappingLists = new Dictionary<string, gkInputButtonMappingList>();
		
		public bool GetButtonDown(string a_oButtonName)
		{
			gkInputButtonMappingList rInputButtonMappingList;
			if(m_oInputButtonMappingLists.TryGetValue(a_oButtonName, out rInputButtonMappingList))
			{
				return rInputButtonMappingList.GetButtonDown();
			}
			else
			{
				return false;
			}
		}
		
		public bool GetButton(string a_oButtonName)
		{
			gkInputButtonMappingList rInputButtonMappingList;
			if(m_oInputButtonMappingLists.TryGetValue(a_oButtonName, out rInputButtonMappingList))
			{
				return rInputButtonMappingList.GetButton();
			}
			else
			{
				return false;
			}
		}
		
		#if UNITY_EDITOR
		public void EditorOnly_AutoFill()
		{
			if(inputButtonMappingLists == null)
			{
				inputButtonMappingLists = new List<gkInputButtonMappingList>();
			}
			else
			{
				inputButtonMappingLists.Clear();
			}
			
			inputButtonMappingLists.AddRange(GetComponentsInChildren<gkInputButtonMappingList>());
			foreach(gkInputButtonMappingList rInputButtonMappingList in inputButtonMappingLists)
			{
				rInputButtonMappingList.EditorOnly_AutoFill();
			}
		}
		#endif
		
		private void Awake()
		{
			GetButtons();
		}
		
		private void GetButtons()
		{
			foreach(gkInputButtonMappingList rInputButtonMappingList in inputButtonMappingLists)
			{
				m_oInputButtonMappingLists.Add(rInputButtonMappingList.name, rInputButtonMappingList);
			}
		}
	}
}