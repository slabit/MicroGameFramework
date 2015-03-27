using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[AddComponentMenu("GK/Input/gkInputButtonMappingLists")]
	public class gkInputButtonMappingList : MonoBehaviour
	{	
		public List<gkInputButtonMapping> inputButtonMappings;
		
		public bool GetButtonDown()
		{
			foreach(gkInputButtonMapping rButtonMapping in inputButtonMappings)
			{
				if(rButtonMapping.GetButtonDown())
				{
					return true;
				}
			}
			
			return false;
		}
		
		public bool GetButton()
		{
			foreach(gkInputButtonMapping rButtonMapping in inputButtonMappings)
			{
				if(rButtonMapping.GetButton())
				{
					return true;
				}
			}
			
			return false;
		}
		
		#if UNITY_EDITOR
		public void EditorOnly_AutoFill()
		{
			if(inputButtonMappings == null)
			{
				inputButtonMappings = new List<gkInputButtonMapping>();
			}
			else
			{
				inputButtonMappings.Clear();
			}
			
			inputButtonMappings.AddRange(GetComponentsInChildren<gkInputButtonMapping>());
		}
		#endif
	}
}