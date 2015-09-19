 using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[AddComponentMenu("GK/Input/ButtonHub/gkButtonHubController")]
	public class gkButtonHubController : MonoBehaviour
	{
		public gkButton controlledButton;
		
		public List<gkButton> inputButtons;
		
		private void Awake()
		{
			controlledButton.onEnable += OnControlledButtonEnable;
			UpdateControlledButtonState(false);
			foreach(gkButton rButton in inputButtons)
			{
				if(rButton != null)
				{
					rButton.onStateChange += OnButtonStateChange;
				}
			}
		}
		
		private void OnDestroy()
		{
			foreach(gkButton rButton in inputButtons)
			{
				if(rButton != null)
				{
					rButton.onStateChange -= OnButtonStateChange;
				}
			}
			controlledButton.onEnable -= OnControlledButtonEnable;
		}

		void OnControlledButtonEnable()
		{
			UpdateControlledButtonState(false);
		}
		
		private void OnButtonStateChange(gkButton a_rButton, bool a_bCanceled)
		{
			if(IsDeactivated())
			{
				return;
			}
			UpdateControlledButtonState(a_bCanceled);
		}
		
		private void UpdateControlledButtonState(bool a_bCanceled)
		{
			bool bButtonPressed = IsAtLeastAButtonPressed();
			if(controlledButton.Pressed)
			{
				if(bButtonPressed == false)
				{
					if(a_bCanceled)
					{
						controlledButton.Cancel();
					}
					else
					{
						controlledButton.Release();	
					}
				}
			}
			else
			{
				if(bButtonPressed)
				{
					controlledButton.Press();
				}
			}
		}
		
		private bool IsAtLeastAButtonPressed()
		{
			foreach(gkButton rButton in inputButtons)
			{
				if(rButton != null && rButton.Pressed)
				{
					return true;
				}
			}
			
			return false;
		}

		private bool IsDeactivated()
		{
			return controlledButton.enabled == false || controlledButton.gameObject.activeInHierarchy == false || enabled == false || gameObject.activeInHierarchy == false;
		}
	}
}