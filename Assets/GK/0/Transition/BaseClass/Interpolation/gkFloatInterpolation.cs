using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[AddComponentMenu("GK/Transition/BaseTransition/gkFloatTransition")]
	public abstract class gkFloatInterpolation : gkInterpolation<gkFloatInterpolator> 
	{	
		public void InitializeTransition(float a_fStartPercent, float a_fFrom, float a_fTo, float a_fDuration, gkInterpolate.EaseType a_eInterpolationType = gkInterpolate.EaseType.Linear)
		{
			InitializeTransitionBase(a_fDuration);
			interpolation.InitializeInterpolation(a_fStartPercent, a_fFrom, a_fTo, a_eInterpolationType);
		}
		
		protected override void OnInterpolatedValueChange()
		{
			OnInterpolatedValueChange(interpolation.InterpolatedValue);
		}
		
		protected virtual void OnInterpolatedValueChange(float a_fInterpolatedValue)
		{
		}
	}
}