using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[AddComponentMenu("GK/Input/gkInputButtonMapping")]
	public abstract class gkInputButtonMapping : MonoBehaviour
	{	
		public virtual bool GetButtonDown()
		{
			return false;
		}
		
		public virtual bool GetButton()
		{
			return false;
		}
	}
}