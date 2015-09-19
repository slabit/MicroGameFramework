using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[AddComponentMenu("GK/Transition/BaseTransition/gkTransition")]
	public abstract class gkTransition : MonoBehaviour
	{
		public float transitionDuration = 1.0f;
		
		public float defaultTransitionSpeed = 1.0f;
		
		public bool playTransitionAtStart = false;
		
		public bool destroyOnEnd = false;
		
		private Action m_oPlayTransitionEndHandler;
		
		private bool m_bPlayingTransition;
		
		private float m_fTransitionPercentTarget;
		
		public virtual float TransitionTime
		{
			get
			{
				return GetTransitionTime();
			}
			
			set
			{
				SetTransitionTime(value);
			}
		}
		
		public bool IsPlaying
		{
			get
			{
				return m_bPlayingTransition;
			}
		}
		
		public float TransitionPercent
		{
			get
			{
				if(transitionDuration == 0.0f)
				{
					if(TransitionSpeed > 0)
					{
						return 1.0f;
					}
					else
					{
						return 0.0f;
					}
				}
				else
				{
					return GetTransitionTime()/transitionDuration;
				}
			}
			
			set
			{
				SetTransitionTime(value * transitionDuration);
			}
		}
		
		public float TransitionDuration
		{
			get
			{
				return transitionDuration;
			}
			
			set
			{
				if(value != transitionDuration)
				{
					// Change the duration but keep the current transition percent to ensure continuity
					float fTransfitionPercent = TransitionPercent;
					transitionDuration = value;
					TransitionPercent = fTransfitionPercent;
				}
			}
		}
		
		public virtual float TransitionSpeed
		{
			get
			{
				return GetTransitionSpeed();
			}
			
			set
			{
				SetTransitionSpeed(value);
			}
		}
		
		public static TransitionType CreateTransition<TransitionType>(GameObject a_rGameObject) where TransitionType : gkTransition
		{
			TransitionType rTransition;
			
			if(a_rGameObject == null)
			{
				rTransition = gkComponentBuilderUtility.BuildComponent<TransitionType>();
			}
			else
			{
				rTransition = a_rGameObject.AddComponent<TransitionType>();
			}
			
			return rTransition;
		}
		
		public void PlayTransitionFromStart(Action a_rOnPlayEnd = null)
		{
			TransitionPercent = 0.0f;
			PlayTransition(defaultTransitionSpeed, a_rOnPlayEnd);
		}
		
		public void PlayTransition(Action a_rOnPlayEnd = null)
		{
			PlayTransition(defaultTransitionSpeed, a_rOnPlayEnd);
		}
		
		public void PlayTransitionInReverse(Action a_rOnPlayEnd = null)
		{
			PlayTransition(-defaultTransitionSpeed, a_rOnPlayEnd);
		}
		
		public void PlayTransitionTo(float a_fTargetTransitionPercent, Action a_rOnPlayEnd = null)
		{
			float fTransitionSpeed = Mathf.Abs(defaultTransitionSpeed);
			float fCurrentTransitionPercent = TransitionPercent;
			if(a_fTargetTransitionPercent < fCurrentTransitionPercent)
			{
				fTransitionSpeed = -fTransitionSpeed;
			}
			
			PlayTransition(a_fTargetTransitionPercent, fTransitionSpeed, a_rOnPlayEnd);
		}
		
		public void PlayTransition(float a_fSpeed, Action a_rOnPlayEnd = null)
		{
			float fTransitionPercentTarget = 1.0f;
			if(a_fSpeed < 0.0f)
			{
				fTransitionPercentTarget = 0.0f;
			}
			PlayTransition(fTransitionPercentTarget, a_fSpeed, a_rOnPlayEnd);
		}
		
		public void StopInterpolation()
		{
			m_bPlayingTransition = false;
			m_oPlayTransitionEndHandler = null;
			OnStopTransition();
		}
		
		protected virtual void OnPlayTransition()
		{
		}
		
		protected virtual void OnStopTransition()
		{
		}
		
		protected virtual float GetTransitionTime()
		{
			return 0.0f;
		}
		
		protected virtual void SetTransitionTime(float a_fTime)
		{
		}
		
		protected virtual float GetTransitionSpeed()
		{
			return 0.0f;
		}
		
		protected virtual void SetTransitionSpeed(float a_fSpeed)
		{
		}
		
		protected virtual void UpdateTransitionTime(float a_fDeltaTime)
		{
		}
		
		protected virtual void BeforeInitializeTransition()
		{
		}
		
		protected virtual void OnInitializeTransition()
		{
		}
		
		protected void InitializeTransitionBase(float a_fDuration)
		{	
			transitionDuration = a_fDuration;
			BeforeInitializeTransition();
			OnInitializeTransition();
		}
		
		protected virtual void Start()
		{
			if(playTransitionAtStart)
			{
				PlayTransition();
			}
		}
		
		protected virtual void Update()
		{
			if(m_bPlayingTransition)
			{
				UpdateTransition(gkTime.DeltaTime);
			}
		}
		
		private void PlayTransition(float a_fTransitionPercentTarget, float a_fSpeed, Action a_rOnPlayEnd = null)
		{
			m_bPlayingTransition = true;
			m_oPlayTransitionEndHandler = a_rOnPlayEnd;
			m_fTransitionPercentTarget = a_fTransitionPercentTarget;
			SetTransitionSpeed(a_fSpeed);
			OnPlayTransition();
			UpdateTransition(0.0f);
		}
		
		private void UpdateTransition(float a_fDeltaTime)
		{
			// Ask child for speed
			float fTransitionSpeed = TransitionSpeed;
			
			// update
			UpdateTransitionTime(a_fDeltaTime * fTransitionSpeed);
			
			// Get and clamp
			float fTransitionPercent = TransitionPercent;
			fTransitionPercent = Mathf.Clamp01(fTransitionPercent);

			// target reached?
			bool bTransitionTargetReached = false;	
			if(fTransitionSpeed == 0.0f)
			{
				if(fTransitionPercent == m_fTransitionPercentTarget)
				{
					bTransitionTargetReached = true;
				}
			}
			else if(fTransitionSpeed <= 0.0f)
			{
				if(fTransitionPercent <= m_fTransitionPercentTarget)
				{
					bTransitionTargetReached = true;
				}
			}
			else
			{
				if(fTransitionPercent >= m_fTransitionPercentTarget)
				{
					bTransitionTargetReached = true;
				}
			}
			
			// End transition if target reached
			if(bTransitionTargetReached)
			{
				TransitionPercent = m_fTransitionPercentTarget;
				TransitionPlayEnd();
			}
		}
		
		private void TransitionPlayEnd()
		{
			OnStopTransition();
			m_bPlayingTransition = false;
			
			if(m_oPlayTransitionEndHandler != null)
			{
				m_oPlayTransitionEndHandler();
				m_oPlayTransitionEndHandler = null;
			}
			if(destroyOnEnd)
			{
				Destroy(this);
			}
		}
	}
}