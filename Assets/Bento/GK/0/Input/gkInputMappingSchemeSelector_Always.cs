using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[AddComponentMenu("GK/Input/gkInputMappingSchemeSelector_Always")]
	public class gkInputMappingSchemeSelector_Always : gkInputMappingSchemeSelector_Base
	{	
		protected override bool ShouldSelect()
		{
			return true;
		}
	}
}