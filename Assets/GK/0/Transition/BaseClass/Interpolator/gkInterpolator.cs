using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[Serializable]
	public class gkInterpolator
	{
		public Action onInterpolatedValueChange;
		
		public gkInterpolate.EaseType interpolationType;
			
		private float m_fCurrentPercent;
		
		public float InterpolationPercent
		{
			get
			{
				return m_fCurrentPercent;
			}
			
			set
			{
				float fNewPercent = Mathf.Clamp01(value);
				if(fNewPercent != m_fCurrentPercent)
				{
					m_fCurrentPercent = fNewPercent;
					if(UpdateInterpolatedValue())
					{
						OnInterpolatedValueChange();
					}
				}
			}
		}
		
		public void InitializeInterpolation(float a_fStartPercent, gkInterpolate.EaseType a_eInterpolationType = gkInterpolate.EaseType.Linear)
		{
			interpolationType = a_eInterpolationType;
			m_fCurrentPercent = Mathf.Clamp01(a_fStartPercent);
			InitializeInterpolatedValue();
		}
		
		protected virtual bool UpdateInterpolatedValue()
		{
			return true;
		}
		
		private void InitializeInterpolatedValue()
		{
			UpdateInterpolatedValue();
			OnInterpolatedValueChange();
		}
		
		private void OnInterpolatedValueChange()
		{
			if(onInterpolatedValueChange != null)
			{
				onInterpolatedValueChange();
			}
		}
	}
}
