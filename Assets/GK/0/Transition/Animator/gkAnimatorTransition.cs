using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[AddComponentMenu("GK/Transition/Animator/gkAnimatorTransition")]
	public class gkAnimatorTransition : gkTransition
	{
		public List<Animator> animatorComponents = new List<Animator>();
		
		public List<AnimationClip> animationClips = new List<AnimationClip>();
		
		public float animationLength = 1.0f;

		public bool manageActivation = true;

		private float m_fTransitionSpeed = 0.0f;
		
		private float m_fTransitionTime = 0.0f;
		
		public static gkAnimatorTransition CreateTransition(GameObject a_rGameObject)
		{
			return gkTransition.CreateTransition<gkAnimatorTransition>(a_rGameObject);
		}
		
		public void InitializeTransition(float a_fStartPercent, AnimationClip a_rAnimationClip)
		{
			Animator rAnimator = GetComponent<Animator>();
			
			float fDuration = a_rAnimationClip.length;
			InitializeTransitionBase(fDuration);
			
			animationLength = a_rAnimationClip.length;
			animationClips.Add(a_rAnimationClip);
			animatorComponents.Add(rAnimator);
			
			InitializeAtStartPercent(a_fStartPercent);
		}
		
		public void InitializeTransition(float a_fStartPercent, Animator a_rAnimatorComponent, AnimationClip a_rAnimationClip)
		{
			float fDuration = a_rAnimationClip.length;
			InitializeTransitionBase(fDuration);
			
			animationLength = a_rAnimationClip.length;
			animationClips.Add(a_rAnimationClip);
			animatorComponents.Add(a_rAnimatorComponent);
			
			InitializeAtStartPercent(a_fStartPercent);
		}
		
		public void InitializeTransition(float a_fStartPercent, Animator a_rAnimatorComponent, AnimationClip a_rAnimationClip, float a_fAnimationLength)
		{
			float fDuration = a_fAnimationLength;
			InitializeTransitionBase(fDuration);
			
			animationLength = a_fAnimationLength;
			animationClips.Add(a_rAnimationClip);
			animatorComponents.Add(a_rAnimatorComponent);
			
			InitializeAtStartPercent(a_fStartPercent);
		}
		
		public void InitializeTransition(float a_fStartPercent,  AnimationClip a_rAnimationClip, float a_fDuration)
		{
			Animator rAnimator = GetComponent<Animator>();
			
			InitializeTransitionBase(a_fDuration);
			
			animationLength = a_rAnimationClip.length;
			animationClips.Add(a_rAnimationClip);
			animatorComponents.Add(rAnimator);
			
			InitializeAtStartPercent(a_fStartPercent);
		}
		
		public void InitializeTransition(float a_fStartPercent, Animator a_rAnimatorComponent)
		{
			a_rAnimatorComponent.Play(0);
			a_rAnimatorComponent.Update(0.0f);
			float fDuration = a_rAnimatorComponent.GetCurrentAnimatorStateInfo(0).length;
			InitializeTransition(a_fStartPercent, fDuration, a_rAnimatorComponent);
		}
		
		public void InitializeTransition(float a_fStartPercent, float a_fDuration, Animator a_rAnimatorComponent)
		{
			InitializeTransitionBase(a_fDuration);
			
			animatorComponents.Add(a_rAnimatorComponent);
			
			InitializeAtStartPercent(a_fStartPercent);
		}
		
		public void InitializeTransition(float a_fStartPercent, float a_fDuration, Animator a_rAnimatorComponent, AnimationClip a_rAnimationClip)
		{
			InitializeTransitionBase(a_fDuration);
			
			animatorComponents.Add(a_rAnimatorComponent);
			animationClips.Add(a_rAnimationClip);
			
			InitializeAtStartPercent(a_fStartPercent);
		}
		
		public void InitializeTransition(float a_fStartPercent, float a_fDuration, List<Animator> a_rAnimatorComponents)
		{
			InitializeTransitionBase(a_fDuration);
			
			animatorComponents.AddRange(a_rAnimatorComponents);
			
			InitializeAtStartPercent(a_fStartPercent);
		}
		
		public void InitializeTransition(float a_fStartPercent, float a_fDuration, List<Animator> a_rAnimatorComponents, List<AnimationClip> a_rAnimationClips)
		{
			InitializeTransitionBase(a_fDuration);
			
			animatorComponents.AddRange(a_rAnimatorComponents);
			animationClips.AddRange(a_rAnimationClips);
			
			InitializeAtStartPercent(a_fStartPercent);
		}
		
		protected override void OnInitializeTransition()
		{
			ResetAttributes();
		}
		
		protected override void OnPlayTransition()
		{
			PlayAnimators();
		}
		
		protected override void OnStopTransition()
		{
			StopAnimators();
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
			
			int iAnimatorsCount = animatorComponents.Count;
			for(int i = 0; i<iAnimatorsCount; i++)
			{
				SetAnimatorNormalizedTime(i, fNormalizedTime);
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
			if(animatorComponents == null)
			{
				animatorComponents = new List<Animator>();
			}
			else
			{
				animatorComponents.Clear();
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
			StopAnimators();
			TransitionPercent = a_fStartPercent;
		}
		
		private bool GetAnimatorControllers(int a_iIndex, out Animator a_rAnimatorComponent, out AnimatorStateInfo a_rAnimatorStateInfo, out AnimationClip a_rAnimationClip)
		{
			a_rAnimatorStateInfo = new AnimatorStateInfo();
			a_rAnimatorComponent = null;
			a_rAnimationClip = null;
			
			if(a_iIndex < animationClips.Count)
			{
				a_rAnimationClip = animationClips[a_iIndex];
			}
			
			if(a_iIndex >= animatorComponents.Count)
			{
				return false;
			}
			
			a_rAnimatorComponent = animatorComponents[a_iIndex];
			if(a_rAnimatorComponent == null)
			{
				return false;
			}
			
			a_rAnimatorStateInfo = a_rAnimatorComponent.GetCurrentAnimatorStateInfo(0);
			return true;
		}
		
		private void SetAnimatorNormalizedTime(int a_iIndex, float a_fNormalizedTime)
		{
			Animator rAnimatorComponent;
			AnimatorStateInfo rAnimatorStateInfo;
			AnimationClip rAnimationClip;
			if(GetAnimatorControllers(a_iIndex, out rAnimatorComponent, out rAnimatorStateInfo, out rAnimationClip))
			{	
				if(TransitionDuration == 0.0f)
				{
					Play(rAnimatorComponent, rAnimationClip, 0.0f);
					rAnimatorComponent.Update(a_fNormalizedTime * animationLength);
				}		
				else
				{
					Play(rAnimatorComponent, rAnimationClip, a_fNormalizedTime);
					rAnimatorComponent.Update(0.0f);
				}
			}
		}
		
		private void PlayAnimators()
		{
			int iAnimatorsCount = animatorComponents.Count;
			for(int i = 0; i<iAnimatorsCount; i++)
			{
				PlayAnimator(i);
			}
			SetTransitionTime(m_fTransitionTime);
		}
		
		private void StopAnimators()
		{
			int iAnimatorsCount = animatorComponents.Count;
			for(int i = 0; i<iAnimatorsCount; i++)
			{
				StopAnimator(i);
			}
		}
		
		private void PlayAnimator(int a_iIndex)
		{
			Animator rAnimatorComponent;
			AnimatorStateInfo rAnimatorStateInfo;
			AnimationClip rAnimationClip;
			if(GetAnimatorControllers(a_iIndex, out rAnimatorComponent, out rAnimatorStateInfo, out rAnimationClip))
			{
				if(manageActivation)
				{
					rAnimatorComponent.enabled = true;
				}
				if(transitionDuration == 0.0f)
				{
					rAnimatorComponent.speed = Math.Sign(TransitionSpeed);
				}
				else
				{
					rAnimatorComponent.speed = (animationLength * TransitionSpeed)/transitionDuration;
				}
				Play(rAnimatorComponent, rAnimationClip);
			}
		}
		
		private void StopAnimator(int a_iIndex)
		{
			Animator rAnimatorComponent;
			AnimatorStateInfo rAnimatorStateInfo;
			AnimationClip rAnimationClip;
			if(GetAnimatorControllers(a_iIndex, out rAnimatorComponent, out rAnimatorStateInfo, out rAnimationClip))
			{	
				rAnimatorComponent.speed = 0.0f;

				Play(rAnimatorComponent, rAnimationClip);
				if(manageActivation)
				{
					rAnimatorComponent.enabled = false;
				}
			}
		}

		private void Play(Animator animator, AnimationClip animationClip)
		{
			if(animationClip == null)
			{
				animator.Play(0);
			}
			else
			{
				animator.Play(animationClip.name);
			}
		}

		private void Play(Animator animator, AnimationClip animationClip, float normalizedTime)
		{
			if(animationClip == null)
			{
				animator.Play(0, 0, normalizedTime);
			}
			else
			{
				animator.Play(animationClip.name, 0, normalizedTime);
			}
		}
	}
}