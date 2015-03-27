using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[Serializable]
	public class gkFloatInterpolator : gkInterpolator
	{
		public float from = 0.0f;
		
		public float to = 1.0f;
			
		private float m_fCurrentInterpolatedValue;
			
		public float InterpolatedValue
		{
			get
			{
				return m_fCurrentInterpolatedValue;
			}
		}
		
		public void InitializeInterpolation(float a_fStartPercent, float a_fFrom, float a_fTo, gkInterpolate.EaseType a_eInterpolationType = gkInterpolate.EaseType.Linear)
		{
			from = a_fFrom;
			to = a_fTo;
			InitializeInterpolation(a_fStartPercent, a_eInterpolationType);
		}
		
		
		protected override bool UpdateInterpolatedValue()
		{
			float fNewInterpolatedValue = ComputeInterpolatedValue();
			if(fNewInterpolatedValue != m_fCurrentInterpolatedValue)
			{
				m_fCurrentInterpolatedValue = fNewInterpolatedValue;
				return true;
			}
			return false;
		}
		
		private float ComputeInterpolatedValue()
		{
			return gkInterpolate.Ease(interpolationType)(from, to - from, InterpolationPercent, 1.0f);
		}
	}
}