using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[AddComponentMenu("GK/Input/gkControlNavigator_ChildControlSelectionOverride")]
	public class gkControlNavigator_ChildControlSelectionOverride : MonoBehaviour
	{	
		[Serializable]
		public class ChildControlSelectionOverride
		{
			public string transitionName;
			public int childIndex;
			public gkControlNavigator_Control controlledControlOverride;
		}
		
		public List<ChildControlSelectionOverride> childControlSelectionOverrides; 
		
		public gkControlNavigator_Control control;
		
		private Dictionary<string , ChildControlSelectionOverride> m_oChildControlSelectionOverrideByName = new Dictionary<string, ChildControlSelectionOverride>();
		
		private void Awake()
		{
			control.onTransitionTo += OnTransitionTo;
			FillDictionary();
		}
		
		private void OnTransitionTo(string a_oTransitionName)
		{
			ChildControlSelectionOverride rTransition;
			if(m_oChildControlSelectionOverrideByName.TryGetValue(a_oTransitionName, out rTransition))
			{
				gkControlNavigator_Control rControlledControl = rTransition.controlledControlOverride;
				if(rControlledControl == null)
				{
					rControlledControl = control;
				}
				
				if(rControlledControl == null || rControlledControl.enabled == false || rControlledControl.gameObject.activeInHierarchy == false)
				{
					return;
				}
				
				rControlledControl.SelectChild(rTransition.childIndex);
			}
		}
		
		private void FillDictionary()
		{
			foreach(ChildControlSelectionOverride rTransitionOverride in childControlSelectionOverrides)
			{
				m_oChildControlSelectionOverrideByName.Add(rTransitionOverride.transitionName, rTransitionOverride);
			}
		}
	}
}