using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[AddComponentMenu("GK/Transition/Template/gkTransitionTemplate")]
	public class gkTransitionTemplate : gkTransition
	{
		protected override float GetTransitionTime()
		{
			return 0.0f;
		}
		
		protected override void SetTransitionTime(float a_fTime)
		{
		}
		
		protected override float GetTransitionSpeed()
		{
			return 0.0f;
		}
		
		protected override void SetTransitionSpeed(float a_fSpeed)
		{
		}
		
		protected override void UpdateTransitionTime(float a_fDeltaTime)
		{
		}
	}
}