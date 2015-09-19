using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[AddComponentMenu("GK/Interpolation/Template/gkFloatInterpolationTemplate")]
	public class gkFloatInterpolationTemplate : gkFloatInterpolation 
	{
		protected override void BeforeInitializeTransition()
		{
		}
		
		protected override void OnInterpolatedValueChange(float a_fInterpolatedValue)
		{
		}
	}
}