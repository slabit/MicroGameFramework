using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[AddComponentMenu("GK/Input/gkControlNavigator_Control")]
	public class gkControlNavigator_Control : MonoBehaviour
	{	
		[Serializable]
		public class Transition
		{
			public gkControlNavigator_Control control;
			
			public string transitionName;
			
			public Transition(string a_oTransitionName, gkControlNavigator_Control a_rControl)
			{
				control = a_rControl;
				
				transitionName = a_oTransitionName;
			}
		}
		
		public System.Action<bool> onSelect;
		
		public System.Action<string> onTransitionTo;
		
		public gkControlNavigator_Control left;
		public gkControlNavigator_Control right;
		public gkControlNavigator_Control up;
		public gkControlNavigator_Control down;
		
		public List<Transition> additionalTransitions;
		
		public List<gkControlNavigator_Control> children;
		public int startChildIndex = 0;
		
		private Dictionary<string, Transition> m_oTransitionByTransitionName = new Dictionary<string, Transition>(); 
		private List<Transition> m_oTransitions = new List<Transition>();
		
		private gkControlNavigator_Control m_rParentControl;
		
		private gkControlNavigator_Control m_rSelectedChildControl;
		
		private bool m_bSelected;
		
		private gkControlNavigator_Control m_rLastSelectedChildControl;
		
		private gkControlNavigator m_rControlNavigator;
		
		public gkControlNavigator_Control ParentControl
		{
			get
			{
				return m_rParentControl;
			}
		}
		
		public bool Selected
		{
			get
			{
				return m_bSelected;
			}
		}
		
		public gkControlNavigator_Control LastSelectedChildControl
		{
			get
			{
				return m_rLastSelectedChildControl;
			}
			
			set
			{
				m_rLastSelectedChildControl = value;
			}
		}
		
		private gkControlNavigator_Control SelectedChildControl
		{
			get
			{
				return m_rSelectedChildControl;
			}
			
			set
			{
				m_rSelectedChildControl = value;
				if(value != null)
				{
					m_rLastSelectedChildControl = value;
				}
			}
		}
		
		public void SelectChild(int a_iChildIndex)
		{
			if(a_iChildIndex >= 0 && a_iChildIndex < children.Count)
			{
				SelectControl(children[a_iChildIndex]);
			}
		}
		
		public void OnSelect(gkControlNavigator a_rControlNavigator, bool a_bSelect)
		{
			m_rControlNavigator = a_rControlNavigator;
			m_bSelected = a_bSelect;
			if(m_rParentControl != null)
			{
				m_rParentControl.OnChildSelect(a_bSelect, this);
			}
			
			if(a_bSelect)
			{
				SelectChild();
			}
			
			if(onSelect != null)
			{
				onSelect(a_bSelect);
			}
		}
		
		public void ExecuteTransition(string a_oTransitionName)
		{	
			if(ProcessChildTransition(a_oTransitionName))
			{
				return;
			}
			
			if(ProcessOwnTransition(a_oTransitionName))
			{
				return;
			}
			
			ProcessParentTransition(a_oTransitionName);
		}
		
		private void Start()
		{
			FillDictionary();
			
			foreach(gkControlNavigator_Control rChild in children)
			{
				rChild.SetParent(this);
			}
		}
		
		private void SetParent(gkControlNavigator_Control a_rParent)
		{
			m_rParentControl = a_rParent;
		}
		
		private void OnChildSelect(bool a_bSelect, gkControlNavigator_Control a_rChild)
		{
			if(a_bSelect)
			{
				SelectedChildControl = a_rChild;
			}
			else
			{
				if(SelectedChildControl == a_rChild)
				{
					SelectedChildControl = null;
				}
			}
		}
		
		private void FillDictionary()
		{
			m_oTransitions.Clear();
			
			AddTransition("left", left);
			AddTransition("right", right);
			AddTransition("up", up);
			AddTransition("down", down);
			m_oTransitions.AddRange(additionalTransitions);
			
			m_oTransitionByTransitionName.Clear();
			
			foreach(Transition rTransition in m_oTransitions)
			{
				m_oTransitionByTransitionName.Add(rTransition.transitionName, rTransition);
			}
		}
		
		private void AddTransition(string a_oTransitionName, gkControlNavigator_Control a_rControl)
		{
			if(a_rControl != null)
			{
				m_oTransitions.Add(new Transition(a_oTransitionName, a_rControl));
			}
		}
		
		private bool ProcessOwnTransition(string a_oTransitionName)
		{
			Transition rTransition;
			if(m_oTransitionByTransitionName.TryGetValue(a_oTransitionName, out rTransition))
			{
				if(rTransition.control != null)
				{
					if(m_rControlNavigator.Select(rTransition.control))
					{
						if(rTransition.control.onTransitionTo != null)
						{
							rTransition.control.onTransitionTo(a_oTransitionName);
						}
						return true;
					}
				}
			}
			
			return false;
		}
		
		private void ProcessParentTransition(string a_oTransitionName)
		{
			if(m_rParentControl == null)
			{
				return;
			}
			
			if(m_rParentControl.ProcessOwnTransition(a_oTransitionName))
			{
				return;
			}
			
			m_rParentControl.ProcessParentTransition(a_oTransitionName);
		}
		
		private bool ProcessChildTransition(string a_oTransitionName)
		{
			if(SelectedChildControl != null)
			{
				return SelectedChildControl.ProcessOwnTransition(a_oTransitionName);
			}
			
			return false;
		}
		
		private bool SelectChild()
		{
			if(children == null)
			{
				return false;
			}
			
			gkControlNavigator_Control rFirstSelectableChildControl = GetFirstSelectableChild();
			
			if(SelectControl(rFirstSelectableChildControl))
			{
				return true;
			}
			
			return false;
		}
		
		private bool SelectControl(gkControlNavigator_Control rSelectControl)
		{
			if(rSelectControl != null && rSelectControl.Selected == false)
			{
				if(m_rControlNavigator.Select(rSelectControl))
				{
					return true;
				}
			}
			return false;
		}
		
		private gkControlNavigator_Control GetFirstSelectableChild()
		{
			if(m_rLastSelectedChildControl != null)
			{
				return m_rLastSelectedChildControl;
			}
			
			if(children == null)
			{
				return null;
			}
			
			gkControlNavigator_Control rSelectedChildControl = SelectedChildControl;
			if(rSelectedChildControl == null)
			{
				if(startChildIndex >= 0 && startChildIndex < children.Count)
				{
					rSelectedChildControl = children[startChildIndex];
				}
			}
			
			return rSelectedChildControl;
		}
	}
}