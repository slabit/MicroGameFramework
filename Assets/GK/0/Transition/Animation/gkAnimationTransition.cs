using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[AddComponentMenu("GK/Transition/Animation/gkAnimationTransition")]
	public class gkAnimationTransition : gkTransition
	{
		public List<Animation> animationComponents;
		
		public List<AnimationClip> animationClips;
		
		private float m_fTransitionSpeed = 0.0f;
		
		private float m_fTransitionTime = 0.0f;
		
		public static gkAnimationTransition CreateTransition(GameObject a_rGameObject)
		{
			return gkTransition.CreateTransition<gkAnimationTransition>(a_rGameObject);
		}
		
		public void InitializeTransition(float a_fStartPercent)
		{
			float fDuration = GetComponent<Animation>()[GetComponent<Animation>().clip.name].length;
			InitializeTransitionBase(fDuration);
			
			animationComponents.Add(GetComponent<Animation>());
			
			InitializeAtStartPercent(a_fStartPercent);
		}
		
		public void InitializeTransition(float a_fStartPercent, float a_fDuration)
		{
			InitializeTransitionBase(a_fDuration);
			
			animationComponents.Add(GetComponent<Animation>());
			
			InitializeAtStartPercent(a_fStartPercent);
		}
		
		public void InitializeTransition(float a_fStartPercent, Animation a_rAnimationComponent)
		{
			float fDuration = a_rAnimationComponent[a_rAnimationComponent.clip.name].length;
			InitializeTransition(a_fStartPercent, fDuration, a_rAnimationComponent);
		}
		
		public void InitializeTransition(float a_fStartPercent, float a_fDuration, Animation a_rAnimationComponent)
		{
			InitializeTransitionBase(a_fDuration);
			
			animationComponents.Add(a_rAnimationComponent);
			
			InitializeAtStartPercent(a_fStartPercent);
		}
		
		public void InitializeTransition(float a_fStartPercent, float a_fDuration, Animation a_rAnimationComponent, AnimationClip a_rAnimationClip)
		{
			InitializeTransitionBase(a_fDuration);
			
			animationComponents.Add(a_rAnimationComponent);
			animationClips.Add(a_rAnimationClip);
			
			InitializeAtStartPercent(a_fStartPercent);
		}
		
		public void InitializeTransition(float a_fStartPercent, float a_fDuration, List<Animation> a_rAnimationComponents)
		{
			InitializeTransitionBase(a_fDuration);
			
			animationComponents.AddRange(a_rAnimationComponents);
			
			InitializeAtStartPercent(a_fStartPercent);
		}
		
		public void InitializeTransition(float a_fStartPercent, float a_fDuration, List<Animation> a_rAnimationComponents, List<AnimationClip> a_rAnimationClips)
		{
			InitializeTransitionBase(a_fDuration);
			
			animationComponents.AddRange(a_rAnimationComponents);
			animationClips.AddRange(a_rAnimationClips);
			
			InitializeAtStartPercent(a_fStartPercent);
		}
		
		protected override void OnInitializeTransition()
		{
			ResetAttributes();
		}
		
		protected override void OnPlayTransition()
		{
			PlayAnimations();
		}
		
		protected override void OnStopTransition()
		{
			StopAnimations();
		}
		
		protected override float GetTransitionTime()
		{
			return m_fTransitionTime;
		}
		
		protected override void SetTransitionTime(float a_fTime)
		{
			m_fTransitionTime = a_fTime;
			
			float fNormalizedTime = 0.0f;
			if(transitionDuration == 0.0f)
			{
				if(a_fTime < 0.0f)
				{
					fNormalizedTime = 0.0f;
				}
				else
				{
					fNormalizedTime = 1.0f;
				}
			}
			else
			{
				fNormalizedTime = a_fTime/transitionDuration;
			}
			
			int iAnimationsCount = animationComponents.Count;
			for(int i = 0; i<iAnimationsCount; i++)
			{
				SetAnimationNormalizedTime(i, fNormalizedTime);
			}
		}
		
		protected override float GetTransitionSpeed()
		{
			return m_fTransitionSpeed;
		}
		
		protected override void SetTransitionSpeed(float a_fSpeed)
		{
			m_fTransitionSpeed = a_fSpeed;
		}
		
		protected override void UpdateTransitionTime(float a_fDeltaTime)
		{
			m_fTransitionTime += a_fDeltaTime;
		}
		
		private void ResetAttributes()
		{		
			if(animationComponents == null)
			{
				animationComponents = new List<Animation>();
			}
			else
			{
				animationComponents.Clear();
			}
			
			if(animationClips == null)
			{
				animationClips = new List<AnimationClip>();
			}
			else
			{
				animationClips.Clear();
			}
		}
		
		private void InitializeAtStartPercent(float a_fStartPercent)
		{
			StopAnimations();
			TransitionPercent = a_fStartPercent;
		}
		
		private bool GetAnimationControllers(int a_iIndex, out Animation a_rAnimationComponent, out AnimationState a_rAnimationState)
		{
			a_rAnimationState = null;
			a_rAnimationComponent = null;
			
			if(a_iIndex < animationComponents.Count)
			{
				a_rAnimationComponent = animationComponents[a_iIndex];
				if(a_rAnimationComponent != null)
				{
					// Try to get the animation clip
					AnimationClip rAnimationClip = null;
					if(a_iIndex < animationClips.Count)
					{
						rAnimationClip = animationClips[a_iIndex];
					}
					if(rAnimationClip == null)
					{
						rAnimationClip = a_rAnimationComponent.clip;
					}
					
					// If we have a animation clip
					if(rAnimationClip != null)
					{
						a_rAnimationState = a_rAnimationComponent[rAnimationClip.name];
						return true;
					}
				}
			}
			
			return false;
		}
		
		private void SetAnimationNormalizedTime(int a_iIndex, float a_fNormalizedTime)
		{
			Animation rAnimationComponent;
			AnimationState rAnimationState;
			if(GetAnimationControllers(a_iIndex, out rAnimationComponent, out rAnimationState))
			{			
				rAnimationComponent[rAnimationState.name].normalizedTime = a_fNormalizedTime;
			}
		}
		
		private void PlayAnimations()
		{
			int iAnimationsCount = animationComponents.Count;
			for(int i = 0; i<iAnimationsCount; i++)
			{
				PlayAnimation(i);
			}
			SetTransitionTime(m_fTransitionTime);
		}
		
		private void StopAnimations()
		{
			int iAnimationsCount = animationComponents.Count;
			for(int i = 0; i<iAnimationsCount; i++)
			{
				StopAnimation(i);
			}
		}
		
		private void PlayAnimation(int a_iIndex)
		{
			Animation rAnimationComponent;
			AnimationState rAnimationState;
			if(GetAnimationControllers(a_iIndex, out rAnimationComponent, out rAnimationState))
			{
				rAnimationState.speed = (rAnimationState.length * TransitionSpeed)/transitionDuration;
				rAnimationComponent.Play(rAnimationState.name);
			}
		}
		
		private void StopAnimation(int a_iIndex)
		{
			Animation rAnimationComponent;
			AnimationState rAnimationState;
			if(GetAnimationControllers(a_iIndex, out rAnimationComponent, out rAnimationState))
			{	
				rAnimationState.speed = 0.0f;
				rAnimationComponent.Play(rAnimationState.name);
			}
		}
	}
}