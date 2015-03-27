using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[AddComponentMenu("GK/Input/gkInputMappingSchemeSelector_ByRuntimePlatform")]
	public class gkInputMappingSchemeSelector_ByRuntimePlatform : gkInputMappingSchemeSelector_Base
	{	
		public List<RuntimePlatform> runtimePlatforms = new List<RuntimePlatform>();
		protected override bool ShouldSelect()
		{
			return runtimePlatforms.Contains(Application.platform);
		}
	}
}