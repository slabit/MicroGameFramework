using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[AddComponentMenu("GK/Input/gkInputButtonMapping_Axis")]
	public class gkInputButtonMapping_Axis : gkInputButtonMapping
	{	
		public enum EAxisDirection
		{
			Positive,
			Negative
		}
		
		public string axisName;
		
		public EAxisDirection axisDirection;
		
		private bool m_bButtonWasPressed;
		
		private bool m_bButtonDown;
		
		public override bool GetButtonDown()
		{
			/*if(m_bButtonDown)
			{
				Debug.Log("GetButtonDown? " + axisName + ", " + axisDirection + " : " + m_bButtonDown);
			}*/
			return m_bButtonDown;
		}
		
		public override bool GetButton()
		{
			float fAxis = Input.GetAxis(axisName);
			bool bButtonPressed = false;
			switch(axisDirection)
			{
				case EAxisDirection.Positive:
				{
					bButtonPressed = fAxis > 0.0f;
				}
				break;
				
				case EAxisDirection.Negative:
				default:
				{
					bButtonPressed = fAxis < 0.0f;
				}
				break;
			}
			
			/*if(bButtonPressed)
			{
				Debug.Log("GetButton? " + axisName + ", " + axisDirection + " : " + bButtonPressed + ", " + fAxis);
			}*/
			
			return bButtonPressed;
		}

		public override float GetAxisValue()
		{
			return Input.GetAxis(axisName);
		}

		public void Update()
		{
			bool bButtonIsPressed = GetButton();
			
			m_bButtonDown = m_bButtonWasPressed == false && bButtonIsPressed;
			
			m_bButtonWasPressed = bButtonIsPressed;
		}
	}
}