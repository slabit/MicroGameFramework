using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	public abstract class gkInputMappingSchemeSelector_Base : MonoBehaviour
	{	
		protected abstract bool ShouldSelect();
		 
		private void Awake()
		{
			if(ShouldSelect())
			{
				gkInputManager.ActivateScheme(gameObject.name);
			}
		}
	}
}