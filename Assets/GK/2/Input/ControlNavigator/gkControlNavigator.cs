using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{	
	[AddComponentMenu("GK/Input/gkControlNavigator")]
	public class gkControlNavigator : MonoBehaviour
	{		
		[Serializable]
		public class TransitionInput
		{
			public string transitionName;
			
			public gkButton input;
			
			private gkControlNavigator m_rNavigator;
			
			public TransitionInput(string a_oTransitionName, gkButton a_rInput)
			{
				input = a_rInput;
				transitionName = a_oTransitionName;
			}
			
			public void Initialize(gkControlNavigator a_rNavigator)
			{
				m_rNavigator = a_rNavigator;
				if(input != null)
				{
					input.onDown += OnInput;
				}
			}
			
			private void OnInput()
			{
				m_rNavigator.ExecuteTransition(transitionName);
			}
		}
		
		public gkButton left;
		public gkButton right;
		public gkButton up;
		public gkButton down;
		
		public List<TransitionInput> additionalInputs;
		
		public gkControlNavigator_Control firstControlToSelect;
		
		public bool selectControlAtStart = false;
		
		private gkControlNavigator_Control m_rSelectedControl;
		
		private List<TransitionInput> m_oTransitionInputs = new List<TransitionInput>();
		
		public gkControlNavigator_Control SelectedControl
		{
			get
			{
				return m_rSelectedControl;
			}
		}
		
		public bool Select(gkControlNavigator_Control a_rControl)
		{
			if(a_rControl != null && (a_rControl.enabled == false || a_rControl.gameObject.activeInHierarchy == false))
			{
				return false;
			}
			
			if(m_rSelectedControl != null)
			{
				m_rSelectedControl.OnSelect(this, false);
			}
			
			m_rSelectedControl = a_rControl;
			
			if(m_rSelectedControl != null)
			{
				m_rSelectedControl.OnSelect(this, true);
				return true;
			}
			
			return false;
		}
		
		public void ExecuteTransition(string a_oTransitionName)
		{
			if(m_rSelectedControl == null)
			{
				Select(firstControlToSelect);
				return;
			}
			
			if(m_rSelectedControl != null)
			{
				m_rSelectedControl.ExecuteTransition(a_oTransitionName);
			}
		}
		
		private void Awake()
		{
			FillDictionary();
			
			foreach(TransitionInput rTransitionInput in m_oTransitionInputs)
			{
				rTransitionInput.Initialize(this);
			}
		}
		
		private void Start()
		{
			if(selectControlAtStart)
			{
				Select(firstControlToSelect);	
			}
		}
		
		private void OnDisable()
		{
			Select(null);
		}
		
		private void FillDictionary()
		{
			m_oTransitionInputs.Clear();
			
			m_oTransitionInputs.Add(new TransitionInput("left", left));
			m_oTransitionInputs.Add(new TransitionInput("right", right));
			m_oTransitionInputs.Add(new TransitionInput("up", up));
			m_oTransitionInputs.Add(new TransitionInput("down", down));
			m_oTransitionInputs.AddRange(additionalInputs);
		}
	}
}