 using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[AddComponentMenu("GK/Input/Key/gkKeyButtonController")]
	public class gkKeyButtonController : MonoBehaviour
	{
		public gkButton controlledButton;
		
		public List<string> keys;
		
		private bool m_bButtonEnabled;
		
		private void OnEnable()
		{
			OnButtonEnable();
		}
		
		private void OnDisable()
		{
			OnButtonDisable();
		}
		
		private void Update()
		{
			bool bButtonEnabled;
			if(controlledButton == null)
			{
				bButtonEnabled = true;
			}
			else
			{
				bButtonEnabled = controlledButton.enabled;
			}
			
			if(bButtonEnabled != m_bButtonEnabled)
			{
				if(bButtonEnabled)
				{
					OnButtonEnable();
				}
				else
				{
					OnButtonDisable();
				}
			}
			if(m_bButtonEnabled)
			{
				UpdateKey();
			}
		}
		
		private void UpdateKey()
		{		
			if(controlledButton.Pressed)
			{
				if(IsAtLeastAKeyPressed() == false)
				{
					controlledButton.Release();
				}
			}
			else
			{
				if(IsAtLeastAKeyDown())
				{
					controlledButton.Press();
				}
			}
		}
		
		private bool IsAtLeastAKeyPressed()
		{
			foreach(string oKey in keys)
			{
				if(gkInputManager.GetButton(oKey))
				{
					return true;
				}
			}
			
			return false;
		}
		
		private bool IsAtLeastAKeyDown()
		{
			foreach(string oKey in keys)
			{
				if(gkInputManager.GetButtonDown(oKey))
				{
					return true;
				}
			}
			
			return false;
		}
		
		private void OnButtonEnable()
		{
			m_bButtonEnabled = true;
		}
		
		private void OnButtonDisable()
		{
			Cancel();
		}
		
		public void Cancel()
		{
			CancelButton();
			m_bButtonEnabled = false;
		}
		
		private void CancelButton()
		{
			if(controlledButton != null)
			{
				controlledButton.Cancel();
			}
		}
	}
}