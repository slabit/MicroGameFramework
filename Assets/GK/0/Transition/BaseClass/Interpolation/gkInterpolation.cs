using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[AddComponentMenu("GK/Transition/BaseTransition/gkInterpolation")]
	public abstract class gkInterpolation<InterpolatorType> : gkTransition where InterpolatorType : gkInterpolator, new()
	{	
		public InterpolatorType interpolation = new InterpolatorType();
		
		private float m_fPlaySpeed = 1.0f;
		
		private bool m_bFirstInitialisation = true;
		
		protected override void OnInitializeTransition()
		{
			if(m_bFirstInitialisation)
			{
				m_bFirstInitialisation = false;
				interpolation.onInterpolatedValueChange += OnInterpolatedValueChange;
			}
		}
		
		protected virtual void OnInterpolatedValueChange()
		{
		}
		
		protected override float GetTransitionTime()
		{
			return interpolation.InterpolationPercent * transitionDuration;
		}
		
		protected override void SetTransitionTime(float a_fTime)
		{
			SetInterpolationTime(a_fTime);
		}
		
		protected override float GetTransitionSpeed()
		{
			return m_fPlaySpeed;
		}
		
		protected override void SetTransitionSpeed(float a_fSpeed)
		{
			m_fPlaySpeed = a_fSpeed;
		}
		
		protected override void UpdateTransitionTime(float a_fDeltaTime)
		{
			UpdateInterpolationTime(a_fDeltaTime);
		}
		
		private void UpdateInterpolationTime(float a_fDeltaTime)
		{
			if(transitionDuration == 0.0f)
			{
				if(a_fDeltaTime < 0.0f)
				{
					interpolation.InterpolationPercent = 0.0f;
				}
				else if(a_fDeltaTime > 0.0f)
				{
					interpolation.InterpolationPercent = 1.0f;
				}
				else
				{
					if(m_fPlaySpeed < 0.0f)
					{
						interpolation.InterpolationPercent = 0.0f;
					}
					else if(m_fPlaySpeed > 0.0f)
					{
						interpolation.InterpolationPercent = 1.0f;
					}
				}
			}
			else
			{
				interpolation.InterpolationPercent += a_fDeltaTime/transitionDuration;
			}
		}
		
		private void SetInterpolationTime(float a_fTime)
		{
			if(transitionDuration == 0.0f)
			{
				if(a_fTime < 0.0f)
				{
					interpolation.InterpolationPercent = 0.0f;
				}
				else if(a_fTime > 0.0f)
				{
					interpolation.InterpolationPercent = 1.0f;
				}
			}
			else
			{
				interpolation.InterpolationPercent = a_fTime/transitionDuration;
			}
		}
	}
}