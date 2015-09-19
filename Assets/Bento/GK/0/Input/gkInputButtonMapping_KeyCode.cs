using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[AddComponentMenu("GK/Input/gkInputButtonMapping_KeyCode")]
	public class gkInputButtonMapping_KeyCode : gkInputButtonMapping
	{	
		public KeyCode keyCode;
		
		public override bool GetButtonDown()
		{
			return Input.GetKeyDown(keyCode);
		}
		
		public override bool GetButton()
		{
			return Input.GetKey(keyCode);
		}
	}
}